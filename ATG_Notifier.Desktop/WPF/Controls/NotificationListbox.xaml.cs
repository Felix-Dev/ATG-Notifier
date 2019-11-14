using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.Controller;
using ATG_Notifier.Desktop.ViewModels;
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

namespace ATG_Notifier.Desktop.WPF.Controls
{
    public partial class NotificationListbox : UserControl
    {
        public NotificationListbox(ChaptersListViewModel viewModel)
        {
            InitializeComponent();

            this.DataContext = viewModel;

            this.HorizontalContentAlignment = HorizontalAlignment.Stretch;
        }
    }
}
