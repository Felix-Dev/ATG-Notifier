using ATG_Notifier.UWP.Configuration;
using ATG_Notifier.UWP.Helpers;
using ATG_Notifier.UWP.ViewModels;
using ATG_Notifier.ViewModels.Helpers.Extensions;
using ATG_Notifier.ViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace ATG_Notifier.UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChaptersView : Page
    {
        private bool setFocusOnSelectedChapter;
        private long chapterId;

        public ChaptersView()
        {
            this.DataContext = ServiceLocator.Current.GetService<ChaptersViewModel>();
            this.InitializeComponent();

            this.ChapterList.Loaded += OnChapterListLoaded;
        }

        public ChaptersViewModel ViewModel => this.DataContext as ChaptersViewModel;

        public Visibility ShowChapterList(bool noChapters)
        {
            return noChapters
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        public void SetFocusOnChapterProfile(long id)
        {
            this.ChapterList.SetFocusOnChapter(id);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                return;
            }

            if (e.Parameter is ChaptersViewArguments args)
            {
                this.setFocusOnSelectedChapter = args.SetFocusOnSelectedChapter;
                this.chapterId = args.SelectedChapterId;

                await this.ViewModel.LoadAsync(new ChapterListArguments { ChapterId = args.SelectedChapterId });
            }
            else
            {
                await this.ViewModel.LoadAsync();
            }
        }

        private void OnChapterListLoaded(object sender, RoutedEventArgs e)
        {
            if (this.setFocusOnSelectedChapter)
            {
                SetFocusOnChapterProfile(this.chapterId);
                this.setFocusOnSelectedChapter = false;
            }
        }

        private void Page_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Delete)
            {
                var focusedChapter = this.ChapterList.GetFocusedElement();
                if (focusedChapter is ChapterProfileViewModel chapterProfile)
                {
                    ViewModel.ChapterListViewModel.Remove(chapterProfile);
                }
            }
        }
    }
}
