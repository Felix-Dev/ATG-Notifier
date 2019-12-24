using ATG_Notifier.Desktop.Components;
using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.Helpers;
using ATG_Notifier.Desktop.Models;
using ATG_Notifier.Desktop.Services;
using ATG_Notifier.Desktop.ViewModels;
using ATG_Notifier.Desktop.WPF.Helpers.Extensions;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Interop;

namespace ATG_Notifier.Desktop.Views
{
    internal partial class AppShell : Window
    {
        private const int SurfaceAreaMinWidth = 100;
        private const int SurfaceAreaMinHeight = 100;

        private readonly DialogService dialogService;
        private readonly SettingsViewModel settingsViewModel;
        private readonly TaskbarButtonService taskbarButtonService;

        private bool canCancel = true;

        private NotificationIcon notificationIcon = null!;

        private AppTaskbarButtonMode currentTaskbarButtonMode;
        private TaskbarButtonResetMode currentTaskbarButtonResetMode;

        public AppShell()
        {
            Current = this;

            this.dialogService = ServiceLocator.Current.GetService<DialogService>();
            this.settingsViewModel = ServiceLocator.Current.GetService<SettingsViewModel>();
            this.taskbarButtonService = ServiceLocator.Current.GetService<TaskbarButtonService>();

            InitializeComponent();

            SetWindowsPosition();
        }

        public static AppShell? Current { get; private set; }

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

        /// <summary>
        /// Updates the badge counter of the app's icon in Window's notification area whenever the number of 
        /// unread chapter profiles in the app changes.
        /// </summary>
        public void UpdateBadge(int number)
        {
            CommonHelpers.RunOnUIThread(() => this.notificationIcon.UpdateBadge(number));
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

        private void OnInitialized(object sender, EventArgs e)
        {
            JumplistManager.BuildJumplist();

            // show program icon in Windows notification area
            this.notificationIcon = new NotificationIcon(Properties.Resources.logo_16_ld4_icon, AppConfiguration.AppId);
            this.notificationIcon.Show();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.ContentFrame.Navigate(new Uri("Views/MainPage.xaml", UriKind.Relative));
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            var source = PresentationSource.FromVisual(this) as HwndSource;
            source?.AddHook(WndProc);
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
            if (this.canCancel && this.settingsViewModel.KeepRunningOnClose)
            {
                this.WindowState = WindowState.Minimized;
                Hide();

                e.Cancel = true;
            }

            SaveWindowPosition();
        }

        protected override void OnClosed(EventArgs e)
        {
            Cleanup();
        }

        private void Cleanup()
        {
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
                this.settingsViewModel.WindowSetting = new WindowSetting(this.RestoreBounds.X, this.RestoreBounds.Y, this.RestoreBounds.Width, this.RestoreBounds.Height);
            }
            else
            {
                this.settingsViewModel.WindowSetting = new WindowSetting(this.Left, this.Top, this.Width, this.Height);
            }
        }

        private void SetWindowsPosition()
        {
            if (!(this.settingsViewModel.WindowSetting is WindowSetting prevWindowSetting))
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
    }
}
