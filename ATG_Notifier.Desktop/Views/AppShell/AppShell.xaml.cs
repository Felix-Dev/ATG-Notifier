﻿using ATG_Notifier.Desktop.Views.Shell;
using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.Helpers;
using ATG_Notifier.Desktop.Models;
using ATG_Notifier.Desktop.Services;
using ATG_Notifier.Desktop.Helpers.Extensions;
using ATG_Notifier.ViewModels.Services;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Input;
using System.Windows.Controls;
using ATG_Notifier.Desktop.ViewModels;
using System.Windows.Data;

namespace ATG_Notifier.Desktop.Views
{
    // TODO: 
    //      - Listen to notification unread amount changes (and stop listening when we shutdown the app shell)
    //        (perhaps we can use a weak event handling system for notification unread changes?)
    internal partial class AppShell : Window
    {
        private const int SurfaceAreaMinWidth = 100;
        private const int SurfaceAreaMinHeight = 100;

#if DEBUG
        private const string NotificationTitle = "ATG Chapter Update! (Debug)";
#else
        private const string NotificationTitle = "ATG Chapter Update!";
#endif

        private readonly DialogService dialogService;
        private readonly ChapterProfileServicePoint chapterProfileServicePoint;
        private readonly ToastNotificationService notificationManager;
        private readonly TaskbarButtonService taskbarButtonService;
        private readonly IUpdateService updateService;

        private bool canCancel = true;

        private NotificationIcon notificationIcon = null!;

        private AppTaskbarButtonMode currentTaskbarButtonMode;
        private TaskbarButtonResetMode currentTaskbarButtonResetMode;

        private readonly AppSettings appSettings;
        private readonly AppState appState;

        public AppShell()
        {
            Current = this;

            this.dialogService = ServiceLocator.Current.GetService<DialogService>();
            this.notificationManager = ServiceLocator.Current.GetService<ToastNotificationService>();
            this.taskbarButtonService = ServiceLocator.Current.GetService<TaskbarButtonService>();

            this.updateService = ServiceLocator.Current.GetService<IUpdateService>();

            this.chapterProfileServicePoint = ServiceLocator.Current.GetService<ChapterProfileServicePoint>();

            this.appSettings = ServiceLocator.Current.GetService<AppSettings>();
            this.appState = ServiceLocator.Current.GetService<AppState>();

            InitializeComponent();

            SetWindowsPosition();
        }

        public static AppShell? Current { get; private set; }

        public async Task InitializeAsync(bool showMinimized)
        {
            // build the jumplist for the app's taskbar button
            JumplistManager.BuildJumplist();

            // show program icon in Windows notification area
            this.notificationIcon = new NotificationIcon(new Icon(Properties.Resources.AppLogo, 16, 16), AppConfiguration.AppId);

            //if (showMinimized)
            //{
            //    var chapterProfileService = ServiceLocator.Current.GetService<IChapterProfileService>();
            //    var chapterProfiles = await chapterProfileService.GetChapterProfilesAsync();

            //    int unreadChapterCount = 0;
            //    foreach (var chapterProfile in chapterProfiles)
            //    {
            //        if (!chapterProfile.IsRead)
            //        {
            //            unreadChapterCount++;
            //        }
            //    }

            //    this.notificationIcon.UpdateBadge(unreadChapterCount);
            //}

            this.notificationIcon.UpdateBadge(this.appState.UnreadChapters);

            this.notificationIcon.Show();

            if (showMinimized)
            {
                // hide the app shell
                this.WindowState = WindowState.Minimized;
                Hide();
            }
            else
            {
                Show();
            }

            // start listening for chapter updates so we can notify the user
            this.updateService.ChapterUpdated += OnUpdateServiceChapterUpdated;

            this.chapterProfileServicePoint.ChapterProfilesUnreadCountChanged += OnChapterProfilesUnreadCountChanged;

            return;
        }

        public new bool BringIntoView()
        {
            if (this.WindowState == WindowState.Minimized)
            {
                Show();
                this.WindowState = WindowState.Normal;

                return true;
            }
            else
            {
                return Activate();
            }
        }

        public void Close(bool isCancelable = true)
        {
            this.canCancel = isCancelable;
            base.Close();

            this.canCancel = true;
        }

        public void SetTaskbarButtonMode(AppTaskbarButtonMode mode, TaskbarButtonResetMode resetMode = TaskbarButtonResetMode.AppActivated)
        {
            switch (mode)
            {
                case AppTaskbarButtonMode.None:
                    this.taskbarButtonService.SetButtonState(TaskbarButtonState.None);
                    break;
                case AppTaskbarButtonMode.Attention:
                    this.taskbarButtonService.FlashButton();
                    break;
                case AppTaskbarButtonMode.Paused:
                    if (resetMode == TaskbarButtonResetMode.AppActivated && CommonHelpers.RunOnUIThread(() => this.IsActive))
                    {
                        this.currentTaskbarButtonMode = AppTaskbarButtonMode.None;
                        this.taskbarButtonService.SetButtonState(TaskbarButtonState.None);
                        return;
                    }

                    this.taskbarButtonService.SetButtonState(TaskbarButtonState.Paused);
                    break;
                case AppTaskbarButtonMode.Error:
                    if (resetMode == TaskbarButtonResetMode.AppActivated && CommonHelpers.RunOnUIThread(() => this.IsActive))
                    {
                        this.currentTaskbarButtonMode = AppTaskbarButtonMode.None;
                        this.taskbarButtonService.SetButtonState(TaskbarButtonState.None);
                        return;
                    }

                    this.taskbarButtonService.SetButtonState(TaskbarButtonState.Error);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode));
            }

