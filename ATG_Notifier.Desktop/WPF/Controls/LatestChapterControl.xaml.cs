using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using WinForms = System.Windows.Forms;

namespace ATG_Notifier.Desktop.WPF.Controls
{
    /// <summary>
    /// Interaction logic for LatestChapterControl.xaml
    /// </summary>
    public partial class LatestChapterControl : UserControl
    {
        private readonly WinForms.Form ownerForm;

        public LatestChapterControl()
        {
            InitializeComponent();
        }

        public LatestChapterControl(WinForms.Form ownerForm, string text, string release)
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
