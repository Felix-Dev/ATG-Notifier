using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.Models;
using ATG_Notifier.Desktop.ViewModels;
using ATG_Notifier.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ATG_Notifier.Desktop.Services
{
    internal class ChapterUpdateListeningService
    {
        private readonly IUpdateService updateService;
        private readonly ChapterProfileServicePoint chapterProfileServicePoint;
        private readonly ILogService logService;

        private readonly AppState appState;
        private readonly SettingsViewModel settingsViewModel;

        private readonly SemaphoreSlim updateWaitSema;

        public ChapterUpdateListeningService(ILogService logService, IUpdateService updateService, ChapterProfileServicePoint chapterProfileServicePoint)
        {
            this.logService = logService;
            this.updateService = updateService;
            this.chapterProfileServicePoint = chapterProfileServicePoint;

            this.settingsViewModel = ServiceLocator.Current.GetService<SettingsViewModel>();
            this.appState = ServiceLocator.Current.GetService<AppState>();

            this.updateWaitSema = new SemaphoreSlim(1, 1);
        }

        public void Initialize()
        {
            this.updateService.ChapterUpdated += OnUpdateServiceChapterUpdated;
        }

        public void WaitAndStop()
        {
            this.updateWaitSema.Wait();

            this.updateService.ChapterUpdated -= OnUpdateServiceChapterUpdated;
        }

        public async Task WaitAndStopAsync()
        {
            await this.updateWaitSema.WaitAsync();

            this.updateService.ChapterUpdated -= OnUpdateServiceChapterUpdated;
        }

        private async void OnUpdateServiceChapterUpdated(object? sender, ChapterUpdateEventArgs e)
        {
            await this.updateWaitSema.WaitAsync();

            this.appState.CurrentChapterId = e.SourceChapterId;
            this.settingsViewModel.LatestUpdateProfile = new LatestUpdateProfile(e.ChapterProfileViewModel.NumberAndTitleDisplayString, e.ChapterProfileViewModel.WordCount)
            {
                ReleaseTime = e.ChapterProfileViewModel.ReleaseTime,
            };

            // TODO: Use TaskExtension to capture potential exceptions (async void here)
            await this.chapterProfileServicePoint.AddChapterProfileAsync(e.ChapterProfileViewModel).ConfigureAwait(false);

            this.updateWaitSema.Release();
        }
    }
}
