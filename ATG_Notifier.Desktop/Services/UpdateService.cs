﻿using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.Models;
using ATG_Notifier.Desktop.Services.Taskbar;
using ATG_Notifier.ViewModels.Models;
using ATG_Notifier.ViewModels.Networking;
using ATG_Notifier.ViewModels.Services;
using ATG_Notifier.ViewModels.ViewModels;
using System;
using System.Threading;

namespace ATG_Notifier.Desktop.Services
{
    internal class UpdateService : IUpdateService
    {
#if DEBUG
        private const int RawSourcePollingInterval = 1 * 10 * 1000;
#else
        private const int RawSourcePollingInterval = 1* 30 * 1000;
#endif

        private const string NotificationTitle = "ATG Chapter Update!";

        private readonly ILogService logService;
        private readonly IWebService webService;

        private readonly RawSourceChecker rawSourceChecker; 

        private readonly ToastNotificationManager notifier = ToastNotificationManager.Instance;

        private readonly object timerLock = new object();
        private Timer? periodicTimerRawSource = null;

        private Semaphore? saveguardSema;

        private SettingsViewModel settingsViewModel = ServiceLocator.Current.GetService<SettingsViewModel>();

        public UpdateService(IWebService webService, ILogService logService)
        {
            this.webService = webService;
            this.logService = logService;

            this.rawSourceChecker = new RawSourceChecker(this.webService, this.logService);
        }

        public event EventHandler<ChapterUpdateEventArgs>? ChapterUpdated;

        // Note: not thread-safe!
        public void Start()
        {
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

            // Wait until a running update process is finished.

            logService.Log(LogType.Debug, "Attempting to terminate the update service...");

            this.saveguardSema?.WaitOne();

            StopTimer();

            logService.Log(LogType.Debug, "The update service was terminated!");

            this.saveguardSema?.Release();

            // Clean-up resources used by the event handler.
            this.saveguardSema?.Dispose();
            this.saveguardSema = null;
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

        private async void OnTimerEllapsed(object? state)
        {
#if DEBUG
            ChapterProfileModel chapterProfileModel = new ChapterProfileModel()
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
            ChapterUpdated?.Invoke(this, new ChapterUpdateEventArgs(chapterProfileViewModel));

            this.settingsViewModel.MostRecentChapterInfo = new MostRecentChapterInfo(chapterProfileViewModel.NumberAndTitleDisplayString, chapterProfileViewModel.WordCount)
            {
                ReleaseTime = chapterProfileViewModel.ReleaseTime,
            };

            TaskbarManager.Current.FlashTaskbarButton();

            if (!settingsViewModel.IsInFocusMode)
            {
                notifier.Show("ATG Chapter Update! (Debug)", chapterProfileViewModel);
            }
#else
            ChapterSourceCheckResult? checkResult = null;
            try
            {
                checkResult = await rawSourceChecker.GetUpdateAsync(this.settingsViewModel.CurrentChapterId);
            }
            catch (SourceCheckerOperationFailedException ex)
            {
                logService.Log(LogType.Error, $"Checking for a raw source chapter update failed!\n Message: {ex.Message}\n" +
                    $"Exception: {ex.InnerException?.ToString() ?? ""}\nTechnical details: {ex.InnerException?.Message ?? ""}");
            }

            if (checkResult != null)
            {
                this.saveguardSema?.WaitOne();

                // Store the [number and title] of the new chapter.
                this.settingsViewModel.CurrentChapterId = checkResult.ChapterId;

                ChapterProfileModel chapterProfileModel = checkResult.ChapterProfileModel;

                var chapterProfileViewModel = new ChapterProfileViewModel(chapterProfileModel);
                ChapterUpdated?.Invoke(this, new ChapterUpdateEventArgs(chapterProfileViewModel));

                this.settingsViewModel.MostRecentChapterInfo = new MostRecentChapterInfo(chapterProfileViewModel.NumberAndTitleDisplayString, chapterProfileViewModel.WordCount)
                {
                    ReleaseTime = chapterProfileViewModel.ReleaseTime,
                };

                TaskbarManager.Current.FlashTaskbarButton();

                if (!settingsViewModel.IsInFocusMode)
                {
                    notifier.Show(NotificationTitle, chapterProfileViewModel);
                }

                this.saveguardSema?.Release();
            }
#endif
//            // TODO: Crashing code below to test unexpected error handling:
//            await Task.Delay(7000);

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
