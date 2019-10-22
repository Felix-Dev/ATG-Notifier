using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ATG_Notifier.Utilities;
using ATG_Notifier.Controller;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using ATG_Notifier.Model;
using ATG_Notifier.ViewModels.Services;

namespace ATG_Notifier.UI.View
{
    public partial class ToastNotificationView : Form
    {
        private FadeTimer1 fadeTimer;

        private static int DISPLAY_START_POSITION_Y = 10;

        private MouseMoveMessageFilter mouseMessageFilter;

        /* Do not interrupt the user work by stealing the focus when popping up. */
        protected override bool ShowWithoutActivation => true;

        public ToastNotificationView(MenuNotification notification, int slot)
        {
            InitializeComponent();

            slot = (slot < 0) ? 0 : slot;
            Init(slot);

            this.mouseMessageFilter = new MouseMoveMessageFilter
            {
                TargetForm = this
            };
            Application.AddMessageFilter(this.mouseMessageFilter);

            menuNotificationBindingSource.DataSource = notification;
    }

        #region MouseMessageFilterClass

        private class MouseMoveMessageFilter : IMessageFilter
        {
            private enum MouseInput
            {
                WM_NCMOUSEMOVE = 0x00A0,
                WM_MOUSEMOVE = 0x200,

                WM_LBUTTONDOWN = 0x0201,

                WM_NCMOUSELEAVE = 0x02A2,
                WM_MOUSELEAVE = 0x02A3
            };

            private bool mouseInBounds = false;

            private ToastNotificationView targetForm;
            public ToastNotificationView TargetForm 
            {
                get 
                {
                    return targetForm;
                }
                set 
                {
                    targetForm = value;
                }
            }

            public bool PreFilterMessage(ref Message m)
            {
                switch (m.Msg)
                {
                    case (int)MouseInput.WM_MOUSEMOVE:
                    //case (int)MouseInput.WM_NCMOUSEMOVE:
                        CheckMouseBounds(true);
                        break;
                    //case (int)MouseInput.WM_NCMOUSELEAVE:
                    case (int)MouseInput.WM_MOUSELEAVE:
                        CheckMouseBounds(false);
                        break;

                    case (int)MouseInput.WM_LBUTTONDOWN:
                        //targetForm.OnClick();
                        break;

                }
                return false;
            }

            private void CheckMouseBounds(bool mouseMove)
            {
                // Already know the mouse is in the bounds, so we dont
                // care that the mouse just moved (saves unnecessary checks
                // on the form bounds and OnMouseBoundsChanged calls)
                if ((mouseInBounds) && (mouseMove))
                    return;

                // if the cursor is in the bounds of the current form 
                // set the bounds to true, else set the bounds to false
                SetMouseBounds(this.IsInBounds());
            }

            public void SetMouseBounds(bool inBounds)
            {
                // prevent setting the bounds status to the same as it was
                // already set (shouldn't happen anyway, so this is just a
                // sanity check)
                if (mouseInBounds != inBounds)
                {
                    mouseInBounds = inBounds;
                    OnMouseBoundsChanged(EventArgs.Empty);
                }
            }

            private void OnMouseBoundsChanged(EventArgs e)
            {
                if (mouseInBounds == true)
                {
                    TargetForm.fadeTimer.CancelFadeOut();
                }
                else
                {
                    TargetForm.fadeTimer.FadeOutStart();
                }
            }

            public bool IsInBounds()
            {
                Point controlRelPoint = this.TargetForm.PointToClient(Cursor.Position);
                return this.TargetForm.ClientRectangle.Contains(controlRelPoint);
            }
        }

        #endregion

        private delegate Screen del();

        private void Init(int slot)
        {
            this.Load += new EventHandler(this.Notification_Load);
            this.Shown += new EventHandler(this.Notification_Shown);

            int offset = 5; /* space between two consecutive notifications */
            int y = DISPLAY_START_POSITION_Y + slot * (this.Size.Height + offset);

            Screen screen;

            if (Program.MainWindow.InvokeRequired)
            {
                del GetScreen = () => Screen.FromControl(Program.MainWindow);               
                screen = (Screen)Program.MainWindow.Invoke(GetScreen);
            }
            else
            {
                screen = Screen.FromControl(Program.MainWindow);
            }

            screen = screen ?? Screen.PrimaryScreen;

            this.Location = new Point(screen.Bounds.Right - this.Width, y);

            fadeTimer = new FadeTimer1(this, 3500, 1500);
        }

        private void Notification_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
        }

        private void Notification_Shown(object sender, EventArgs e)
        {           
            if (!mouseMessageFilter.IsInBounds())
            {
                fadeTimer.FadeOutStart();
                mouseMessageFilter.SetMouseBounds(false);
            }
            else
            {
                mouseMessageFilter.SetMouseBounds(true);
            }
        }

        private void Notification_MouseClick(object sender, MouseEventArgs e)
        {
            /* 
             * On left-click: Open the obtained chapter URL in the default Internet Browser
             * and close the notification.
             */
            if (e.Button == MouseButtons.Left)
            {
                MenuNotification notification = (MenuNotification)this.DataContext;
                WebService.OpenWebsite(notification.Url);

                this.DialogResult = DialogResult.OK;
            }
        }

        private void ButtonCancel_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                fadeTimer.Stop();
                fadeTimer.Dispose();

                this.DialogResult = DialogResult.OK;
            }
        }

        private void Notification_FormClosing(object sender, FormClosingEventArgs e)
        {
            fadeTimer.Stop();
        }

        private void Notification_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.RemoveMessageFilter(this.mouseMessageFilter);

            fadeTimer.Dispose();
            fadeTimer = null;
        }

        private void TextBox_Chapter_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var textBox = (TextBox)sender;

            textBox.SelectAll();
        }
    }
}
