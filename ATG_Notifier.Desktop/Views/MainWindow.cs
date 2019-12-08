using ATG_Notifier.Desktop.Components;
using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.Models;
using ATG_Notifier.Desktop.Utilities;
using ATG_Notifier.Desktop.ViewModels;
using ATG_Notifier.Desktop.WPF.Controls;
using ATG_Notifier.ViewModels.Services;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ATG_Notifier.Desktop.Views
{
    internal partial class MainWindow : Form
    {
        private const int SurfaceAreaMinWidth = 100;
        private const int SurfaceAreaMinHeight = 100;

        private readonly static ILogService logService = ServiceLocator.Current.GetService<ILogService>();

        private readonly IUpdateService updateService = ServiceLocator.Current.GetService<IUpdateService>();

        private readonly SettingsViewModel appSettings;

        private readonly ChapterProfilesViewModel chapterProfilesViewModel;

        private NotificationIcon notificationIcon;

        public MainWindow()
        {
            InitializeComponent();

            // TODO: enable full monitor width here? If yes, we can switch to WPF windows...
            //this.MaximumSize = new Size(this.MaximumSize.Width, Screen.GetWorkingArea(this).Height);

            Rectangle screenArea = Screen.GetWorkingArea(this);
            this.MaximumSize = new Size(screenArea.Width, screenArea.Height);
            this.Text = AppConfiguration.AppId;

            this.appSettings = ServiceLocator.Current.GetService<SettingsViewModel>();

            RestorePreviousWindowsPosition();

            //this.chaptersListViewModel = ServiceLocator.Current.GetService<ChapterProfilesListViewModel>();
            //this.chaptersListViewModel.ChapterProfilesUnreadCountChanged += OnChapterProfilesUnreadCountChanged;

            this.chapterProfilesViewModel = ServiceLocator.Current.GetService<ChapterProfilesViewModel>();
            this.chapterProfilesViewModel.ListViewModel.ChapterProfilesUnreadCountChanged += OnChapterProfilesUnreadCountChanged;

            this.wpfHost.Child = new MainView(this);
            this.wpfHost.Visible = true;

            //JumplistManager.BuildJumplist();
            this.notificationIcon = new NotificationIcon(Properties.Resources.logo_16_ld4_icon, AppConfiguration.AppId);
        }

        public new void BringToFront()
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Show();
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                Activate();
            }
        }

        /// <summary>
        /// Handles our custom window messages such as jumplist commands.
        /// </summary>
        /// <param name="msg">The window message to handle.</param>
        protected override void WndProc(ref Message msg)
        {
            // terminate the application when we receive our custom exit message.
            if (msg.Msg == WindowsMessageHelper.WM_EXIT)
            {
                // Calling WinForms Exit() method here, so that we terminate the app insted of keeping it running (Form ClosingReason = ApplicationExit).
                // We don't directly call WPFs Application Exit method here as any normal app termination will first close this window and then exits the app
                // (so that we can handle <keep running on closed). In fact, we never call WPFs Exit method but in case of an unhandled exception just quit directly.
                Application.Exit();
            }
            // bring the current instance to the foreground when we receive out custom show-instance message.
            else if (msg.Msg == WindowsMessageHelper.WM_SHOWINSTANCE)
            {
                BringToFront();
            }
            else
            {
                // not one of our custom messages, pass it to the system so it can handle this message.
                base.WndProc(ref msg);
            }
        }

        /// <summary>
        /// Loads the saved chapter profiles from the database and starts the update service.
        /// </summary>
        /// <param name="e">The event data.</param>
        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // load chapter profiles from database 
            await this.chapterProfilesViewModel.ListViewModel.LoadAsync();

            if (this.appSettings.IsUpdateServiceRunning)
            {
                this.updateService.Start();
            }   
        }

        /// <summary>
        /// Shows the app icon in the notification area and creates the app's taskbar jumplist.
        /// </summary>
        /// <param name="e">The event data.</param>
        protected override void OnShown(EventArgs e)
        {
            // show program icon in Windows notification area
            this.notificationIcon.Show();

            base.OnShown(e);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && this.appSettings.KeepRunningOnClose)
            {
                this.WindowState = FormWindowState.Minimized;
                Hide();

                e.Cancel = true;
            }

            SaveWindowPosition();

            base.OnFormClosing(e);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            Exit();
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

            System.Windows.Application.Current.Shutdown();
        }

        private void RestorePreviousWindowsPosition()
        {
            if (!(this.appSettings.WindowSetting is WindowSetting prevWindowSetting)
                || !(Screen.PrimaryScreen?.Bounds is Rectangle screenClientArea))
            {
                return;
            }

            if (prevWindowSetting.X >= screenClientArea.Width
                || prevWindowSetting.Y < -8 || prevWindowSetting.Y >= screenClientArea.Height)
            {
                return;
            }

            if (prevWindowSetting.Width < this.MinimumSize.Width || prevWindowSetting.Height < this.MinimumSize.Height)
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

            this.Location = new Point((int)prevWindowSetting.X, (int)prevWindowSetting.Y);

            this.Width = (int)prevWindowSetting.Width;
            this.Height = (int)prevWindowSetting.Height;
        }

        private void SaveWindowPosition()
        {
            if (this.WindowState == FormWindowState.Minimized || this.WindowState == FormWindowState.Maximized)
            {
                this.appSettings.WindowSetting = new WindowSetting(this.RestoreBounds.X, this.RestoreBounds.Y, this.RestoreBounds.Width, this.RestoreBounds.Height);
            }
            else
            {
                this.appSettings.WindowSetting = new WindowSetting(this.Left, this.Top, this.Width, this.Height);
            }
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
    }
}
