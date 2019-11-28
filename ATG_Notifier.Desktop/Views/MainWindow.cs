using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.Helpers;
using ATG_Notifier.Desktop.Models;
using ATG_Notifier.Desktop.Utilities;
using ATG_Notifier.Desktop.Utilities.Bindings;
using ATG_Notifier.Desktop.ViewModels;
using ATG_Notifier.Desktop.WPF.Controls;
using ATG_Notifier.ViewModels.Services;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
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

        public MainWindow()
        {
            InitializeComponent();

            this.MaximumSize = new Size(Size.Width, Screen.GetWorkingArea(this).Height);
            this.Text = AppConfiguration.AppId;

            this.notifyIcon.Icon = Properties.Resources.logo_16_ld4_icon;
#if DEBUG
            this.notifyIcon.Text += " (Debug)";
#endif
            this.appSettings = ServiceLocator.Current.GetService<SettingsViewModel>();

            RestorePreviousWindowsPosition();

            //this.chaptersListViewModel = ServiceLocator.Current.GetService<ChapterProfilesListViewModel>();
            //this.chaptersListViewModel.ChapterProfilesUnreadCountChanged += OnChapterProfilesUnreadCountChanged;

            this.chapterProfilesViewModel = ServiceLocator.Current.GetService<ChapterProfilesViewModel>();
            this.chapterProfilesViewModel.ListViewModel.ChapterProfilesUnreadCountChanged += OnChapterProfilesUnreadCountChanged;

            this.wpfHost.Child = new ChapterProfilesView(this.chapterProfilesViewModel);
            this.wpfHost.Visible = true;

            SetupBindings();    
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

            this.Location = new Point(prevWindowSetting.X, prevWindowSetting.Y);

            this.Width = prevWindowSetting.Width;
            this.Height = prevWindowSetting.Height;
        }

        private void SaveWindowPosition()
        {
            this.appSettings.WindowSetting = new WindowSetting(this.Location.X, this.Location.Y, this.Width, this.Height);
        }

        private void SetupBindings()
        {
            var binding = new Binding(nameof(BindableMenuItem.Checked), Properties.Settings.Default, nameof(Properties.Settings.NotificationDisplayPosition),
                true,
                DataSourceUpdateMode.OnPropertyChanged);
            binding.Format += Binding_Format;

            this.menuItemNotificationPositionTopLeft.DataBindings.Add(binding);
        }

        /// <summary>
        /// Handles our custom window messages such as jumplist commands.
        /// </summary>
        /// <param name="msg">The window message to handle.</param>
        protected override void WndProc(ref Message msg)
        {
            // terminate the application when we receive our custom exit message.
            if (msg.Msg == WindowsMessageHelper.TaskCloseArg)
            {
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
            this.notifyIcon.Visible = true;

#if DesktopPackage2
            JumplistManager.BuildJumplistAsync();
#else

            // create a new taskbar jump list for the main window 
            JumplistManager.BuildJumplist(AppConfiguration.AppId, this.Handle);
#endif

            base.OnShown(e);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.WindowState = FormWindowState.Minimized;
                this.Hide();

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
            this.notifyIcon.Visible = false;
            this.notifyIcon.Dispose();

            //Environment.Exit(0);
            Application.Exit();
        }

        /// <summary>
        /// Updates the badge counter of the app's icon in Window's notification area whenever the number of 
        /// unread chapter profiles in the app changes.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data. Contains the new unread-chapter-profiles count.</param>
        private void OnChapterProfilesUnreadCountChanged(object sender, ChapterProfilesUnreadCountChangedEventArgs e)
        {
            CommonHelpers.RunOnUIThread(() =>
            {
                UpdateBadgeCounter(e.UnreadCount);
            });
        }

        private void UpdateBadgeCounter(int counter)
        {
            Graphics canvas;
            Bitmap iconBitmap = new Bitmap(this.notifyIcon.Icon.Width, this.notifyIcon.Icon.Height);
            canvas = Graphics.FromImage(iconBitmap);

            canvas.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //canvas.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            canvas.DrawIcon(Properties.Resources.logo_16_ld4_icon, 0, 0);

            // draw badge if counter is positive
            if (counter > 0)
            {
                canvas.FillEllipse(
                    new SolidBrush(Color.DarkRed),
                    new RectangleF(5, 4, this.notifyIcon.Icon.Width - 6, this.notifyIcon.Icon.Width - 6)
                    );

                canvas.DrawEllipse(
                    new Pen(Color.DarkGray),
                    new RectangleF(5, 4, this.notifyIcon.Icon.Width - 6, this.notifyIcon.Icon.Width - 6)
                    );

                if (counter <= 9)
                {
                    StringFormat format = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };

                    canvas.DrawString(
                        counter.ToString(),
                        new Font("Calibri", 7),
                        new SolidBrush(Color.White),
                        new RectangleF(5.7f, 5, this.notifyIcon.Icon.Width - 7, this.notifyIcon.Icon.Height - 5),
                        format
                    );
                }
                else
                {
                    StringFormat format = new StringFormat
                    {
                        Alignment = StringAlignment.Near
                    };

                    canvas.DrawString(
                        "9",
                        new Font("Calibri", 7),
                        new SolidBrush(Color.White),
                        new RectangleF(5.5f, 4f, this.notifyIcon.Icon.Width - 7, this.notifyIcon.Icon.Height - 5),
                        format
                    );

                    //format.Alignment = System.Drawing.StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Center;

                    canvas.DrawString(
                        "+",
                        new Font("Calibri", 6),
                        new SolidBrush(Color.White),
                        new RectangleF(9.5f, 4.5f, this.notifyIcon.Icon.Width - 7, this.notifyIcon.Icon.Height - 5),
                        format
                    );

                }
            }

            this.notifyIcon.Icon = Icon.FromHandle(iconBitmap.GetHicon());
        }

