using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.Helpers;
using ATG_Notifier.Desktop.Utilities;
using System;
using System.Windows;

namespace ATG_Notifier.Desktop.Views
{
    internal partial class AboutView : Window
    {
        public AboutView()
        {
            InitializeComponent();

            this.Title = $"About {AppConfiguration.AppId}";
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
    }
}
