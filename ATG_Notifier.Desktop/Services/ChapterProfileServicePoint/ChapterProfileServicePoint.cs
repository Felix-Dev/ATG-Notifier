using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.Models;
using ATG_Notifier.ViewModels.Models;
using ATG_Notifier.ViewModels.Services;
using ATG_Notifier.ViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ATG_Notifier.Desktop.Services
{
    internal class ChapterProfileServicePoint
    {
        private readonly ILogService logService;
        private readonly IChapterProfileService chapterProfileService;

        private readonly AppState appState;

        private readonly SemaphoreSlim chapterLock = new SemaphoreSlim(1, 1);

        public ChapterProfileServicePoint(ILogService logService, IChapterProfileService chapterProfileService)
        {
            this.logService = logService;
            this.chapterProfileService = chapterProfileService;

            this.appState = ServiceLocator.Current.GetService<AppState>();
        }

        public event EventHandler<ChapterProfilesUnreadCountChangedEventArgs>? ChapterProfilesUnreadCountChanged;

        public void WaitAndStop()
        {
            this.chapterLock.Wait();
        }

        public async Task WaitAndStopAsync()
        {
            await this.chapterLock.WaitAsync();
        }

        public async Task AddChapterProfileAsync(ChapterProfileViewModel viewModel)
        {
            await chapterLock.WaitAsync().ConfigureAwait(false);

            await this.chapterProfileService.UpdateChapterProfileAsync(viewModel.ChapterProfileModel).ConfigureAwait(false);

            this.appState.UnreadChapters++;
            this.ChapterProfilesUnreadCountChanged?.Invoke(this, new ChapterProfilesUnreadCountChangedEventArgs(this.appState.UnreadChapters));

            chapterLock.Release();
        }

        public async Task<IList<ChapterProfileModel>> GetChapterProfilesAsync()
        {
            await chapterLock.WaitAsync().ConfigureAwait(false);

            IList<ChapterProfileModel> profileModels = await this.chapterProfileService.GetChapterProfilesAsync();

            chapterLock.Release();

            return profileModels;
        }

        public async Task UpdateChapterProfileAsync(ChapterProfileViewModel viewModel)
        {
            await chapterLock.WaitAsync().ConfigureAwait(false);

            ChapterProfileModel? chapterProfileModel = await this.chapterProfileService.GetChapterProfileAsync(viewModel.ChapterProfileId).ConfigureAwait(false);

            await this.chapterProfileService.UpdateChapterProfileAsync(viewModel.ChapterProfileModel).ConfigureAwait(false);

            if (chapterProfileModel != null
                && viewModel.IsRead && !chapterProfileModel.IsRead)
            {
                this.appState.UnreadChapters--;
                this.ChapterProfilesUnreadCountChanged?.Invoke(this, new ChapterProfilesUnreadCountChangedEventArgs(this.appState.UnreadChapters));
            }

            chapterLock.Release();
        }

        public async Task UpdateChapterProfilesAsync(IList<ChapterProfileViewModel> viewModels)
        {
            await chapterLock.WaitAsync().ConfigureAwait(false);

            int count = 0;
            foreach (var viewModel in viewModels)
            {
                ChapterProfileModel? chapterProfileModel = await this.chapterProfileService.GetChapterProfileAsync(viewModel.ChapterProfileId).ConfigureAwait(false);

                await this.chapterProfileService.UpdateChapterProfileAsync(viewModel.ChapterProfileModel).ConfigureAwait(false);

                if (chapterProfileModel != null
                    && viewModel.IsRead && !chapterProfileModel.IsRead)
                {
                    count++;
                }
            }

            if (count > 0)
            {
                this.appState.UnreadChapters -= count;
                this.ChapterProfilesUnreadCountChanged?.Invoke(this, new ChapterProfilesUnreadCountChangedEventArgs(this.appState.UnreadChapters));
            }

            chapterLock.Release();
        }

        //public async Task UpdateAllChapterProfilesToReadStatus()
        //{
        //    await chapterLock.WaitAsync();

        //    IList<ChapterProfileModel> profileModels = await this.chapterProfileService.GetChapterProfilesAsync().ConfigureAwait(false);

        //    if (this.appState.UnreadChapters > 0)
        //    {
        //        this.appState.UnreadChapters = 0;
        //        ChapterProfilesUnreadCountChanged?.Invoke(this, new ChapterProfilesUnreadCountChangedEventArgs(this.appState.UnreadChapters));
        //    }

        //    chapterLock.Release();
        //}

        public async Task DeleteChapterProfileAsync(ChapterProfileViewModel viewModel)
        {
            await chapterLock.WaitAsync().ConfigureAwait(false);

            await this.chapterProfileService.DeleteChapterProfileAsync(viewModel.ChapterProfileModel).ConfigureAwait(false);

            if (!viewModel.IsRead)
            {
                this.appState.UnreadChapters--;
                ChapterProfilesUnreadCountChanged?.Invoke(this, new ChapterProfilesUnreadCountChangedEventArgs(this.appState.UnreadChapters));
            }

            chapterLock.Release();
        }

        public async Task DeleteChapterProfilesAsync(IList<ChapterProfileViewModel> viewModels)
        {
            await chapterLock.WaitAsync().ConfigureAwait(false);

            int unreadChapters = 0;
            await this.chapterProfileService.DeleteChapterProfileRangeAsync(viewModels.Select(vm => 
                { 
                    if (!vm.IsRead) 
                    { 
                        unreadChapters++; 
                    } 
                    
                    return vm.ChapterProfileModel; 
                }).ToList()).ConfigureAwait(false);

            if (unreadChapters > 0)
            {
                this.appState.UnreadChapters -= unreadChapters;
                ChapterProfilesUnreadCountChanged?.Invoke(this, new ChapterProfilesUnreadCountChangedEventArgs(this.appState.UnreadChapters));
            }

            chapterLock.Release();
        }

        public async Task DeleteAllChapterProfilesAsync()
        {
            await chapterLock.WaitAsync().ConfigureAwait(false);

            await this.chapterProfileService.DeleteAllChapterProfilesAsync().ConfigureAwait(false);

            if (this.appState.UnreadChapters > 0)
            {
                this.appState.UnreadChapters = 0;
                ChapterProfilesUnreadCountChanged?.Invoke(this, new ChapterProfilesUnreadCountChangedEventArgs(this.appState.UnreadChapters));
            }

            chapterLock.Release();
        }
    }
}
