using ATG_Notifier.Desktop.Views.ToastNotification;
using System;
using System.Windows;
using System.Windows.Input;

namespace ATG_Notifier.Desktop.Views
{
    internal partial class ToastNotificationView : Window
    {
        private CloseReason closeReason;

        public ToastNotificationView(string title, string content)
        {
            InitializeComponent();

            this.TitleTextBlock.Text = title;
            this.ChapterNumberAndTitleTextBox.Text = content;
        }

        public new event EventHandler<ToastNotificationClosedEventArgs>? Closed;

        public new CloseReason ShowDialog()
        {
            base.ShowDialog();

            return this.closeReason;
        }

        protected override void OnClosed(EventArgs e)
        {
            Closed?.Invoke(this, new ToastNotificationClosedEventArgs(this.closeReason));
        }

        private void OnButtonCloseClick(object sender, RoutedEventArgs e)
        {
            this.closeReason = CloseReason.Close;
            Close();
        }

        private void OnChapterNumberAndTitleTextBoxMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.ChapterNumberAndTitleTextBox.SelectAll();
            }
        }

        private void OnFadeOutStoryboardCompleted(object sender, EventArgs e)
        {
            if (this.Opacity - 0.1 <= 0)
            {
                this.closeReason = CloseReason.Close;
                Close();
            }
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.closeReason = CloseReason.Click;
            Close();
        }
    }
}
