using ATG_Notifier.UI.Model;
using ATG_Notifier.Utilities;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATG_Notifier.Controller;
using ATG_Notifier.Utilities.Extensions;
using System.Collections.Generic;
using ATG_Notifier.Model;
using System.IO;
using ATG_Notifier.ViewModels.Infrastructure;
using ATG_Notifier.UI.Utilities;
using ATG_Notifier.ViewModels.Services;
using ATG_Notifier.UI.Views;

namespace ATG_Notifier.UI
{
    public partial class MainWindow : Form
    {
        private const string APP_ID = "ATG-Notifier";

        private readonly object locker = new object();

        // Background worker instance
        private readonly UpdateWorker worker = UpdateWorker.Instance;

        private NotificationsController notificationsController = NotificationsController.Instance;

        private int unreadCounter = 0;

        private int unseenNotifications = 0;

        public int UnseenNotifications
        {
            get => unseenNotifications;
            set
            {
                lock (locker)
                {
                    unseenNotifications = value;
                }
            }
        }

        protected override void WndProc(ref Message msg)
        {
            // if the coming message has the same number as our registered message
            if (msg.Msg == WindowsMessageHelper.TaskCloseArg)
            {
                // terminate the application
                Application.Exit();
            }

            else
            {
                base.WndProc(ref msg);
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            MaximumSize = new Size(Size.Width, Screen.GetWorkingArea(this).Height);
            notifyIcon.Icon = Properties.Resources.logo_16_ld4_icon;
#if DEBUG
            notifyIcon.Text = notifyIcon.Text + " (Debug)";
#endif

            notificationsController.NotificationCollectionChanged += OnNotificationCollectionChanged;

            this.wpfHost.Child = new ATG_Notifier.WPF.NotificationListbox(notificationsController);            

            SetupRecovery();      
        }

        /* Main window event functions */

        private void MainWindow_Load(object sender, EventArgs e)
        {
            /* Load Notifications from database */

            LoadNotifications();

            worker.Start();

            // Debug code below 

            //DebugNotifs();

            //void DebugNotifs()
            //{
            //    new Thread(new ThreadStart(_DebugNotifs)).Start();                
            //}
        }

        private void _DebugNotifs()
        {
            string[] chapUrls = new string[]
            {
                "http://book.zongheng.com/chapter/408586/43329733.html",
                "http://book.zongheng.com/chapter/408586/43331543.html",
                "http://book.zongheng.com/chapter/408586/43335463.html",
                "http://book.zongheng.com/chapter/408586/43337640.html",
                "http://book.zongheng.com/chapter/408586/43341011.html"
            };

            int i = 0;
            foreach (var url in chapUrls)
            {
                Settings settings = Settings.Instance;
                MenuNotification notification = new MenuNotification()
                {
                    Title = "ATG Chapter Update! (DEBUG)",
                    Body = "Chapter " + (1221 + i),
                    Url = url,
                    ArrivalTime = DateTime.Now
                };

                NotificationsController.Instance.Add(notification);

                AppFeedbackManager.FlashApplicationTaskbarButton();

                if (!settings.DoNotDisturb)
                {
                    ToastNotificationManager.Instance.Show(notification);
                }

                i++;

                Thread.Sleep(1 * 10 * 1000);
            }
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            /* Show program icon in Windows notification area. */
            notifyIcon.Visible = true;

            /* create a new taskbar jump list for the main window */
            JumplistManager.BuildJumplist(APP_ID, this.Handle);

            Utility_Autostart.SetAutostart(Autostart.Disable);
        }

        private void SetupRecovery()
        {
            // Start recovery process in case of an unhandled exception

            AppDomain.CurrentDomain.UnhandledException +=
                new UnhandledExceptionEventHandler((s, e) =>
                {
                    // TODO: Only restart app when it was running at least 1 minute(stopwatch)

                    Program.CommonServices.LogService.Log(LogType.Fatal, (e.ExceptionObject as Exception).ToString() + "\r\n" + (e.ExceptionObject as Exception).Message);

                    PerformRecovery();
                    Environment.Exit(1);
                });
        }

        private void PerformRecovery()
        {
            /* Save data. */
            SaveData();

            /*  Start watchdog which restarts the notifier. */
            var asm = Assembly.GetExecutingAssembly();

            var proc = Process.GetCurrentProcess();

            Process wd = new Process();

            wd.StartInfo.FileName = Path.Combine(Path.GetDirectoryName(asm.Location), @"watchdog\wd.exe");
            wd.StartInfo.Arguments = proc.Id + ";" + asm.Location;
            wd.StartInfo.UseShellExecute = false;
            wd.StartInfo.CreateNoWindow = true;

            bool started = wd.Start();
            if (!started)
            {
                Program.CommonServices.LogService.Log(LogType.Error, "ERROR: Attempted to start watchdog but could not start it!");
            }
        }

        //private void RegisterApplicationRecoveryAndRestart()
        //{

        //    // register for Application Restart
        //    //RestartSettings restartSettings =
        //    //    new RestartSettings(string.Empty, RestartRestrictions.None);
        //    //ApplicationRestartRecoveryManager.RegisterForApplicationRestart(restartSettings);

        //    // register for Application Recovery

        //    RecoverySettings recoverySettings =
        //        new RecoverySettings(new RecoveryData(PerformRecovery, null), 5000);
        //    ApplicationRestartRecoveryManager.RegisterForApplicationRecovery(recoverySettings);
        //}

        //private void UnregisterApplicationRecoveryAndRestart()
        //{
        //    //ApplicationRestartRecoveryManager.UnregisterApplicationRestart();
        //    //ApplicationRestartRecoveryManager.UnregisterApplicationRecovery();
        //}

        /// <summary>
        /// Performs recovery by saving the state
        /// </summary>
        /// <param name=”parameter”>Unused.</param>
        /// <returns>Unused.</returns>
        //private int PerformRecovery(object parameter)
        //{
        //    try
        //    {
        //        ApplicationRestartRecoveryManager.ApplicationRecoveryInProgress();

        //        // Save your work here for recovery

        //        Recover();

        //        ApplicationRestartRecoveryManager.ApplicationRecoveryFinished(true);
        //    }
        //    catch
        //    {
        //        ApplicationRestartRecoveryManager.ApplicationRecoveryFinished(false);
        //    }

        //    return 0;
        //}

        private void OnNotificationCollectionChanged(object sender, NotificationCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotificationCollectionChangedAction.Add:
                    OnNotificationAdded(e.Notification);                    
                    break;
                case NotificationCollectionChangedAction.Remove:
                    OnNotificationRemoved(e.Notification);
                    break;
                case NotificationCollectionChangedAction.ReadStatusChange:
                    OnNotificationReadStatusChanged(e.Notification);
                    break;
                default:
                    throw new ArgumentException(nameof(e), $"Error: Invalid operation {e.Action} specified!");
            }

        }

