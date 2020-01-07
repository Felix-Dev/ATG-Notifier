using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.ViewModels;
using ATG_Notifier.ViewModels.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ATG_Notifier.Desktop.WPF.Controls
{
    internal partial class ChapterProfilesView : UserControl
    {
        public ChapterProfilesView()
        {
            this.DataContext = this;

            this.ViewModel = ServiceLocator.Current.GetService<ChapterProfilesViewModel>();
            this.SettingsViewModel = ServiceLocator.Current.GetService<SettingsViewModel>();

            InitializeComponent();
        }

        public ChapterProfilesViewModel ViewModel { get; }

        public SettingsViewModel SettingsViewModel { get; }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Delete
                && Keyboard.FocusedElement is UIElement uiElement
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
                this.ViewModel.ListViewModel.DeleteChapterProfileAsync(chapterProfileViewModel);
            }

            base.OnKeyDown(e);
        }

        private void OnLatestChapterButtonClick(object sender, RoutedEventArgs e)
        {
            if (!this.LatestChapterPopup.IsOpen)
            {
                this.LatestChapterPopup.IsOpen = true;
            }
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            //var vm = this.ViewModel.ListViewModel;

            // load chapter profiles from database 
            //await Task.Run(() => vm.LoadAsync());

            //if (!this.ViewModel.ListViewModel.IsEmpty)
            //{
            //    //this.NoChapterTextBlock.Visibility = Visibility.Collapsed;
            //    //this.ChapterProfileList.Visibility = Visibility.Visible;
            //}

            //if (this.appSettings.IsUpdateServiceRunning)
            //{
            //    this.updateService.Start();
            //}
        }
    }
}
