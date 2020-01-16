using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.Helpers;
using ATG_Notifier.Desktop.Utilities;
using System;
using System.Windows;
using System.Windows.Input;

namespace ATG_Notifier.Desktop.Views
{
    internal partial class AboutView : Window
    {
        public AboutView()
        {
            InitializeComponent();

            this.Title = $"About {AppConfiguration.AppId}";

            this.AppIdTextBox.Text = AppConfiguration.AppId;
            this.AppVersionTextBox.Text = $"Version {AppConfiguration.AppVersion}";
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            WindowWin32InteropHelper.HideIcon(this);
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnSourceCodeHyperlinkClick(object sender, RoutedEventArgs e)
        {
            WebUtility.OpenWebsite(AppConfiguration.SourceCodeUri);
        }

        private void OnAppIdTextBoxMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.AppIdTextBox.SelectAll();
        }

        private void OnAppVersionTextBoxMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.AppVersionTextBox.SelectAll();
        }

        private void OnAppIdTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            ClearAppIdTextBoxSelection();
        }

        private void OnAppVersionTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            ClearAppVersionTextBoxSelection();
        }

        private void OnGridMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ClearAppIdTextBoxSelection();
            ClearAppVersionTextBoxSelection();
        }

        private void ClearAppIdTextBoxSelection()
        {
            this.AppIdTextBox.SelectionStart = 0;
            this.AppIdTextBox.SelectionLength = 0;
        }

        private void ClearAppVersionTextBoxSelection()
        {
            this.AppVersionTextBox.SelectionStart = 0;
            this.AppVersionTextBox.SelectionLength = 0;
        }
    }
}
