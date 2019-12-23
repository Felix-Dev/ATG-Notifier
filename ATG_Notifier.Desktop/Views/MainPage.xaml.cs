﻿using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.Services;
using ATG_Notifier.Desktop.ViewModels;
using ATG_Notifier.ViewModels.Services;
using System.Windows;
using System.Windows.Controls;

namespace ATG_Notifier.Desktop.Views
{
    internal partial class MainPage : Page
    {
        private readonly DialogService dialogService;
        private readonly SettingsViewModel settingsViewModel;

        public MainPage()
        {
            this.dialogService = ServiceLocator.Current.GetService<DialogService>();
            this.settingsViewModel = ServiceLocator.Current.GetService<SettingsViewModel>();

            this.DataContext = this;

            InitializeComponent();
            InitializeNotificationDisplayPositionMenu();
        }

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
        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            // TODO: Rethink the loading process and if the updater should have to wait until the database file has been loaded.

            var chapterProfilesViewModel = ServiceLocator.Current.GetService<ChapterProfilesViewModel>();
            chapterProfilesViewModel.ListViewModel.ChapterProfilesUnreadCountChanged += (s, e) => AppShell.Current?.UpdateBadge(e.UnreadCount);

            // load chapter profiles from database 
            await chapterProfilesViewModel.ListViewModel.LoadAsync();

            if (this.settingsViewModel.IsUpdateServiceRunning)
            {
                ServiceLocator.Current.GetService<IUpdateService>().Start();
            }
        }

        private void OnMenuItemCloseClick(object sender, RoutedEventArgs e)
        {
            AppShell.Current?.Close();
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

            this.settingsViewModel.NotificationDisplayPosition = Views.ToastNotification.DisplayPosition.TopLeft;
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

            this.settingsViewModel.NotificationDisplayPosition = Views.ToastNotification.DisplayPosition.TopRight;
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

            this.settingsViewModel.NotificationDisplayPosition = Views.ToastNotification.DisplayPosition.BottomLeft;
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

            this.settingsViewModel.NotificationDisplayPosition = Views.ToastNotification.DisplayPosition.BottomRight;
        }
    }
}
