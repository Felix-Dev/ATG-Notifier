using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.Helpers;
using ATG_Notifier.Desktop.Model;
using ATG_Notifier.Desktop.Utilities;
using ATG_Notifier.Desktop.Utilities.Bindings;
using ATG_Notifier.Desktop.ViewModels;
using ATG_Notifier.Desktop.WPF.Controls;
using ATG_Notifier.ViewModels.Services;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ATG_Notifier.Desktop.Views
{
    public partial class MainWindow : Form
    {
        private readonly static ILogService logService = ServiceLocator.Current.GetService<ILogService>();

        private readonly IUpdateService updateService = ServiceLocator.Current.GetService<IUpdateService>();

        private readonly ChaptersListViewModel chaptersListViewModel;

        public MainWindow()
        {
            InitializeComponent();

            this.MaximumSize = new Size(Size.Width, Screen.GetWorkingArea(this).Height);
            this.Text = AppConfiguration.AppId;

            this.notifyIcon.Icon = Properties.Resources.logo_16_ld4_icon;
#if DEBUG
            this.notifyIcon.Text += " (Debug)";
#endif

            this.chaptersListViewModel = ServiceLocator.Current.GetService<ChaptersListViewModel>();
            this.chaptersListViewModel.ChapterProfilesUnreadCountChanged += OnChapterProfilesUnreadCountChanged;

            this.wpfHost.Child = new NotificationListbox(chaptersListViewModel);
            this.wpfHost.Visible = true;

            SetupBindings();    
        }

        private void SetupBindings()
        {
            var binding = new Binding(nameof(BindableMenuItem.Checked), Properties.Settings.Default, nameof(Properties.Settings.NotificationDisplayPosition),
                true,
                DataSourceUpdateMode.OnPropertyChanged);
            binding.Format += Binding_Format;

            this.menuItemNotificationPositionTopLeft.DataBindings.Add(binding);
        }

        protected override void WndProc(ref Message msg)
        {
            // if the coming message has the same number as our registered message
            if (msg.Msg == WindowsMessageHelper.TaskCloseArg)
            {
                // terminate the application
                Application.Exit();
            }
            else if (msg.Msg == WindowsMessageHelper.WM_SHOWINSTANCE)
            {
                BringToFront();
            }
            else
            {
                base.WndProc(ref msg);
            }
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            /* Load Notifications from database */
            await this.chaptersListViewModel.LoadAsync();

            updateService.Start();
        }

        protected override void OnShown(EventArgs e)
        {
            /* Show program icon in Windows notification area. */
            notifyIcon.Visible = true;

            /* create a new taskbar jump list for the main window */
            JumplistManager.BuildJumplist(AppConfiguration.AppId, this.Handle);

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

            base.OnFormClosing(e);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);

            Exit();
        }

        private new void BringToFront()
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.Activate();
            }
        }

        private void Exit()
        {
            this.updateService.Stop();

            this.chaptersListViewModel.ChapterProfilesUnreadCountChanged -= OnChapterProfilesUnreadCountChanged;

            /* Remove icon from Windows notification area. */
            notifyIcon.Visible = false;
            notifyIcon.Dispose();

            //Environment.Exit(0);
            Application.Exit();
        }

        private void OnChapterProfilesUnreadCountChanged(object sender, ChapterProfilesUnreadCountChangedEventArgs e)
        {
            this.UpdateBadgeCounter(e.UnreadCount);
        }

        private void UpdateBadgeCounter(int counter)
        {
            CommonHelpers.RunOnUIThread(() =>
            {
                DrawNotifyIconBadge(counter);
            });

        }

        private void DrawNotifyIconBadge(int counter)
        {
            Graphics canvas;
            Bitmap iconBitmap = new Bitmap(this.notifyIcon.Icon.Width, this.notifyIcon.Icon.Height);
            canvas = Graphics.FromImage(iconBitmap);

            canvas.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //canvas.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            canvas.DrawIcon(Properties.Resources.logo_16_ld4_icon, 0, 0);

            if (counter == 0)
            {
                //this.notifyIcon.Icon = Icon.FromHandle(iconBitmap.GetHicon());
            }
            else
            {
                canvas.FillEllipse(
                    new SolidBrush(Color.DarkRed),
                    new RectangleF(5, 4, this.notifyIcon.Icon.Width - 6, this.notifyIcon.Icon.Width - 6)
                    );

                canvas.DrawEllipse(
                    new Pen(Color.DarkGray),
                    new RectangleF(5, 4, this.notifyIcon.Icon.Width - 6, this.notifyIcon.Icon.Width - 6)
                    );

                if (counter < 9)
                {
                    StringFormat format = new StringFormat();
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;

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
            Settings.Instance.PlayPopupSound = !Settings.Instance.PlayPopupSound;
        }

        private void OnMenuItemTurnOnDisplay_Click(object sender, EventArgs e)
        {
            Settings.Instance.TurnOnDisplay = !Settings.Instance.TurnOnDisplay;
        }

        private void OnMenuItemDisableOnFullscreen_Click(object sender, EventArgs e)
        {
            Settings.Instance.DisableOnFullscreen = !Settings.Instance.DisableOnFullscreen;
        }

        private void OnMenuItemDoNotDisturb_Click(object sender, EventArgs e)
        {
            Settings.Instance.DoNotDisturb = !Settings.Instance.DoNotDisturb;
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
            Settings.Instance.NotificationDisplayPosition = ToastNotification.DisplayPosition.TopLeft;

        }

        private void OnMenuItemNotificationPositionTopRight_Click(object sender, EventArgs e)
        {
            Settings.Instance.NotificationDisplayPosition = ToastNotification.DisplayPosition.TopRight;
        }

        private void OnMenuItemNotificationPositionBottomLeft_Click(object sender, EventArgs e)
        {
            Settings.Instance.NotificationDisplayPosition = ToastNotification.DisplayPosition.BottomLeft;
        }

        private void OnMenuItemNotificationPositionBottomRight_Click(object sender, EventArgs e)
        {
            Settings.Instance.NotificationDisplayPosition = ToastNotification.DisplayPosition.BottomRight;
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

        private void ButtonClearList_MouseEnter(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            //btn.BackColor = Color.LightGray;
            btn.FlatAppearance.BorderColor = Color.Gray;

            btn.ForeColor = Color.Red;
        }

        private void ButtonClearList_MouseLeave(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            //btn.BackColor = Color.White;
            btn.FlatAppearance.BorderColor = Color.LightGray;

            btn.ForeColor = Color.Black;
        }

        private async void ButtonClearList_MouseClick(object sender, MouseEventArgs e)
        {
            Button btn = (Button)sender;

            btn.FlatAppearance.BorderSize = 1;

            await this.chaptersListViewModel.ClearAsync();
        }
    }
}
