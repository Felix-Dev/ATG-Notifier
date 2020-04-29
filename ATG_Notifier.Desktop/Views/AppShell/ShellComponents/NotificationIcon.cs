using ATG_Notifier.Desktop.Activation;
using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.Helpers;
using ATG_Notifier.Desktop.ViewModels;
using ATG_Notifier.Desktop.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ATG_Notifier.Desktop.Views.Shell
{
    internal class NotificationIcon : IDisposable
    {
        private bool isDisposed = false;

        private readonly Icon baseIcon;
        private NotifyIcon notifyIcon;

        private readonly SettingsViewModel settingsViewModel;  

        public NotificationIcon(Icon icon, string tooltipText)
        {
            this.settingsViewModel = ServiceLocator.Current.GetService<SettingsViewModel>();

            this.baseIcon = icon;

            var components = new System.ComponentModel.Container();
            this.notifyIcon = new NotifyIcon(components)
            {
                ContextMenuStrip = GetContextMenu(),
                Icon = icon,
                Text = tooltipText,
            };

            this.notifyIcon.MouseUp += OnMouseUp;
        }

        public void Show()
        {
            this.notifyIcon.Visible = true;
        }

        public void Hide()
        {
            this.notifyIcon.Visible = false;
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        public void UpdateBadge(int number)
        {
            var iconBitmap = new Bitmap(this.notifyIcon.Icon.Width, this.notifyIcon.Icon.Height);
            Graphics canvas = Graphics.FromImage(iconBitmap);

            canvas.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //canvas.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            canvas.DrawIcon(this.baseIcon, 0, 0);

            // draw badge if number is positive
            if (number > 0)
            {
                canvas.FillEllipse(
                    new SolidBrush(Color.DarkRed),
                    new RectangleF(5, 4, this.notifyIcon.Icon.Width - 6, this.notifyIcon.Icon.Width - 6)
                    );

                canvas.DrawEllipse(
                    new Pen(Color.DarkGray),
                    new RectangleF(5, 4, this.notifyIcon.Icon.Width - 6, this.notifyIcon.Icon.Width - 6)
                    );

                if (number <= 9)
                {
                    StringFormat format = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };

                    canvas.DrawString(
                        number.ToString(),
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

            DispatcherHelper.ExecuteOnUIThread(() => this.notifyIcon.Icon = Icon.FromHandle(iconBitmap.GetHicon()));
        }

        protected void Dispose(bool disposing)
        {
            if (this.isDisposed)
            {
                return;
            }

            if (disposing)
            {
                this.notifyIcon.Dispose();
                this.notifyIcon = null!;
            }

            this.isDisposed = true;
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ActivateApp();
            }
        }

        private ContextMenuStrip GetContextMenu()
        {
            var contextMenu = new ContextMenuStrip();

            // Open app
            var menuItemOpen = new ToolStripMenuItem()
            {
                Text = "Open",
            };
            menuItemOpen.Click += (s, e) => ActivateApp();

            contextMenu.Items.Add(menuItemOpen);

            // separator
            var separator = new ToolStripSeparator();

            contextMenu.Items.Add(separator);

            // Sound
            var menuItemSound = new BindableToolStripMenuItem
            {
                Text = "Play Notification Sound",
                CheckOnClick = true,
            };
            menuItemSound.DataBindings.Add(new Binding(nameof(BindableToolStripMenuItem.Checked), this.settingsViewModel, nameof(this.settingsViewModel.IsSoundEnabled), true, DataSourceUpdateMode.OnPropertyChanged));

            contextMenu.Items.Add(menuItemSound);

            // Focus mode
            var menuItemFocus = new BindableToolStripMenuItem
            {
                Text = "Do Not Disturb",
                CheckOnClick = true,
            };
            menuItemFocus.DataBindings.Add(new Binding(nameof(BindableToolStripMenuItem.Checked), this.settingsViewModel, nameof(this.settingsViewModel.IsFocusModeEnabled), true, DataSourceUpdateMode.OnPropertyChanged));

            contextMenu.Items.Add(menuItemFocus);

            // separator
            separator = new ToolStripSeparator();

            contextMenu.Items.Add(separator);

            // Exit
            var menuItemExit = new ToolStripMenuItem
            {
                Text = "Quit"
            };

            // TODO: 
            // We are using a Hotfix here: Previously, we called Application.Current.Shutdown()
            // but this call failed to terminate the app correctly when the app was started minimized 
            // and then brought to the foreground. The app windows would close and cleanup would be performed
            // yet the app process would still be running (visible in TaskManager).
            //
            // As a Hotfix, we are now using a slightly indirect App shutdown request call (i.e. we are sending
            // a custom shutdown message to the app window). This works as intended in the above case.
            menuItemExit.Click += (s, e) => App.RequestCloseRunningInstance();

            contextMenu.Items.Add(menuItemExit);

            return contextMenu;
        }

        private void ActivateApp()
        {
            // TODO: Add SaveFireAndForget task handler?
            App.Current.ActivateAsync(new NotificationAreaActivatedEventArgs());
        }
    }
}
