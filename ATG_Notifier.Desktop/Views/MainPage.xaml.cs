using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.Controls;
using ATG_Notifier.Desktop.Services;
using ATG_Notifier.Desktop.Utilities;
using ATG_Notifier.Desktop.ViewModels;
using ATG_Notifier.ViewModels.Services;
using ATG_Notifier.ViewModels.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ATG_Notifier.Desktop.Views
{
    internal partial class MainPage : NavigationPage
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

        protected override void OnNavigatedTo(NavigationEventArgs2 args)
        {
            if (args.Parameter is MainPageArgs mArgs)
            {
                this.ChapterProfilesViewControl.SetPayload(new ChapterProfileListArgs() { ChapterProfileViewModel = mArgs.ChapterProfileViewModel });
            }
            else
            {
                this.ChapterProfilesViewControl.SetPayload(ChapterProfileListArgs.CreateDefault());
            }
        }

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

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
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

        private void OnMenuPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Keyboard.FocusedElement is TextBox textBox && textBox.IsSelectionActive)
            {
                textBox.SelectionStart = 0;
                textBox.SelectionLength = 0;
                //textBox.RaiseEvent(new RoutedEventArgs(UIElement.LostFocusEvent));
                //Keyboard.ClearFocus();
            }
        }

        private void OnMenuPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is MenuItem menuItem && menuItem.Role == MenuItemRole.TopLevelHeader
                && Keyboard.FocusedElement is TextBox textBox && textBox.IsSelectionActive)
            {
                textBox.SelectionStart = 0;
                textBox.SelectionLength = 0;
                //textBox.RaiseEvent(new RoutedEventArgs(UIElement.LostFocusEvent));
                //Keyboard.ClearFocus();
            }
        }
    }
}
