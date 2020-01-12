using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.Services;
using ATG_Notifier.Desktop.Utilities;
using ATG_Notifier.Desktop.ViewModels;
using ATG_Notifier.ViewModels.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ATG_Notifier.Desktop.Views
{
    internal partial class MainPage : Page
    {
        private readonly DialogService dialogService;
        //private readonly NetworkService networkService;

        private readonly MainPageViewModel viewModel;
        private readonly SettingsViewModel settingsViewModel;

        public MainPage()
        {
            this.dialogService = ServiceLocator.Current.GetService<DialogService>();
            //this.networkService = ServiceLocator.Current.GetService<NetworkService>();

            this.viewModel = ServiceLocator.Current.GetService<MainPageViewModel>();
            this.settingsViewModel = ServiceLocator.Current.GetService<SettingsViewModel>();

            this.DataContext = this;

            InitializeComponent();
            InitializeNotificationDisplayPositionMenu();
        }

        public MainPageViewModel ViewModel => this.viewModel;

        public SettingsViewModel SettingsViewModel => this.settingsViewModel;

        private void InitializeNotificationDisplayPositionMenu()
        {
            switch (this.settingsViewModel.NotificationDisplayPosition)
            {
                case ToastNotification.DisplayPosition.TopLeft:
                    this.MenuItemTopLeft.IsChecked = true;
                    break;
                case ToastNotification.DisplayPosition.TopRight:
                    this.MenuItemTopRight.IsChecked = true;
                    break;
                case ToastNotification.DisplayPosition.BottomLeft:
                    this.MenuItemBottomLeft.IsChecked = true;
                    break;
                case ToastNotification.DisplayPosition.BottomRight:
                    this.MenuItemBottomRight.IsChecked = true;
                    break;
            }
        }

        /// <summary>
        /// Load chapters from database and start the update service.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // TODO: Rethink the loading process and if the updater should have to wait until the database file has been loaded.

            var chapterProfilesViewModel = ServiceLocator.Current.GetService<ChapterProfilesViewModel>();
            chapterProfilesViewModel.ListViewModel.ChapterProfilesUnreadCountChanged += (s, e) => AppShell.Current?.UpdateBadge(e.UnreadCount);
        }

        private void OnMenuItemCloseClick(object sender, RoutedEventArgs e)
        {
            // TODO: Use Application.MainWindow.Close() here (less coupling, i.e. the main page shouldn't know an appshell exists)?
            AppShell.Current?.Close();
        }

        private void OnMenuItemAboutClick(object sender, RoutedEventArgs e)
        {
            var aboutView = new AboutView()
            {
                Owner = AppShell.Current,
            };

            aboutView.ShowDialog();
        }

        private void OnMenuItemTopLeftClick(object sender, RoutedEventArgs e)
        {
            var menuItem = (MenuItem)sender;
            if (!menuItem.IsChecked)
            {
                menuItem.IsChecked = true;
                return;
            }

            menuItem.IsChecked = true;

            this.MenuItemTopRight.IsChecked = false;
            this.MenuItemBottomLeft.IsChecked = false;
            this.MenuItemBottomRight.IsChecked = false;

            this.settingsViewModel.NotificationDisplayPosition = ToastNotification.DisplayPosition.TopLeft;
        }

        private void OnMenuItemTopRightClick(object sender, RoutedEventArgs e)
        {
            var menuItem = (MenuItem)sender;
            if (!menuItem.IsChecked)
            {
                menuItem.IsChecked = true;
                return;
            }

            menuItem.IsChecked = true;

            this.MenuItemTopLeft.IsChecked = false;
            this.MenuItemBottomLeft.IsChecked = false;
            this.MenuItemBottomRight.IsChecked = false;

            this.settingsViewModel.NotificationDisplayPosition = ToastNotification.DisplayPosition.TopRight;
        }

        private void OnMenuItemBottomLeftClick(object sender, RoutedEventArgs e)
        {
            var menuItem = (MenuItem)sender;
            if (!menuItem.IsChecked)
            {
                menuItem.IsChecked = true;
                return;
            }

            menuItem.IsChecked = true;

            this.MenuItemTopLeft.IsChecked = false;
            this.MenuItemTopRight.IsChecked = false;
            this.MenuItemBottomRight.IsChecked = false;

            this.settingsViewModel.NotificationDisplayPosition = ToastNotification.DisplayPosition.BottomLeft;
        }

        private void OnMenuItemBottomRightClick(object sender, RoutedEventArgs e)
        {
            var menuItem = (MenuItem)sender;
            if (!menuItem.IsChecked)
            {
                menuItem.IsChecked = true;
                return;
            }

            menuItem.IsChecked = true;

            this.MenuItemTopLeft.IsChecked = false;
            this.MenuItemTopRight.IsChecked = false;
            this.MenuItemBottomLeft.IsChecked = false;

            this.settingsViewModel.NotificationDisplayPosition = ToastNotification.DisplayPosition.BottomRight;
        }

        private void OnMenuItemFeedbackClick(object sender, RoutedEventArgs e)
        {
            WebUtility.OpenWebsite(AppConfiguration.FeedbackUri);
        }
    }
}
