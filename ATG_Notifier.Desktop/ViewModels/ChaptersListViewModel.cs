using ATG_Notifier.Desktop.Helpers;
using ATG_Notifier.Desktop.Utilities;
using ATG_Notifier.ViewModels.Infrastructure;
using ATG_Notifier.ViewModels.Models;
using ATG_Notifier.ViewModels.Services;
using ATG_Notifier.ViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ATG_Notifier.Desktop.ViewModels
{
    public class ChaptersListViewModel
    {
        private readonly IChapterProfileService chapterProfileService;
        private readonly IUpdateService updateService;

        private int chapterProfilesUnreadCount;

        public ChaptersListViewModel(IChapterProfileService chapterProfileService, IUpdateService updateService)
        {
            this.chapterProfileService = chapterProfileService ?? throw new ArgumentNullException(nameof(chapterProfileService));
            this.updateService = updateService ?? throw new ArgumentNullException(nameof(updateService));

            this.ClearListCommand = new RelayCommand(OnClearAsync);
            this.DeleteChapterProfileCommand = new RelayCommand<ChapterProfileViewModel>(OnChapterProfileRemoveAsync);
            this.OpenChapterCommand = new RelayCommand<ChapterProfileViewModel>(OnOpenChapter);
            this.ReadChapterCommand = new RelayCommand<ChapterProfileViewModel>(OnChapterProfileReadAsync);

            this.ChapterProfiles = new ObservableCollection<ChapterProfileViewModel>();

            this.updateService.ChapterUpdated += OnChapterUpdateAsync;
        }

        public event EventHandler<ChapterProfilesUnreadCountChangedEventArgs> ChapterProfilesUnreadCountChanged;

        public ICommand DeleteChapterProfileCommand { get; }

        public ICommand OpenChapterCommand { get; }

        public ICommand ReadChapterCommand { get; }

        public ICommand ClearListCommand { get; }

        public IList<ChapterProfileViewModel> ChapterProfiles { get; private set; }

        public async Task LoadAsync()
        {
            IList<ChapterProfileModel> chapterProfileModels = await this.chapterProfileService.GetChapterProfilesAsync();

            foreach (var model in chapterProfileModels)
            {
                this.ChapterProfiles.Add(new ChapterProfileViewModel(model));

                if (!model.IsRead)
                {
                    this.chapterProfilesUnreadCount++;
                }
            }

            this.ChapterProfilesUnreadCountChanged?.Invoke(this, new ChapterProfilesUnreadCountChangedEventArgs(this.chapterProfilesUnreadCount));
        }

        public async void OnClearAsync()
        {
            await this.chapterProfileService.DeleteChapterProfileRangeAsync(this.ChapterProfiles.Select(viewModel => viewModel.ChapterProfileModel).ToList());

            this.ChapterProfiles.Clear();
            this.chapterProfilesUnreadCount = 0;

            ChapterProfilesUnreadCountChanged?.Invoke(this, new ChapterProfilesUnreadCountChangedEventArgs(this.chapterProfilesUnreadCount));
        }

        private async void OnChapterUpdateAsync(object sender, ChapterUpdateEventArgs e)
        {
            CommonHelpers.RunOnUIThread(() => this.ChapterProfiles.Add(e.ChapterProfile));

            this.chapterProfilesUnreadCount++;
            ChapterProfilesUnreadCountChanged?.Invoke(this, new ChapterProfilesUnreadCountChangedEventArgs(this.chapterProfilesUnreadCount));

            await this.chapterProfileService.UpdateChapterProfileAsync(e.ChapterProfile.ChapterProfileModel);
        }

        private async void OnChapterProfileRemoveAsync(ChapterProfileViewModel viewModel)
        {
            this.ChapterProfiles.Remove(viewModel);

            await this.chapterProfileService.DeleteChapterProfileAsync(viewModel.ChapterProfileModel);

            if (!viewModel.IsRead)
            {
                this.chapterProfilesUnreadCount--;
                ChapterProfilesUnreadCountChanged?.Invoke(this, new ChapterProfilesUnreadCountChangedEventArgs(this.chapterProfilesUnreadCount));
            }
        }

        private async void OnChapterProfileReadAsync(ChapterProfileViewModel viewModel)
        {
            if (viewModel.IsRead)
            {
                return;
            }

            viewModel.IsRead = true;

            this.chapterProfilesUnreadCount--;
            ChapterProfilesUnreadCountChanged?.Invoke(this, new ChapterProfilesUnreadCountChangedEventArgs(this.chapterProfilesUnreadCount));

            await this.chapterProfileService.UpdateChapterProfileAsync(viewModel.ChapterProfileModel);
        }

        private void OnOpenChapter(ChapterProfileViewModel viewModel)
        {
            WebUtility.OpenWebsite(viewModel.Url);

            OnChapterProfileReadAsync(viewModel);
        }
    }
}
