using ATG_Notifier.Desktop.Native.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ATG_Notifier.Desktop.Views
{
    /// <summary>
    /// Interaction logic for AboutView2.xaml
    /// </summary>
    internal partial class AboutView2 : Window
    {
        public AboutView2(string version)
        {
            if (version is null)
            {
                throw new ArgumentNullException(nameof(version));
            }

            InitializeComponent();

            this.VersionTextBlock.Text = $"Version: {version}";
        }

        private void OnButtonOKClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
