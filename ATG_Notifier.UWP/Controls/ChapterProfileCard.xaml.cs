using ATG_Notifier.ViewModels.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using ATG_Notifier.ViewModels.Helpers.Extensions;

namespace ATG_Notifier.UWP.Controls
{
    public sealed partial class ChapterProfileCard : UserControl
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

        #region LostFocusCommand

        public static readonly DependencyProperty LostFocusCommandProperty =
            DependencyProperty.Register(nameof(LostFocusCommand), typeof(ICommand), typeof(ChapterProfileCard), new PropertyMetadata(null));

        public ICommand LostFocusCommand
        {
            get => (ICommand)GetValue(LostFocusCommandProperty);
            set => SetValue(LostFocusCommandProperty, value);
        }

        #endregion // LostFocusCommand

        public ChapterProfileCard()
        {
            this.InitializeComponent();
        }

        public static Visibility ShowUnreadIndicator(bool hasRead)
        {
            return hasRead
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        public static string ConvertToDescription(int words, DateTime? time)
        {
            if (words > 0 && time.HasValue)
            {
                return $"{words} · {time.Value.ToString("g")}";
            }
            else if (words > 0)
            {
                return $"{words}";
            }
            else if (time.HasValue)
            {
                return $"{time.Value.ToString("g")}";
            }
            else
            {
                return "";
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteCommand?.TryExecute(this.ChapterProfileViewModel);
        }
    }
}
