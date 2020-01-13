using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.Models;
using ATG_Notifier.Desktop.ViewModels;
using ATG_Notifier.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ATG_Notifier.Desktop.Services
{
    internal class ChapterUpdateListeningService
    {
        private readonly IUpdateService updateService;
        private readonly ILogService logService;

        private readonly IChapterProfileService chapterProfileService;

        private readonly AppState appState;
        private readonly SettingsViewModel settingsViewModel;

        public ChapterUpdateListeningService(ILogService logService, IUpdateService updateService)
        {
            this.logService = logService;
            this.updateService = updateService;

            this.chapterProfileService = ServiceLocator.Current.GetService<IChapterProfileService>();

            this.settingsViewModel = ServiceLocator.Current.GetService<SettingsViewModel>();
            this.appState = ServiceLocator.Current.GetService<AppState>();
        }

        public void Initialize()
        {
            this.updateService.ChapterUpdated += OnUpdateServiceChapterUpdated;
        }

        private async void OnUpdateServiceChapterUpdated(object? sender, ChapterUpdateEventArgs e)
        {
            this.appState.CurrentChapterId = e.SourceChapterId;
            this.settingsViewModel.LatestUpdateProfile = new LatestUpdateProfile(e.ChapterProfileViewModel.NumberAndTitleDisplayString, e.ChapterProfileViewModel.WordCount)
            {
                ReleaseTime = e.ChapterProfileViewModel.ReleaseTime,
            };

            // TODO: Use TaskExtension to capture potential exceptions (async void here)
            await this.chapterProfileService.UpdateChapterProfileAsync(e.ChapterProfileViewModel.ChapterProfileModel).ConfigureAwait(false);
        }
    }
}
