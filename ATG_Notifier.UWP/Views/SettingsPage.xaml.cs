using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ATG_Notifier.UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
        }

        // TODO: Move to SettingsViewModel
        public List<string> ThemeOptions = new List<string>(new string[] { "Light theme", "Dark theme", "Use my Windows mode" });

        private async void AdvancedNotificationSettingsHyperlink_Click(object sender, RoutedEventArgs e)
        {
            bool result = await Launcher.LaunchUriAsync(new Uri("ms-settings:notifications"));
        }
    }
}
