using System.Windows;
using System.Windows.Controls;

namespace ATG_Notifier.Desktop.WPF.Controls
{
    public partial class LatestChapterControl : UserControl
    {
        private readonly System.Windows.Forms.Form ownerForm;

        public LatestChapterControl()
        {
            InitializeComponent();
        }

        public LatestChapterControl(System.Windows.Forms.Form ownerForm, string text, string release)
        {
            InitializeComponent();

            this.ownerForm = ownerForm;

            //this.NumberAndTitleTextBlock.Text = text;
            //this.WordCountAndReleaseTimeTextBlock.Text = release;      
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.ownerForm.Close();
        }
    }
}