#region MenuItem Handler

        private void OnMenuItemPlayPopupSound_Click(object sender, EventArgs e)
        {
            this.appSettings.PlayPopupSound = !this.appSettings.PlayPopupSound;
        }

        private void OnMenuItemDisableOnFullscreen_Click(object sender, EventArgs e)
        {
            this.appSettings.DisableOnFullscreen = !this.appSettings.DisableOnFullscreen;
        }

        private void OnMenuItemDoNotDisturb_Click(object sender, EventArgs e)
        {
            this.appSettings.DoNotDisturb = !this.appSettings.DoNotDisturb;
        }

        private void OnMenuItemExit_Click(object sender, EventArgs e)
        {
            //_Exit();
            Application.Exit();
        }

        private void OnMenuItem_AboutNotifier_Click(object sender, EventArgs e)
        {
            using (var aboutDialogue = new AboutView())
            {
                aboutDialogue.ShowDialog();
            }
        }

        private void OnMenuItemNotificationPositionTopLeft_Click(object sender, EventArgs e)
        {
            this.appSettings.NotificationDisplayPosition = ToastNotification.DisplayPosition.TopLeft;

        }

        private void OnMenuItemNotificationPositionTopRight_Click(object sender, EventArgs e)
        {
            this.appSettings.NotificationDisplayPosition = ToastNotification.DisplayPosition.TopRight;
        }

        private void OnMenuItemNotificationPositionBottomLeft_Click(object sender, EventArgs e)
        {
            this.appSettings.NotificationDisplayPosition = ToastNotification.DisplayPosition.BottomLeft;
        }

        private void OnMenuItemNotificationPositionBottomRight_Click(object sender, EventArgs e)
        {
            this.appSettings.NotificationDisplayPosition = ToastNotification.DisplayPosition.BottomRight;
        }

#endregion // MenuItem Handler

#region System-Tray Icon

        private void NotificationIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                BringToFront();
            }
        }

#endregion // System-Tray Icon
    }
}