            this.currentTaskbarButtonMode = mode;
            this.currentTaskbarButtonResetMode = resetMode;
        }

        public void SaveAndCleanup()
        {
            SaveWindowPosition();
            Cleanup();
        }

        /// <summary>
        /// Updates the badge counter of the app's icon in Window's notification area whenever the number of 
        /// unread chapter profiles in the app changes.
        /// </summary>
        private void UpdateBadge(int number)
        {
            // TODO: This code might throw a NullReference exception for this.notificationIcon when the timing is bad.
            CommonHelpers.RunOnUIThread(() => this.notificationIcon.UpdateBadge(number));
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.ContentFrame.Navigate(new Uri("Views/MainPage.xaml", UriKind.Relative));
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            if (PresentationSource.FromVisual(this) is HwndSource source)
            {
                source.AddHook(WndProc);

                // Don't show the app icon in the titlebar. This mimics the behavior of UWP apps.
                WindowWin32InteropHelper.HideIcon(source.Handle);
            }
            else
            {
                throw new InvalidOperationException("Critical error: AppShell is not in a valid state! The AppShell window might not exist.");
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            if (this.currentTaskbarButtonResetMode == TaskbarButtonResetMode.AppActivated
                && (this.currentTaskbarButtonMode == AppTaskbarButtonMode.Paused || this.currentTaskbarButtonMode == AppTaskbarButtonMode.Error))
            {
                this.taskbarButtonService.SetButtonState(TaskbarButtonState.None);
                this.currentTaskbarButtonResetMode = TaskbarButtonResetMode.None;
            }

            base.OnActivated(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (this.canCancel && this.appSettings.KeepRunningOnClose)
            {
                this.WindowState = WindowState.Minimized;
                Hide();

                e.Cancel = true;
            }
            else
            {
                this.updateService.ChapterUpdated -= OnUpdateServiceChapterUpdated;
                this.chapterProfileServicePoint.ChapterProfilesUnreadCountChanged -= OnChapterProfilesUnreadCountChanged;
            }

            SaveWindowPosition();
        }

        protected override void OnClosed(EventArgs e)
        {
            Cleanup();
        }

        private void Cleanup()
        {
            this.updateService.ChapterUpdated -= OnUpdateServiceChapterUpdated;

            // clear jumplist from added custom entries
            JumplistManager.ClearJumplist();

            // remove icon from Windows notification area
            this.notificationIcon.Hide();
            this.notificationIcon.Dispose();

            this.notificationIcon = null!;
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WindowWin32InteropHelper.WM_EXIT)
            {
                Close(false);
                return new IntPtr(1);
            }
            else if (msg == WindowWin32InteropHelper.WM_SHOWINSTANCE)
            {
                BringIntoView();
                return new IntPtr(1);
            }

            return IntPtr.Zero;
        }

        private void SaveWindowPosition()
        {
            if (this.WindowState == WindowState.Minimized || this.WindowState == WindowState.Maximized)
            {
                this.appState.WindowLocation = new WindowLocation(this.RestoreBounds.X, this.RestoreBounds.Y, this.RestoreBounds.Width, this.RestoreBounds.Height);
            }
            else
            {
                this.appState.WindowLocation = new WindowLocation(this.Left, this.Top, this.Width, this.Height);
            }
        }

        private void SetWindowsPosition()
        {
            Rect? screenBoundsRef = this.GetScreenBounds();
            if (!screenBoundsRef.HasValue)
            {
                return;
            }

            if (this.appState.WindowLocation is WindowLocation previousWindowLocation)
            {
                var screenBounds = screenBoundsRef.Value;
                if (previousWindowLocation.X >= screenBounds.Width
                    || previousWindowLocation.Y < -8 || previousWindowLocation.Y >= screenBounds.Height
                    || previousWindowLocation.Width < this.MinWidth || previousWindowLocation.Height < this.MinHeight
                    || previousWindowLocation.X < 0 && previousWindowLocation.X + previousWindowLocation.Width < SurfaceAreaMinWidth
                    || screenBounds.Width - previousWindowLocation.X < SurfaceAreaMinWidth
                    || screenBounds.Height - previousWindowLocation.Y < SurfaceAreaMinHeight)
                {
                    return;
                }

                this.Left = previousWindowLocation.X;
                this.Top = previousWindowLocation.Y;
                this.Width = previousWindowLocation.Width;
                this.Height = previousWindowLocation.Height;
            }          
        }

        private void OnUpdateServiceChapterUpdated(object? sender, ChapterUpdateEventArgs e)
        {
            SetTaskbarButtonMode(AppTaskbarButtonMode.Attention);

            if (!this.appSettings.IsFocusModeEnabled)
            {
                this.notificationManager.Show(NotificationTitle, e.ChapterProfileViewModel);
            }
        }

        private void OnChapterProfilesUnreadCountChanged(object? sender, ChapterProfilesUnreadCountChangedEventArgs e)
        {
            UpdateBadge(e.Count);
        }

        private void OnWindowMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Keyboard.FocusedElement is TextBox textBox && textBox.IsSelectionActive)
            {
                textBox.SelectionStart = 0;
                textBox.SelectionLength = 0;
                //textBox.RaiseEvent(new RoutedEventArgs(UIElement.LostFocusEvent));
                //Keyboard.ClearFocus();
            }
        }
    }
}
