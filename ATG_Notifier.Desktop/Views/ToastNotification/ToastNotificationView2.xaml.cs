using ATG_Notifier.Desktop.Views.ToastNotification;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ATG_Notifier.Desktop.Views
{
    internal partial class ToastNotificationView2 : Window
    {
        private CloseReason closeReason;

        public ToastNotificationView2(string title, string content)
        {
            InitializeComponent();

            this.TitleTextBlock.Text = title;
            this.ChapterNumberAndTitleTextBox.Text = content;
        }

        public new event EventHandler<ToastNotificationClosedEventArgs> Closed;

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
