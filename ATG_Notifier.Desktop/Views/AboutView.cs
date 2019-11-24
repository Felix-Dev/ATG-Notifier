using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.WPF.Controls;
using System.Windows.Forms;

namespace ATG_Notifier.Desktop.Views
{
    internal partial class AboutView : Form
    {
        public AboutView()
        {
            InitializeComponent();

            this.wpfElementHost.Child = new AboutControl(this, AppConfiguration.AppVersion);
        }
    }
}
