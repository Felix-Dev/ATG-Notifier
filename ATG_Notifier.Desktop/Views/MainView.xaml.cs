using ATG_Notifier.Desktop.Components;
using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.Helpers;
using ATG_Notifier.Desktop.Models;
using ATG_Notifier.Desktop.Services;
using ATG_Notifier.Desktop.ViewModels;
using ATG_Notifier.Desktop.WPF.Helpers.Extensions;
using ATG_Notifier.ViewModels.Services;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace ATG_Notifier.Desktop.Views
{
    internal partial class MainView : Window
    {
        private const int SurfaceAreaMinWidth = 100;
        private const int SurfaceAreaMinHeight = 100;

        private readonly DialogService dialogService;
        private readonly static ILogService logService = ServiceLocator.Current.GetService<ILogService>();

        private readonly IUpdateService updateService = ServiceLocator.Current.GetService<IUpdateService>();

        private readonly SettingsViewModel appSettings;

        private readonly ChapterProfilesViewModel chapterProfilesViewModel;

        private NotificationIcon notificationIcon;

        public MainView()
        {
            this.dialogService = ServiceLocator.Current.GetService<DialogService>();
            this.DataContext = ServiceLocator.Current.GetService<MainViewModel>();

            InitializeComponent();

            this.appSettings = ServiceLocator.Current.GetService<SettingsViewModel>();

            SetWindowsPosition();

            this.chapterProfilesViewModel = ServiceLocator.Current.GetService<ChapterProfilesViewModel>();
            this.chapterProfilesViewModel.ListViewModel.ChapterProfilesUnreadCountChanged += OnChapterProfilesUnreadCountChanged;

            //JumplistManager.BuildJumplist();
            this.notificationIcon = new NotificationIcon(Properties.Resources.logo_16_ld4_icon, AppConfiguration.AppId);

            switch (ViewModel.SettingsViewModel.NotificationDisplayPosition)
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

            Loaded += OnLoaded;
        }

        public MainViewModel ViewModel => (MainViewModel)this.DataContext;

        public new void BringIntoView()
        {
            if (this.WindowState == WindowState.Minimized)
            {
                Show();
                this.WindowState = WindowState.Normal;
            }
            else
            {
                Activate();
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            var source = PresentationSource.FromVisual(this) as HwndSource;
            source?.AddHook(WndProc);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (this.appSettings.KeepRunningOnClose)
            {
                this.WindowState = WindowState.Minimized;
                Hide();

                e.Cancel = true;
            }

            SaveWindowPosition();
        }

        protected override void OnClosed(EventArgs e)
        {
            Exit();
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WindowWin32InteropHelper.WM_EXIT)
            {
                Application.Current.Shutdown();
            }
            else if (msg == WindowWin32InteropHelper.WM_SHOWINSTANCE)
            {
                BringIntoView();
            }

            return IntPtr.Zero;
        }

        /// <summary>
        /// Loads the saved chapter profiles from the database and starts the update service.
        /// </summary>
        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            // show program icon in Windows notification area
            this.notificationIcon.Show();

            // load chapter profiles from database 
            await this.chapterProfilesViewModel.ListViewModel.LoadAsync();

            if (this.appSettings.IsUpdateServiceRunning)
            {
                this.updateService.Start();
            }
        }

        private void Exit()
        {
            if (this.appSettings.IsUpdateServiceRunning)
            {
                this.updateService.Stop();
            }

            this.chapterProfilesViewModel.ListViewModel.ChapterProfilesUnreadCountChanged -= OnChapterProfilesUnreadCountChanged;

            // remove icon from Windows notification area
            this.notificationIcon.Hide();
            this.notificationIcon.Dispose();

            //Application.Current.Shutdown();
        }

        private void SaveWindowPosition()
        {
            if (this.WindowState == WindowState.Minimized || this.WindowState == WindowState.Maximized)
            {
                this.appSettings.WindowSetting = new WindowSetting(this.RestoreBounds.X, this.RestoreBounds.Y, this.RestoreBounds.Width, this.RestoreBounds.Height);
            }
            else
            {
                this.appSettings.WindowSetting = new WindowSetting(this.Left, this.Top, this.Width, this.Height);
            }
        }

        private void SetWindowsPosition()
        {
            if (!(this.appSettings.WindowSetting is WindowSetting prevWindowSetting))
            {
                return;
            }

            Rect? screenBoundsRef = this.GetScreenBounds();
            if (!screenBoundsRef.HasValue)
            {
                return;
            }

            var screenBounds = screenBoundsRef.Value;
            if (prevWindowSetting.X >= screenBounds.Width
                || prevWindowSetting.Y < -8 || prevWindowSetting.Y >= screenBounds.Height
                || prevWindowSetting.Width < this.MinWidth || prevWindowSetting.Height < this.MinHeight
                || prevWindowSetting.X < 0 && prevWindowSetting.X + prevWindowSetting.Width < SurfaceAreaMinWidth
                || screenBounds.Width - prevWindowSetting.X < SurfaceAreaMinWidth
                || screenBounds.Height - prevWindowSetting.Y < SurfaceAreaMinHeight)
            {
                return;
            }

            this.Left = prevWindowSetting.X;
            this.Top = prevWindowSetting.Y;
            this.Width = prevWindowSetting.Width;
            this.Height = prevWindowSetting.Height;
        }

        /// <summary>
        /// Updates the badge counter of the app's icon in Window's notification area whenever the number of 
        /// unread chapter profiles in the app changes.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data. Contains the new unread-chapter-profiles count.</param>
        private void OnChapterProfilesUnreadCountChanged(object? sender, ChapterProfilesUnreadCountChangedEventArgs e)
        {
            this.notificationIcon.UpdateBadge(e.UnreadCount);
        }

        private void OnMenuItemCloseClick(object sender, RoutedEventArgs e)
        {
            //Application.Current.Shutdown();

            Close();
        }

        private void OnMenuItemAboutClick(object sender, RoutedEventArgs e)
        {
            dialogService.ShowDialog($"Version: {AppConfiguration.AppVersion}", $"About {AppConfiguration.AppId}", MessageDialogButton.OK);
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

            this.ViewModel.SettingsViewModel.NotificationDisplayPosition = Views.ToastNotification.DisplayPosition.TopLeft;
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

            this.ViewModel.SettingsViewModel.NotificationDisplayPosition = Views.ToastNotification.DisplayPosition.TopRight;
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

            this.ViewModel.SettingsViewModel.NotificationDisplayPosition = Views.ToastNotification.DisplayPosition.BottomLeft;
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

            this.ViewModel.SettingsViewModel.NotificationDisplayPosition = Views.ToastNotification.DisplayPosition.BottomRight;
        }
    }
}
