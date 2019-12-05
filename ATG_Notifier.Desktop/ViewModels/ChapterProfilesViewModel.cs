using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.Models;
using ATG_Notifier.Desktop.Utilities;
using ATG_Notifier.ViewModels.Infrastructure;
using ATG_Notifier.ViewModels.Services;
using ATG_Notifier.ViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using ATG_Notifier.ViewModels.Helpers.Extensions;

namespace ATG_Notifier.Desktop.ViewModels
{
    internal class ChapterProfilesViewModel : ObservableObject
    {
        private readonly IUpdateService updateService;
        private bool canChangeUpdateServiceStatus;

        public ChapterProfilesViewModel(ChapterProfilesListViewModel listViewModel, IUpdateService updateService)
        {
            this.ListViewModel = listViewModel ?? throw new ArgumentNullException(nameof(listViewModel));
            this.updateService = updateService ?? throw new ArgumentNullException(nameof(updateService));

            this.ChangeUpdateServiceStatusCommand = new RelayCommand(OnChangeChapterUpdateModel);
            this.OpenChapterProfileCommand = new RelayCommand<ChapterProfileViewModel>(OnOpenChapterProfile);
            this.ChapterProfileLostFocusCommand = new RelayCommand<ChapterProfileViewModel>(OnChapterProfileLostFocus);

            this.canChangeUpdateServiceStatus = true;
        }

        public ChapterProfilesListViewModel ListViewModel { get; private set; }

        public SettingsViewModel AppSettings { get; } = ServiceLocator.Current.GetService<SettingsViewModel>();

        public bool CanChangeUpdateServiceStatus
        {
            get => this.canChangeUpdateServiceStatus;
            set => Set(ref this.canChangeUpdateServiceStatus, value);
        }

        public ICommand ChangeUpdateServiceStatusCommand { get; }

        public ICommand OpenChapterProfileCommand { get; }

        public ICommand ChapterProfileLostFocusCommand { get; }

        private void OnChangeChapterUpdateModel()
        {
            bool isUpdateServiceRunning = AppSettings.IsUpdateServiceRunning;

            if (isUpdateServiceRunning)
            {
                updateService.Stop();
            }
            else
            {
                updateService.Start();
            }

            AppSettings.IsUpdateServiceRunning = !isUpdateServiceRunning;

            ApplyCooldownPhaseForCanChangeUpdateStatusService().FireAndForget();
        }

        private async Task ApplyCooldownPhaseForCanChangeUpdateStatusService()
        {
            CanChangeUpdateServiceStatus = false;

            await Task.Delay(2000);

            CanChangeUpdateServiceStatus = true;
        }

        private void OnOpenChapterProfile(ChapterProfileViewModel chapterProfileViewModel)
        {
            WebUtility.OpenWebsite(chapterProfileViewModel.Url);

            if (chapterProfileViewModel.IsRead)
            {
                return;
            }

            chapterProfileViewModel.IsRead = true;
            this.ListViewModel.UpdateChapterProfileAsync(chapterProfileViewModel).FireAndForgetSafeAsync();
        }

        private void OnChapterProfileLostFocus(ChapterProfileViewModel chapterProfileViewModel)
        {
            if (chapterProfileViewModel == null || chapterProfileViewModel.IsRead)
            {
                return;
            }

            chapterProfileViewModel.IsRead = true;
            this.ListViewModel.UpdateChapterProfileAsync(chapterProfileViewModel).FireAndForgetSafeAsync();
        }
    }
}
