using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.Helpers;
using ATG_Notifier.Desktop.Models;
using ATG_Notifier.Desktop.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ATG_Notifier.Desktop.Components
{
    internal class NotificationIcon : IDisposable
    {
        private bool isDisposed = false;

        private NotifyIcon notifyIcon;

        private SettingsViewModel settingsViewModel;

        public NotificationIcon(Icon icon, string tooltipText)
        {
            this.settingsViewModel = ServiceLocator.Current.GetService<SettingsViewModel>();

            var components = new System.ComponentModel.Container();
            this.notifyIcon = new NotifyIcon(components)
            {
                ContextMenuStrip = GetContextMenu(),
                Icon = icon,
                Text = tooltipText,
            };

            this.notifyIcon.MouseUp += OnMouseUp;
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            App.MainView.BringToFront();
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
            if (!this.isDisposed)
            {
                this.notifyIcon.Dispose();
                this.isDisposed = true;
            }
        }

        public void UpdateBadge(int number)
        {
            var iconBitmap = new Bitmap(this.notifyIcon.Icon.Width, this.notifyIcon.Icon.Height);
            Graphics canvas = Graphics.FromImage(iconBitmap);

            canvas.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //canvas.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            canvas.DrawIcon(Properties.Resources.logo_16_ld4_icon, 0, 0);

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

            CommonHelpers.RunOnUIThread(() => this.notifyIcon.Icon = Icon.FromHandle(iconBitmap.GetHicon()));
        }

        private ContextMenuStrip GetContextMenu()
        {
            var contextMenu = new ContextMenuStrip();

            // Sound
            var menuItemSound = new BindableToolStripMenuItem
            {
                Text = "Play Notification Sound"
            };
            menuItemSound.DataBindings.Add(new Binding(nameof(BindableToolStripMenuItem.Checked), this.settingsViewModel, nameof(this.settingsViewModel.IsSoundEnabled), true, DataSourceUpdateMode.OnPropertyChanged));
            menuItemSound.Click += OnMenuItemSoundClick;

            contextMenu.Items.Add(menuItemSound);

            // separator
            var separator = new ToolStripSeparator();

            contextMenu.Items.Add(separator);

            // Focus mode
            var menuItemFocus = new BindableToolStripMenuItem
            {
                Text = "Do Not Disturb"
            };
            menuItemFocus.DataBindings.Add(new Binding(nameof(BindableToolStripMenuItem.Checked), this.settingsViewModel, nameof(this.settingsViewModel.IsInFocusMode), true, DataSourceUpdateMode.OnPropertyChanged));
            menuItemFocus.Click += OnMenuItemFocusClick;

            contextMenu.Items.Add(menuItemFocus);

            // separator
            separator = new ToolStripSeparator();

            contextMenu.Items.Add(separator);

            // Exit
            var menuItemExit = new ToolStripMenuItem
            {
                Text = "Exit"
            };
            menuItemExit.Click += OnMenuItemExitClick;

            contextMenu.Items.Add(menuItemExit);

            return contextMenu;
        }

        private void OnMenuItemExitClick(object? sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void OnMenuItemFocusClick(object? sender, EventArgs e)
        {
            if (sender is BindableToolStripMenuItem menuItem)
            {
                menuItem.Checked = !menuItem.Checked;
            }
        }

        private void OnMenuItemSoundClick(object? sender, EventArgs e)
        {
            if (sender is BindableToolStripMenuItem menuItem)
            {
                menuItem.Checked = !menuItem.Checked;
            }
        }
    }
}