        private void LoadNotifications()
        {
            unreadCounter = notificationsController.LoadNotifications();

            if (unreadCounter == 0)
            {
                this.wpfHost.Visible = false;
                this.labelNoNotifications.Visible = true;
            }

            UpdateBadgeCounter(unreadCounter);           
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.WindowState = FormWindowState.Minimized;
                this.Hide();

                e.Cancel = true;
            }           
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            Exit();
        }

        private void Exit()
        {
            worker.Finish();

            /* Save data. */
            SaveData();

            /* Remove icon from Windows notification area. */
            notifyIcon.Visible = false;
            notifyIcon.Dispose();

            Environment.Exit(0);
            //Application.Exit();
        }

        /// <summary>
        /// Save data (user settings, unread notifications).
        /// </summary>
        private void SaveData()
        {
            notificationsController.SaveNotifications();

            /* Save user settings. */
            Settings.Instance.Save();
        }

        private void UpdateBadgeCounter(int counter)
        {
            DrawNotifyIconBadge(counter);
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

        #endregion

        #region System-Tray Icon

        private void NotificationIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
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
        }

        #endregion // System-Tray Icon

        private void OnNotificationAdded(MenuNotification notification)
        {
            this.Invoke(() =>
            {
                UpdateBadgeCounter(++unreadCounter);

                if (unreadCounter == 1)
                {
                    this.labelNoNotifications.Visible = false;
                    this.wpfHost.Visible = true;
                }
            });
        }

        private void OnNotificationRemoved(MenuNotification notification)
        {
            this.Invoke(() =>
            {
                if (!notification.IsRead)
                {
                    UpdateBadgeCounter(--unreadCounter);
                }               

                if (notificationsController.Notifications.Count == 0)
                {
                    this.wpfHost.Visible = false;
                    this.labelNoNotifications.Visible = true;                    
                }
            });
        }

        private void OnNotificationReadStatusChanged(MenuNotification notification)
        {
            this.Invoke(() =>
            {
                if (notification.IsRead)
                {
                    UpdateBadgeCounter(--unreadCounter);
                }
            });
        }

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

        private void ButtonClearList_MouseClick(object sender, MouseEventArgs e)
        {
            Button btn = (Button)sender;

            btn.FlatAppearance.BorderSize = 1;

            notificationsController.DeleteAllNotifications();

            unreadCounter = 0;

            this.wpfHost.Visible = false;
            this.labelNoNotifications.Visible = true;

            UpdateBadgeCounter(unreadCounter);
        }
    }
}
