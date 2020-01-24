using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.Helpers;
using ATG_Notifier.Desktop.Models;
using ATG_Notifier.Desktop.Services;
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

    internal class ChapterProfileListArgs
    {
        public ChapterProfileViewModel? ChapterProfileViewModel { get; set; }

        public static ChapterProfileListArgs CreateDefault() => new ChapterProfileListArgs() { ChapterProfileViewModel = null };
    }

    internal class ChapterProfilesListViewModel : GenericListViewModel<ChapterProfileViewModel>
    {
        private readonly IUpdateService updateService;

        private readonly ChapterProfileServicePoint chapterProfileServicePoint;

        //private readonly object itemsLock = new object();

        private bool hasUnreadChapters;

        public ChapterProfilesListViewModel(IChapterProfileService chapterProfileService, IUpdateService updateService)
        {
            this.updateService = updateService;

            this.chapterProfileServicePoint = ServiceLocator.Current.GetService<ChapterProfileServicePoint>();

            var appState = ServiceLocator.Current.GetService<AppState>();

            this.Items = new ObservableCollection<ChapterProfileViewModel>();
            //BindingOperations.EnableCollectionSynchronization(this.Items, this.itemsLock);

            this.SetListAsReadCommand = new RelayCommand(OnSetListAsRead);

            this.updateService.ChapterUpdated += OnChapterUpdate;
            this.chapterProfileServicePoint.ChapterProfilesUnreadCountChanged += OnChapterProfilesUnreadCountChanged;

            this.HasUnreadChapters = appState.UnreadChapters > 0;
        }

        public bool HasUnreadChapters
        {
            get => this.hasUnreadChapters;
            set => Set(ref this.hasUnreadChapters, value);
        }

        public ICommand SetListAsReadCommand { get; }

        public ChapterProfileListArgs CreateArgs()
        {
            return new ChapterProfileListArgs
            {
                ChapterProfileViewModel = null,
            };
        }

        public async Task LoadAsync()
        {
            IList<ChapterProfileModel> chapterProfileModels = await this.chapterProfileServicePoint.GetChapterProfilesAsync();

            foreach (var model in chapterProfileModels)
            {
                Add(new ChapterProfileViewModel(model));
            }
        }

        public async Task LoadAsync(ChapterProfileListArgs args)
        {
            IList<ChapterProfileModel> chapterProfileModels = await this.chapterProfileServicePoint.GetChapterProfilesAsync();

            foreach (var model in chapterProfileModels)
            {
                Add(new ChapterProfileViewModel(model));
            }

            //if (args.ChapterProfileViewModel != null)
            //{
            //    this.SelectedItem =
            //        this.Items.
            //        Where(item => item.ChapterProfileId == args.ChapterProfileViewModel!.ChapterProfileId)
            //        .FirstOrDefault();
            //}
        }

        public async Task DeleteChapterProfileAsync(ChapterProfileViewModel chapterProfileViewModel)
        {
            Remove(chapterProfileViewModel);
        }

        protected override async void OnClear()
        {
            await this.chapterProfileServicePoint.DeleteAllChapterProfilesAsync();

            base.OnClear();
        }

        private async void OnSetListAsRead()
        {
            foreach (var chapterProfileViewModel in this.Items)
            {
                chapterProfileViewModel.IsRead = true;
            }

            await this.chapterProfileServicePoint.UpdateChapterProfilesAsync(this.Items);

            //this.chapterProfilesUnreadCount = 0;
            //ChapterProfilesUnreadCountChanged?.Invoke(this, new ChapterProfilesUnreadCountChangedEventArgs(this.chapterProfilesUnreadCount));

            //await this.chapterProfileService.UpdateChapterProfilesAsync(this.Items.Select(vm => vm.ChapterProfileModel).ToList());
        }

        private void OnChapterUpdate(object? sender, ChapterUpdateEventArgs e)
        {
            CommonHelpers.RunOnUIThread(() => Add(e.ChapterProfileViewModel));
        }

        protected override async void OnRemove(ChapterProfileViewModel viewModel)
        {
            base.OnRemove(viewModel);

            await this.chapterProfileServicePoint.DeleteChapterProfileAsync(viewModel);
        }

        public async Task UpdateChapterProfileAsync(ChapterProfileViewModel chapterProfileViewModel)
        {
            await this.chapterProfileServicePoint.UpdateChapterProfileAsync(chapterProfileViewModel);
        }

        private void OnChapterProfilesUnreadCountChanged(object? sender, Services.ChapterProfilesUnreadCountChangedEventArgs e)
        {
            this.HasUnreadChapters = e.Count > 0;
        }
    }
}
