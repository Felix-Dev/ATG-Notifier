using System;
using System.Windows;
using System.Windows.Controls;

using WinForms = System.Windows.Forms;

namespace ATG_Notifier.Desktop.WPF.Controls
{
    internal partial class AboutControl : UserControl
    {
        private readonly WinForms.Form ownerForm;

        public AboutControl(WinForms.Form ownerForm, string version)
        {
            if (version is null)
            {
                throw new ArgumentNullException("Version cannot be <null>!", nameof(version));
            }

            InitializeComponent();

            this.ownerForm = ownerForm ?? throw new ArgumentNullException("Owner Form cannot be <null>!", nameof(ownerForm));
            this.VersionTextBlock.Text = $"Version: {version}";
        }

        private void OnButtonOKClick(object sender, RoutedEventArgs e)
        {
            this.ownerForm.Close();
        }
    }
}
