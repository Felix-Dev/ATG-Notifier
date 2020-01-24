using ATG_Notifier.Desktop.Utilities;
using ATG_Notifier.ViewModels.Helpers.Extensions;
using ATG_Notifier.ViewModels.Infrastructure;
using ATG_Notifier.ViewModels.Services;
using ATG_Notifier.ViewModels.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ATG_Notifier.Desktop.ViewModels
{
    internal class ChapterProfilesViewModel : ObservableObject
    {
        private readonly IUpdateService updateService;

        private bool canChangeUpdateServiceStatus;
        private bool isUpdateServiceRunning;
        private bool isLoaded;

        public ChapterProfilesViewModel(ChapterProfilesListViewModel listViewModel, IUpdateService updateService)
        {
            this.ListViewModel = listViewModel;
            this.updateService = updateService;

            this.updateService.Started += (s, e) => this.IsUpdateServiceRunning = true;
            this.updateService.Stopped += (s, e) => this.IsUpdateServiceRunning = false;

            this.ChangeUpdateServiceStatusCommand = new RelayCommand(OnChangeChapterUpdateMode);
            this.OpenChapterProfileCommand = new RelayCommand<ChapterProfileViewModel>(OnOpenChapterProfile);
            this.ChapterProfileLostFocusCommand = new RelayCommand<ChapterProfileViewModel>(OnChapterProfileLostFocus);

            this.canChangeUpdateServiceStatus = true;
            this.IsUpdateServiceRunning = this.updateService.IsRunning;
            this.IsLoaded = false;
        }

        public ChapterProfilesListViewModel ListViewModel { get; private set; }

        public bool IsLoaded
        {
            get => this.isLoaded;
            set => Set(ref this.isLoaded, value);
        }

        public bool IsUpdateServiceRunning
        {
            get => this.isUpdateServiceRunning;
            set => Set(ref this.isUpdateServiceRunning, value);
        }

        public bool CanChangeUpdateServiceStatus
        {
            get => this.canChangeUpdateServiceStatus;
            set => Set(ref this.canChangeUpdateServiceStatus, value);
        }

        public ICommand ChangeUpdateServiceStatusCommand { get; }

        public ICommand OpenChapterProfileCommand { get; }

        public ICommand ChapterProfileLostFocusCommand { get; }

        public async Task LoadAsync()
        {
            await this.ListViewModel.LoadAsync();

            this.IsLoaded = true;
        }

        public async Task LoadAsync(ChapterProfileListArgs args)
        {
            await this.ListViewModel.LoadAsync(args);

            this.IsLoaded = true;
        }

        private void OnChangeChapterUpdateMode()
        {
            if (this.updateService.IsRunning)
            {
                updateService.Stop();
            }
            else
            {
                updateService.Start();
            }

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
