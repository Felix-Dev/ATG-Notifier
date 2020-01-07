using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.ViewModels.Infrastructure;
using ATG_Notifier.ViewModels.Models;
using ATG_Notifier.ViewModels.Services;
using ATG_Notifier.ViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace ATG_Notifier.Desktop.ViewModels
{
    // TODO: Perhaps add an IsLoaded Property the View can bind to?
    internal class ChapterProfilesListViewModel : GenericListViewModel<ChapterProfileViewModel>
    {
        private readonly IChapterProfileService chapterProfileService;
        private readonly IUpdateService updateService;

        private readonly object itemsLock = new object();

        private int chapterProfilesUnreadCount;

        public ChapterProfilesListViewModel(IChapterProfileService chapterProfileService, IUpdateService updateService)
        {
            this.chapterProfileService = chapterProfileService;
            this.updateService = updateService;

            this.AppSettings = ServiceLocator.Current.GetService<SettingsViewModel>();

            this.Items = new ObservableCollection<ChapterProfileViewModel>();
            BindingOperations.EnableCollectionSynchronization(this.Items, this.itemsLock);

            this.SetListAsReadCommand = new RelayCommand(OnSetListAsRead);

            this.updateService.ChapterUpdated += OnChapterUpdateAsync;
        }

        public event EventHandler<ChapterProfilesUnreadCountChangedEventArgs>? ChapterProfilesUnreadCountChanged;

        public SettingsViewModel AppSettings { get; private set; }

        public ICommand SetListAsReadCommand { get; }

        public async Task LoadAsync()
        {
            IList<ChapterProfileModel> chapterProfileModels = await this.chapterProfileService.GetChapterProfilesAsync();

            foreach (var model in chapterProfileModels)
            {
                Add(new ChapterProfileViewModel(model));

                if (!model.IsRead)
                {
                    this.chapterProfilesUnreadCount++;
                }
            }

            this.ChapterProfilesUnreadCountChanged?.Invoke(this, new ChapterProfilesUnreadCountChangedEventArgs(this.chapterProfilesUnreadCount));
        }

        public async Task DeleteChapterProfileAsync(ChapterProfileViewModel chapterProfileViewModel)
        {
            Remove(chapterProfileViewModel);
        }

        protected override async void OnClear()
        {
            await this.chapterProfileService.DeleteChapterProfileRangeAsync(this.Items.Select(viewModel => viewModel.ChapterProfileModel).ToList());

            base.OnClear();

            this.chapterProfilesUnreadCount = 0;
            ChapterProfilesUnreadCountChanged?.Invoke(this, new ChapterProfilesUnreadCountChangedEventArgs(this.chapterProfilesUnreadCount));
        }

        private async void OnSetListAsRead()
        {
            foreach (var chapterProfile in this.Items)
            {
                chapterProfile.IsRead = true;
            }

            this.chapterProfilesUnreadCount = 0;
            ChapterProfilesUnreadCountChanged?.Invoke(this, new ChapterProfilesUnreadCountChangedEventArgs(this.chapterProfilesUnreadCount));

            await this.chapterProfileService.UpdateChapterProfilesAsync(this.Items.Select(vm => vm.ChapterProfileModel).ToList());
        }

        private async void OnChapterUpdateAsync(object? sender, ChapterUpdateEventArgs e)
        {
            //CommonHelpers.RunOnUIThread(() => Add(e.ChapterProfile));

            Add(e.ChapterProfile);

            this.chapterProfilesUnreadCount++;
            ChapterProfilesUnreadCountChanged?.Invoke(this, new ChapterProfilesUnreadCountChangedEventArgs(this.chapterProfilesUnreadCount));

            await this.chapterProfileService.UpdateChapterProfileAsync(e.ChapterProfile.ChapterProfileModel);
        }

        protected override async void OnRemove(ChapterProfileViewModel model)
        {
            base.OnRemove(model);

            await this.chapterProfileService.DeleteChapterProfileAsync(model.ChapterProfileModel);

            if (!model.IsRead)
            {
                this.chapterProfilesUnreadCount--;
                ChapterProfilesUnreadCountChanged?.Invoke(this, new ChapterProfilesUnreadCountChangedEventArgs(this.chapterProfilesUnreadCount));
            }
        }

        public async Task UpdateChapterProfileAsync(ChapterProfileViewModel chapterProfileViewModel)
        {
            ChapterProfileModel? chapterProfileModel = await this.chapterProfileService.GetChapterProfileAsync(chapterProfileViewModel.ChapterProfileId).ConfigureAwait(false);
            if (chapterProfileModel != null 
                && chapterProfileViewModel.IsRead && !chapterProfileModel.IsRead)
            {
                this.chapterProfilesUnreadCount--;
                ChapterProfilesUnreadCountChanged?.Invoke(this, new ChapterProfilesUnreadCountChangedEventArgs(this.chapterProfilesUnreadCount));
            }

            await this.chapterProfileService.UpdateChapterProfileAsync(chapterProfileViewModel.ChapterProfileModel).ConfigureAwait(false);
        }
    }
}
