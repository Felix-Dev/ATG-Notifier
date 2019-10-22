using ATG_Notifier.Controller;
using ATG_Notifier.Model;
using ATG_Notifier.ViewModels.ViewModels;
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

namespace ATG_Notifier.WPF
{
    /// <summary>
    /// Interaction logic for NotificationListbox.xaml
    /// </summary>
    public partial class NotificationListbox : UserControl
    {
        public NotificationListbox(NotificationsController viewModel)
        {

            InitializeComponent();

            this.DataContext = viewModel;

            //this.HorizontalAlignment = HorizontalAlignment.Left;
            this.HorizontalContentAlignment = HorizontalAlignment.Left;
        }

        private void CloseButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var button = (Button)sender;

            button.Command.Execute(null);

            e.Handled = true;
        }

        private void ChapterTitleTextBox_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var textBox = (TextBox)sender;

            textBox.SelectAll();

            e.Handled = true;
        }
    }
}
