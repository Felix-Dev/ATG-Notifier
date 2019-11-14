using ATG_Notifier.UWP.Helpers;
using ATG_Notifier.UWP.Services;
using ATG_Notifier.UWP.Utilities;
using ATG_Notifier.ViewModels.Infrastructure;
using ATG_Notifier.ViewModels.Services;
using ATG_Notifier.ViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ATG_Notifier.UWP.ViewModels
{
    public class ChaptersViewModel : ObservableObject
    {
        private static readonly ToastNotificationService toastNotificationService = Singleton<ToastNotificationService>.Instance;

        private readonly IChapterProfileService chapterProfileService;
        private readonly IUpdateService updateService;

        public ChaptersViewModel(IChapterProfileService chapterProfileService, IUpdateService updateService)
        {
            this.chapterProfileService = chapterProfileService ?? throw new ArgumentNullException(nameof(chapterProfileService));
            this.updateService = updateService ?? throw new ArgumentNullException(nameof(updateService));

            this.ChapterListViewModel = new ChapterListViewModel(this.chapterProfileService);

            this.OpenChapterCommand = new RelayCommand<ChapterProfileViewModel>(OpenChapter);
            this.LostFocusChapterCommand = new RelayCommand<ChapterProfileViewModel>(LostFocusChapter);

            this.UpdateCommand = new RelayCommand(CheckForUpdates);

            this.updateService.ChapterUpdated += OnChapterUpdate;
        }

        public ChapterListViewModel ChapterListViewModel { get; private set; }

        public async Task LoadAsync(ChapterListArguments args = null)
        {
            await this.ChapterListViewModel.LoadAsync(args);
        }

        private void OnChapterUpdate(object sender, ChapterUpdateEventArgs e)
        {
            CommonHelpers.RunOnUIThread(() => this.ChapterListViewModel.Add(e.ChapterProfile));
        }

        private async void OpenChapter(ChapterProfileViewModel chapterProfileVM)
        {
            chapterProfileVM.IsRead = true;
            await WebUtility.OpenWebsiteAsync(chapterProfileVM.Url);
        }

        private void LostFocusChapter(ChapterProfileViewModel chapterProfileVM)
        {
            chapterProfileVM.IsRead = true;
            toastNotificationService.RemoveNotification(chapterProfileVM.ChapterProfileId);
        }

        private void CheckForUpdates()
        {
        }

        #region Commands

        public ICommand OpenChapterCommand { get; }

        public ICommand LostFocusChapterCommand { get; }

        public ICommand UpdateCommand { get; }

        #endregion
    }
}
