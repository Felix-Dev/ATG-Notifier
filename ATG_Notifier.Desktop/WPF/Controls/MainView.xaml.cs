﻿using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.Services;
using ATG_Notifier.Desktop.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ATG_Notifier.Desktop.WPF.Controls
{
    internal partial class MainView : UserControl
    {
        private readonly System.Windows.Forms.Form owner;
        private readonly DialogService dialogService;

        public MainView(System.Windows.Forms.Form owner)
        {
            this.DataContext = ServiceLocator.Current.GetService<MainViewModel>();

            this.dialogService = ServiceLocator.Current.GetService<DialogService>();

            InitializeComponent();

            this.owner = owner;

            switch (ViewModel.SettingsViewModel.NotificationDisplayPosition)
            {
                case Views.ToastNotification.DisplayPosition.TopLeft:
                    this.MenuItemTopLeft.IsChecked = true;
                    break;
                case Views.ToastNotification.DisplayPosition.TopRight:
                    this.MenuItemTopRight.IsChecked = true;
                    break;
                case Views.ToastNotification.DisplayPosition.BottomLeft:
                    this.MenuItemBottomLeft.IsChecked = true;
                    break;
                case Views.ToastNotification.DisplayPosition.BottomRight:
                    this.MenuItemBottomRight.IsChecked = true;
                    break;
            }
        }

        public MainViewModel ViewModel => (MainViewModel)this.DataContext;

        private void OnMenuItemCloseClick(object sender, RoutedEventArgs e)
        {
            this.owner.Close();
        }

        private void OnMenuItemAboutClick(object sender, RoutedEventArgs e)
        {
            dialogService.ShowDialog($"Version: {AppConfiguration.AppVersion}", $"About {AppConfiguration.AppId}");
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