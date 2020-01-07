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

using ATG_Notifier.ViewModels.Helpers.Extensions;

namespace ATG_Notifier.Desktop.WPF.Controls
{
    public partial class ChapterProfileCard : UserControl
    {
        #region ChapterProfileViewModel

        public static readonly DependencyProperty ChapterProfileViewModelProperty =
            DependencyProperty.Register(nameof(ChapterProfileViewModel), typeof(ChapterProfileViewModel), typeof(ChapterProfileCard), new PropertyMetadata(null));

        public ChapterProfileViewModel ChapterProfileViewModel
        {
            get => (ChapterProfileViewModel)GetValue(ChapterProfileViewModelProperty);
            set => SetValue(ChapterProfileViewModelProperty, value);
        }

        #endregion // ChapterProfileViewModel

        #region DeleteCommand

        public static readonly DependencyProperty DeleteCommandProperty =
            DependencyProperty.Register(nameof(DeleteCommand), typeof(ICommand), typeof(ChapterProfileCard), new PropertyMetadata(null));

        public ICommand DeleteCommand
        {
            get => (ICommand)GetValue(DeleteCommandProperty);
            set => SetValue(DeleteCommandProperty, value);
        }

        #endregion // DeleteCommand

        public ChapterProfileCard()
        {
            InitializeComponent();
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            DeleteCommand?.TryExecute(this.ChapterProfileViewModel);

            e.Handled = true;
        }

        private void OnCloseButtonPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DeleteCommand?.TryExecute(this.ChapterProfileViewModel);

                e.Handled = true;
            }   
        }

        private void OnChapterTitleTextBoxPreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.ChapterNumberAndTitleTextBox.SelectAll();
            e.Handled = true;
        }
    }
}
