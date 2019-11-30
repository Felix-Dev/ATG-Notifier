using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.Models;
using ATG_Notifier.Desktop.Native.Win32;
using ATG_Notifier.Desktop.Services;
using ATG_Notifier.Desktop.Utilities;
using ATG_Notifier.Desktop.ViewModels;
using ATG_Notifier.Desktop.WPF.Helpers;
using ATG_Notifier.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ATG_Notifier.Desktop.Views
{
    internal partial class MainWindow2 : Window
    {
        private const int SurfaceAreaMinWidth = 100;
        private const int SurfaceAreaMinHeight = 100;

        private readonly static ILogService logService = ServiceLocator.Current.GetService<ILogService>();

        private readonly IUpdateService updateService = ServiceLocator.Current.GetService<IUpdateService>();

        private readonly SettingsViewModel appSettings;

        private readonly ChapterProfilesViewModel chapterProfilesViewModel;

        private NotificationIcon notificationIcon;

        public MainWindow2()
        {
            this.DataContext = ServiceLocator.Current.GetService<MainWindowViewModel>();

            this.InitializeComponent();

            //var screenBounds = WindowHelper.CurrentWindowBoundRectangle(this);

            //this.MaxHeight = screenBounds.Height;
            this.Title = AppConfiguration.AppId;

            this.appSettings = ServiceLocator.Current.GetService<SettingsViewModel>();

            RestorePreviousWindowsPosition();

            this.chapterProfilesViewModel = ServiceLocator.Current.GetService<ChapterProfilesViewModel>();
            this.chapterProfilesViewModel.ListViewModel.ChapterProfilesUnreadCountChanged += OnChapterProfilesUnreadCountChanged;

            this.notificationIcon = new NotificationIcon(Properties.Resources.logo_16_ld4_icon, AppConfiguration.AppId);
            this.notificationIcon.Show();

            JumplistManager.BuildJumplistWpf();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            source.AddHook(WindowProc);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (this.appSettings.KeepRunningOnClose)
            {
                this.WindowState = WindowState.Minimized;
                Hide();

                e.Cancel = true;
            }

            // We have to save the winos position here because that data is not available any longer 
            // when the window has been closed.
            SaveWindowPosition();
        }

        protected override void OnClosed(EventArgs e)
        {
            //SaveWindowPosition();

            this.notificationIcon.Dispose();
            this.notificationIcon = null;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            // load chapter profiles from database 
            //await Task.Run(() => this.chapterProfilesViewModel.ListViewModel.LoadAsync());

            //if (!this.chapterProfilesViewModel.ListViewModel.IsEmpty)
            //{
            //}

            //if (this.appSettings.IsUpdateServiceRunning)
            //{
            //    this.updateService.Start();
            //}
        }

        private void RestorePreviousWindowsPosition()
        {
            if (!(this.appSettings.WindowSetting is WindowSetting prevWindowSetting)
                || !(System.Windows.Forms.Screen.PrimaryScreen?.Bounds is System.Drawing.Rectangle screenClientArea))
            {
                return;
            }

            if (prevWindowSetting.X >= screenClientArea.Width
                || prevWindowSetting.Y < -8 || prevWindowSetting.Y >= screenClientArea.Height)
            {
                return;
            }

            if (prevWindowSetting.Width < this.MinWidth || prevWindowSetting.Height < this.MinHeight)
            {
                return;
            }

            if (prevWindowSetting.X < 0 && prevWindowSetting.X + prevWindowSetting.Width < SurfaceAreaMinWidth)
            {
                return;
            }

            if (screenClientArea.Width - prevWindowSetting.X < SurfaceAreaMinWidth)
            {
                return;
            }

            if (screenClientArea.Height - prevWindowSetting.Y < SurfaceAreaMinHeight)
            {
                return;
            }

            this.Left = prevWindowSetting.X;
            this.Top = prevWindowSetting.Y;

            this.Width = prevWindowSetting.Width;
            this.Height = prevWindowSetting.Height;
        }

        private void SaveWindowPosition()
        {
            this.appSettings.WindowSetting = new WindowSetting(this.RestoreBounds.Left, this.RestoreBounds.Top, this.RestoreBounds.Width, this.RestoreBounds.Height);
        }

        public void BringToFront()
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

        private void OnChapterProfilesUnreadCountChanged(object sender, ChapterProfilesUnreadCountChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WindowsMessageHelper.WM_SHOWINSTANCE)
            {
                BringToFront();
            }
            else if (msg == WindowsMessageHelper.WM_EXIT)
            {
                Application.Current.Shutdown();
            }

            return (IntPtr)0;
        }

        private void OnMenuItemAboutClick(object sender, RoutedEventArgs e)
        {
            var aboutView = new AboutView2(AppConfiguration.AppVersion)
            {
                Owner = this,
            };

            aboutView.ShowDialog();
        }

        private void OnMenuItemCloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
