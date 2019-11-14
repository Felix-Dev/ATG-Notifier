using ATG_Notifier.UWP.Helpers;
using ATG_Notifier.UWP.Services;
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

namespace ATG_Notifier.UWP.ViewModels
{
    public class ChapterListViewModel : GenericListViewModel<ChapterProfileViewModel>
    {
        //private readonly IDictionary<long, ChapterProfileViewModel> chapterDictionary = new Dictionary<long, ChapterProfileViewModel>();
        private readonly IChapterProfileService chapterProfileService;

        public ChapterListViewModel(IChapterProfileService chapterProfileService)
        {
            this.chapterProfileService = chapterProfileService ?? throw new ArgumentNullException(nameof(chapterProfileService));
        }

        public ICommand FilterCommand { get; }

        public ICommand SortCommand { get; }

        public async Task LoadAsync(ChapterListArguments args = null)
        {
            // TODO: Only make LoadAsync load chapters, move selecting an item to a different method 
            // (so we don't always reload when we don't have to)

            if (this.Items == null)
            {
                IList<ChapterProfileModel> chapterProfileModels = await this.chapterProfileService.GetChapterProfilesAsync();

                this.Items = new ObservableCollection<ChapterProfileViewModel>(chapterProfileModels.Select(profile => new ChapterProfileViewModel(profile)));
                //foreach (var viewModel in this.Items)
                //{
                //    this.chapterDictionary[viewModel.ChapterProfileId] = viewModel;
                //}
            }

            //if (args != null
            //    && this.chapterDictionary.TryGetValue(args.ChapterId, out ChapterProfileViewModel chapterProfile))
            //{
            //    this.SelectedItem = chapterProfile;
            //}
        }

        //protected override void OnAdd(ChapterProfileViewModel model)
        //{
        //    base.OnAdd(model);

        //    chapterDictionary[model.ChapterProfileId] = model;
        //}

        protected override void OnRemove(ChapterProfileViewModel model)
        {
            base.OnRemove(model);

            //this.chapterDictionary.Remove(model.ChapterProfileId);
            this.chapterProfileService.DeleteChapterProfileAsync(model.ChapterProfileModel);
        }

        protected override void OnClear()
        {
            this.chapterProfileService.DeleteChapterProfileRangeAsync(this.Items.Select(viewModel => viewModel.ChapterProfileModel).ToList());

            base.OnClear();
            //this.chapterDictionary.Clear();
        }

        private void LoadChaptersTest()
        {
            //var chapterModels = new List<ChapterProfileModel>()
            //{
            //    new ChapterProfileModel()
            //    {
            //        NumberAndTitleFallbackString = $"Chapter 1598 - {DateTime.Now.ToLongTimeString()}",
            //        Url = "http://book.zongheng.com/chapter/408586/58484757.html", /* settings.ChapterUrl */
            //        ReleaseTime = null, AppArrivalTime = DateTime.Now,
            //    },
            //    new ChapterProfileModel()
            //    {
            //        NumberAndTitleFallbackString = $"Chapter 1599 - {DateTime.Now.ToLongTimeString()}",
            //        Url = "http://book.zongheng.com/chapter/408586/58484757.html", /* settings.ChapterUrl */
            //        ReleaseTime = null, AppArrivalTime = DateTime.Now,
            //    },
            //    new ChapterProfileModel()
            //    {
            //        NumberAndTitleFallbackString = $"Chapter 1600 - {DateTime.Now.ToLongTimeString()}",
            //        Url = "http://book.zongheng.com/chapter/408586/58484757.html", /* settings.ChapterUrl */
            //        ReleaseTime = null, AppArrivalTime = DateTime.Now,
            //    },
            //    new ChapterProfileModel()
            //    {
            //        NumberAndTitleFallbackString = $"Chapter 1601 - {DateTime.Now.ToLongTimeString()}",
            //        Url = "http://book.zongheng.com/chapter/408586/58484757.html", /* settings.ChapterUrl */
            //        ReleaseTime = null, AppArrivalTime = DateTime.Now,
            //    },
            //};

            //foreach (var model in chapterModels)
            //{
            //    this.Items.Add(new ChapterProfileViewModel(model));
            //}

        }
    }
}
