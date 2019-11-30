using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.Models;
using ATG_Notifier.Desktop.Services.Taskbar;
using ATG_Notifier.ViewModels.Models;
using ATG_Notifier.ViewModels.Networking;
using ATG_Notifier.ViewModels.Services;
using ATG_Notifier.ViewModels.ViewModels;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ATG_Notifier.Desktop.Services
{
    internal class UpdateService : IUpdateService
    {
#if DEBUG
        private const int RawSourcePollingInterval = 1 * 10 * 1000;
#else
        private const int RawSourcePollingInterval = 1* 30 * 1000;
#endif

        private readonly ILogService logService;
        private readonly IWebService webService;

        private readonly RawSourceChecker rawSourceChecker; 

        private readonly ToastNotificationManager notifier = ToastNotificationManager.Instance;

        private readonly object timerLock = new object();
        private Timer periodicTimerRawSource = null;

        private readonly EventWaitHandle jobRoundFinished;
        private readonly Thread workThread;
        private bool finishJob;

        private Random rand;
        private int debugCount = 0;

        private Semaphore saveguardSema;

        private SettingsViewModel appSettings = ServiceLocator.Current.GetService<SettingsViewModel>();

        public UpdateService(IWebService webService, ILogService logService)
        {
            this.webService = webService ?? throw new ArgumentNullException(nameof(webService));
            this.logService = logService ?? throw new ArgumentNullException(nameof(logService));

            this.rawSourceChecker = new RawSourceChecker(this.webService, this.logService);

            /* Create the worker thread used to check for updates. */
            //workThread = new Thread(new ThreadStart(DoJob));
#if DEBUG
            //workThread.Name = "worker"; // Assign worker thread a name for easier debugging.
#endif
            /* Create the job-round Event Handler */
            this.jobRoundFinished = new EventWaitHandle(true, EventResetMode.ManualReset);

            this.rand = new Random();
        }

        public event EventHandler<ChapterUpdateEventArgs> ChapterUpdated;

        // Note: not thread-safe!
        public void Start()
        {
            //ThreadState state = workThread.ThreadState;

            //// Only have one worker at a time.
            //if (state == ThreadState.Unstarted || state == ThreadState.Stopped)
            //{
            //    workThread.Start();
            //}

            if (this.periodicTimerRawSource != null)
            {
                // The update service is already running.
                return;
            }

            this.saveguardSema = new Semaphore(1, 1);

            this.periodicTimerRawSource = new Timer(OnTimerEllapsed, null, 0, Timeout.Infinite);
        }

        // Note: not thread-safe!
        public void Stop()
        {
            if (this.periodicTimerRawSource == null)
            {
                // The update service is already stopped.
                return;
            }

            /* Wait until a running update process is finished. */

            logService.Log(LogType.Debug, "Attempting to terminate the update service...");

            this.saveguardSema.WaitOne();

            //jobRoundFinished.WaitOne();

            //finishJob = true;

            // Request (immediate) termination of worker thread.
            //workThread.Interrupt();
            //workThread.Abort();

            /* Wait for the worker thread to terminate. */
            //workThread.Join();

            StopTimer();

            logService.Log(LogType.Debug, "The update service was terminated!");

            this.saveguardSema.Release();

            // Clean-up resources used by the event handler.
            this.saveguardSema.Dispose();
            this.saveguardSema = null;

            //jobRoundFinished.Dispose();
        }

        private void StopTimer()
        {
            lock (this.timerLock)
            {
                if (this.periodicTimerRawSource != null)
                {
                    this.periodicTimerRawSource.Dispose();
                    this.periodicTimerRawSource = null;
                }
            }
        }

        private async void OnTimerEllapsed(object state)
        {
#if DEBUG
            ChapterProfileModel chapterProfileModel = new ChapterProfileModel()
            {
                Number = 1600 + debugCount++,
                Title = "Hello World1234567",
                NumberAndTitleFallbackString = "Fallback String",
                Url = "http://book.zongheng.com/chapter/408586/58484757.html",
                WordCount = this.rand.Next(2500, 4500),
                ReleaseTime = DateTime.Now,
                AppArrivalTime = DateTime.Now
            };

            var chapterProfileViewModel = new ChapterProfileViewModel(chapterProfileModel);
            ChapterUpdated?.Invoke(this, new ChapterUpdateEventArgs(chapterProfileViewModel));

            this.appSettings.MostRecentChapterInfo = new MostRecentChapterInfo()
            {
                NumberAndTitle = chapterProfileViewModel.NumberAndTitleDisplayString,
                ReleaseTime = chapterProfileViewModel.ReleaseTime,
                WordCount = chapterProfileViewModel.WordCount,
            };

            TaskbarManager.Current.FlashTaskbarButton();

            if (!appSettings.IsInFocusMode)
            {
                notifier.Show("ATG Chapter Update! (Debug)", chapterProfileViewModel);
            }
#else
            ChapterSourceCheckResult checkResult = null;
            try
            {
                checkResult = await rawSourceChecker.GetUpdateAsync(this.appSettings.CurrentChapterId);
            }
            catch (SourceCheckerOperationFailedException ex)
            {
                logService.Log(LogType.Error, $"Checking for a raw source chapter update failed!\nException: {ex.InnerException}\nTechnical details: {ex.InnerException.Message}");
            }

            if (checkResult != null)
            {
                this.saveguardSema.WaitOne();

                // Store the [number and title] of the new chapter.
                this.appSettings.CurrentChapterId = checkResult.ChapterId;

                ChapterProfileModel chapterProfileModel = checkResult.ChapterProfileModel;

                var chapterProfileViewModel = new ChapterProfileViewModel(chapterProfileModel);
                ChapterUpdated?.Invoke(this, new ChapterUpdateEventArgs(chapterProfileViewModel));

                this.appSettings.MostRecentChapterInfo = new MostRecentChapterInfo()
                {
                    NumberAndTitle = chapterProfileViewModel.NumberAndTitleDisplayString,
                    ReleaseTime = chapterProfileViewModel.ReleaseTime,
                    WordCount = chapterProfileViewModel.WordCount,
                };

                TaskbarManager.Current.FlashTaskbarButton();

                //jobRoundFinished.Set();

                if (!appSettings.IsInFocusMode)
                {
                    notifier.Show("ATG Chapter Update!", chapterProfileViewModel);
                }

                this.saveguardSema.Release();
            }
#endif
            // TODO: Crashing code below to text unexpected error handling:
            //await Task.Delay(7000);

            //string s = null; int i = s.Length;

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
