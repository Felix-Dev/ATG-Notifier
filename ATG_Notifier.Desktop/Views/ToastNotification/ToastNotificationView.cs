using ATG_Notifier.Desktop.Utilities;
using ATG_Notifier.Desktop.Utilities.UI;
using ATG_Notifier.Desktop.Views.ToastNotification;
using ATG_Notifier.ViewModels.ViewModels;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ATG_Notifier.Desktop.View
{
    internal partial class ToastNotificationView : Form
    {
        private const int DISPLAY_START_POSITION_Y = 10;

        /// <summary>Space between two consecutive notifications.</summary>
        private const int INTER_NOTIFICATION_OFFSET_Y = 5;

        private readonly MouseMoveMessageFilter mouseMessageFilter;

        private FadeTimer fadeTimer;

        private UserInteraction userInteraction = UserInteraction.None;

        // Do not interrupt the user work by stealing the focus when popping up.
        protected override bool ShowWithoutActivation => true;

        public ToastNotificationView(ChapterProfileViewModel chapterProfileVM, string title, int slot, DisplayPosition position)
        {
            if (chapterProfileVM is null)
            {
                throw new ArgumentNullException(nameof(chapterProfileVM));
            }

            if (!Enum.IsDefined(typeof(DisplayPosition), position))
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            InitializeComponent();

            this.labelTitle.Text = title ?? "";

            slot = (slot < 0) ? 0 : slot;
            Init(slot, position);

            this.mouseMessageFilter = new MouseMoveMessageFilter
            {
                TargetForm = this
            };

            Application.AddMessageFilter(this.mouseMessageFilter);

            menuNotificationBindingSource.DataSource = chapterProfileVM;
        }

        public new UserInteraction ShowDialog()
        {
            base.ShowDialog();
            return this.userInteraction;
        }

        private delegate Screen del();

        private void Init(int slot, DisplayPosition position)
        {
            SetupBindings();

            this.Location = GetDisplayPosition(slot, position);

            this.fadeTimer = new FadeTimer(this, 3500, 1500);
        }

        private void SetupBindings()
        {
            this.ChapterNumberAndTitleTextBox.DataBindings.Add(
                new Binding(
                    "Text", 
                    this.menuNotificationBindingSource, 
                    nameof(ChapterProfileViewModel.NumberAndTitleDisplayString)));
        }

        private Point GetDisplayPosition(int slot, DisplayPosition position)
        {
            Screen? appScreen = null;
            //if (Program.MainWindow.InvokeRequired)
            //{
            //    del GetScreen = () => Screen.FromControl(Program.MainWindow);
            //    appScreen = (Screen)Program.MainWindow.Invoke(GetScreen);
            //}
            //else
            //{
            //    appScreen = Screen.FromControl(Program.MainWindow);
            //}

            Screen screen = appScreen ?? Screen.PrimaryScreen;

            int x = 0, y = 0;
            switch (position)
            {
                case DisplayPosition.TopLeft:
                    y = DISPLAY_START_POSITION_Y + slot * (this.Size.Height + INTER_NOTIFICATION_OFFSET_Y);
                    break;
                case DisplayPosition.TopRight:
                    x = screen.Bounds.Right - this.Width;
                    y = DISPLAY_START_POSITION_Y + slot * (this.Size.Height + INTER_NOTIFICATION_OFFSET_Y);
                    break;
                case DisplayPosition.BottomLeft:
                    y = screen.Bounds.Bottom - DISPLAY_START_POSITION_Y - this.Size.Height - slot * (this.Size.Height + INTER_NOTIFICATION_OFFSET_Y);
                    break;
                case DisplayPosition.BottomRight:
                    x = screen.Bounds.Right - this.Width;
                    y = screen.Bounds.Bottom - DISPLAY_START_POSITION_Y - this.Size.Height - slot * (this.Size.Height + INTER_NOTIFICATION_OFFSET_Y);
                    break;
            }

            return new Point(x, y);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Resize the chapter number and title textbox based on the width of its actual content.
            Size size = TextRenderer.MeasureText(this.ChapterNumberAndTitleTextBox.Text, this.ChapterNumberAndTitleTextBox.Font);
            this.ChapterNumberAndTitleTextBox.Width = size.Width;

            this.TopMost = true;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

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

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            fadeTimer.Stop();

            base.OnFormClosing(e);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            Application.RemoveMessageFilter(this.mouseMessageFilter);

            fadeTimer.Dispose();

            base.OnFormClosed(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            // On left-click: Set the user interaction as 'click'.
            if (e.Button == MouseButtons.Left)
            {
                this.userInteraction = UserInteraction.Click;
                this.DialogResult = DialogResult.OK;
            }

            base.OnMouseClick(e);
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            // On left-click: Set the user interaction as 'click'.
            if (e.Button == MouseButtons.Left)
            {
                this.userInteraction = UserInteraction.Click;
                this.DialogResult = DialogResult.OK;
            }
        }

        private void OnCancelButtonMouseClick(object sender, MouseEventArgs e)
        {
            // On left-click: Set the user interaction as 'close'.
            if (e.Button == MouseButtons.Left)
            {
                this.userInteraction = UserInteraction.Close;
                this.DialogResult = DialogResult.OK;
            }
        }

        private void OnChapterNumberAndTitleTextBoxMouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.ChapterNumberAndTitleTextBox.SelectAll();
        }

        #region MouseMessageFilter

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

        #endregion // MouseMessageFilter
    }
}
