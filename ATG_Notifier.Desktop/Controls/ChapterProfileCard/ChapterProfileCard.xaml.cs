﻿using ATG_Notifier.ViewModels.Helpers.Extensions;
using ATG_Notifier.ViewModels.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ATG_Notifier.Desktop.Controls
{
    public partial class ChapterProfileCard : UserControl
    {
        public ChapterProfileCard()
        {
            InitializeComponent();
        }

        #region ChapterProfileViewModelProperty

        public static readonly DependencyProperty ChapterProfileViewModelProperty =
            DependencyProperty.Register(nameof(ChapterProfileViewModel), typeof(ChapterProfileViewModel), typeof(ChapterProfileCard), new PropertyMetadata(null));

        public ChapterProfileViewModel ChapterProfileViewModel
        {
            get => (ChapterProfileViewModel)GetValue(ChapterProfileViewModelProperty);
            set => SetValue(ChapterProfileViewModelProperty, value);
        }

        #endregion // ChapterProfileViewModelProperty

        #region DeleteCommandProperty

        public static readonly DependencyProperty DeleteCommandProperty =
            DependencyProperty.Register(nameof(DeleteCommand), typeof(ICommand), typeof(ChapterProfileCard), new PropertyMetadata(null));

        public ICommand DeleteCommand
        {
            get => (ICommand)GetValue(DeleteCommandProperty);
            set => SetValue(DeleteCommandProperty, value);
        }

        #endregion // DeleteCommandProperty

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