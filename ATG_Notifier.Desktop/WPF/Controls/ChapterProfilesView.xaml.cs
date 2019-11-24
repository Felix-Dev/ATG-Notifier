using ATG_Notifier.Desktop.ViewModels;
using ATG_Notifier.ViewModels.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ATG_Notifier.Desktop.WPF.Controls
{
    internal partial class ChapterProfilesView : UserControl
    {
        public ChapterProfilesView(ChapterProfilesViewModel viewModel)
        {
            this.DataContext = viewModel;

            InitializeComponent();
        }

        public ChapterProfilesViewModel ViewModel => this.DataContext as ChapterProfilesViewModel;

        private async void OnChapterProfilesViewKeyDown(object sender, KeyEventArgs e)
        {
            // TODO: Remove, this is Focus testing code
            //if (e.Key == Key.Tab)
            //{
            //    var item = Keyboard.FocusedElement;
            //    Console.WriteLine(item?.ToString() ?? "");
            //}

            var uiElement = Keyboard.FocusedElement as UIElement;
            if (e.Key == Key.Delete
                && this.ChapterProfileList.GetDataItem(uiElement) is ChapterProfileViewModel chapterProfileViewModel)
            {
                // Programmatically set the focus to the chapter profile when we delete the currently focused one.
                // We set the focus to the next chapter profile below it, only when it is the last chapter profile,
                // we set the focus to the profile above it.
                // If there is no other profiel available, no UI element will be focused.
                var focusDirection = this.ChapterProfileList.IsLastItem(chapterProfileViewModel)
                    ? FocusNavigationDirection.Up
                    : FocusNavigationDirection.Down;

                TraversalRequest request = new TraversalRequest(focusDirection);

                // Change keyboard focus. 
                uiElement.MoveFocus(request);

                // Delete the specified chapter profile from the database.
                await this.ViewModel.ListViewModel.DeleteChapterProfileAsync(chapterProfileViewModel);
            }
        }

        private void OnLatestChapterButtonClick(object sender, RoutedEventArgs e)
        {
            if (!this.LatestChapterPopup.IsOpen)
            {
                this.LatestChapterPopup.IsOpen = true;
            }
        }
    }
}
