using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.Models;
using ATG_Notifier.ViewModels.Models;
using ATG_Notifier.ViewModels.Networking;
using ATG_Notifier.ViewModels.Services;
using ATG_Notifier.ViewModels.ViewModels;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ATG_Notifier.Desktop.Services
{
    // TODO: Design the update service more flexible by
    //      - passing it the current source chapter Id instead of querrying the AppState
    //      - make the update interval configurable
    internal class UpdateService : IUpdateService
    {
#if DEBUG || DesktopPackage
        private const int RawSourcePollingInterval = 1 * 15 * 1000;
#else
        private const int RawSourcePollingInterval = 1* 30 * 1000;
#endif

        private readonly ILogService logService;
        private readonly IWebService webService;

        private readonly RawSourceChecker rawSourceChecker; 

        private readonly object runningLock = new object();
        private readonly object timerLock = new object();

        private Timer? periodicTimerRawSource = null;

        private Semaphore? saveguardSema;

        private string currentChapterId;

        public UpdateService(IWebService webService, ILogService logService)
        {
            this.webService = webService;
            this.logService = logService;

            var appState = ServiceLocator.Current.GetService<AppState>();
            this.currentChapterId = appState.CurrentChapterId ?? "";

            this.rawSourceChecker = new RawSourceChecker(this.webService, this.logService);
        }

        public event EventHandler<ChapterUpdateEventArgs>? ChapterUpdated;

        public event EventHandler? Started;

        public event EventHandler? Stopped;

        public bool IsRunning { get; private set; } = false;

        public void Start()
        {
            lock (this.runningLock)
            {
                if (this.IsRunning)
                {
                    return;
                }

                this.saveguardSema = new Semaphore(1, 1);

                this.periodicTimerRawSource = new Timer(OnTimerEllapsed, null, 0, Timeout.Infinite);

                this.IsRunning = true;
                Started?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Stop()
        {
            lock (this.runningLock)
            {
                if (!this.IsRunning)
                {
                    return;
                }

                logService.Log(LogType.Debug, "Attempting to terminate the update service...");

                // Wait until a running update process is finished.
                this.saveguardSema?.WaitOne();

                StopTimer();

                logService.Log(LogType.Debug, "The update service was terminated!");

                this.saveguardSema?.Release();

                // Clean-up resources used by the event handler.
                this.saveguardSema?.Dispose();
                this.saveguardSema = null;

                this.IsRunning = false;
                Stopped?.Invoke(this, EventArgs.Empty);
            }
        }

        private void StopTimer()
        {
            if (this.periodicTimerRawSource != null)
            {
                this.periodicTimerRawSource.Dispose();
                this.periodicTimerRawSource = null;
            }
        }

        private async void OnTimerEllapsed(object? state)
        {
#if DEBUG || DesktopPackage
            var chapterProfileModel = new ChapterProfileModel()
            {
                Number = 1600,
                Title = "Hello World1234567",
                NumberAndTitleFallbackString = "Fallback String",
                Url = "http://book.zongheng.com/chapter/408586/58484757.html",
                WordCount = 3000,
                ReleaseTime = DateTime.Now,
                AppArrivalTime = DateTime.Now
            };

            var chapterProfileViewModel = new ChapterProfileViewModel(chapterProfileModel);
            ChapterUpdated?.Invoke(this, new ChapterUpdateEventArgs("", chapterProfileViewModel));

#else
            ChapterSourceCheckResult? checkResult = null;
            try
            {
                checkResult = await rawSourceChecker.GetUpdateAsync(this.currentChapterId);
            }
            catch (SourceCheckerOperationFailedException ex)
            {
                logService.Log(LogType.Error, $"Checking for a raw source chapter update failed!\n Message: {ex.Message}\n" +
                    $"Exception: {ex.InnerException?.ToString() ?? ""}\nTechnical details: {ex.InnerException?.Message ?? ""}");
            }

            if (checkResult != null)
            {
                this.saveguardSema?.WaitOne();

                this.currentChapterId = checkResult.SourceChapterId;

                var chapterProfileViewModel = new ChapterProfileViewModel(checkResult.ChapterProfileModel);
                ChapterUpdated?.Invoke(this, new ChapterUpdateEventArgs(this.currentChapterId, chapterProfileViewModel));

                this.saveguardSema?.Release();
            }
#endif
            // TODO: Crashing code below to test unexpected error handling:
//            await Task.Delay(10000);

//#pragma warning disable CS8602 // Dereference of a possibly null reference.
//            string? s = null; int i = s.Length;
//#pragma warning restore CS8602 // Dereference of a possibly null reference.

            lock (this.timerLock)
            {
                if (periodicTimerRawSource != null)
                {
                    periodicTimerRawSource.Change(RawSourcePollingInterval, Timeout.Infinite);
                }
            }
        }
    }
}
