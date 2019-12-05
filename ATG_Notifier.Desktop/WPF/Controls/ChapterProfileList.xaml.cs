using ATG_Notifier.Desktop.ViewModels;
using ATG_Notifier.ViewModels.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using ATG_Notifier.ViewModels.Helpers.Extensions;

namespace ATG_Notifier.Desktop.WPF.Controls
{
    internal partial class ChapterProfileList : UserControl
    {
        public ChapterProfileList()
        {
            InitializeComponent();
        }

        public ChapterProfilesListViewModel ViewModel => this.DataContext as ChapterProfilesListViewModel;

        #region ItemClickCommand

        public ICommand ItemClickCommand
        {
            get => (ICommand)GetValue(ItemClickCommandProperty);
            set => SetValue(ItemClickCommandProperty, value);
        }

        public static readonly DependencyProperty ItemClickCommandProperty =
            DependencyProperty.Register(nameof(ItemClickCommand), typeof(ICommand), typeof(ChapterProfileList), new PropertyMetadata(null));

        #endregion // ItemClickCommand

        #region ItemLostFocusCommand

        public ICommand ItemLostFocusCommand
        {
            get => (ICommand)GetValue(ItemLostFocusCommandProperty);
            set => SetValue(ItemLostFocusCommandProperty, value);
        }

        public static readonly DependencyProperty ItemLostFocusCommandProperty =
            DependencyProperty.Register(nameof(ItemLostFocusCommand), typeof(ICommand), typeof(ChapterProfileList), new PropertyMetadata(null));

        #endregion // ItemLostFocusCommand

        public object GetDataItem(UIElement element)
        {
            return element is ListBoxItem listBoxItem 
                ? listBoxItem.DataContext 
                : null;
        }

        public bool IsLastItem(object item)
        {
            int index = this.ChapterList.Items.IndexOf(item);
            if (index == -1)
            {
                throw new ArgumentException(nameof(item));
            }

            return index == this.ChapterList.Items.Count - 1;
        }

        private void OnChapterListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var item = e.AddedItems[0];

                this.ChapterList.ScrollIntoView(item);

                if (this.ChapterList.ItemContainerGenerator.ContainerFromItem(item) is UIElement element 
                    && !element.IsFocused)
                {
                    element.Focus();
                }
            }
        }

        private void OnChapterListKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && e.OriginalSource is ListBoxItem item)
            {
                this.ItemClickCommand.TryExecute(item.DataContext as ChapterProfileViewModel);
            }
        }

        private void OnChapterListMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (this.ChapterList.SelectedItem is ChapterProfileViewModel chapterProfileViewModel)
            {
                this.ItemClickCommand.TryExecute(chapterProfileViewModel);
            }
        }

        private void OnChapterProfileLostFocus(object sender, RoutedEventArgs e)
        {
            var item = (ListBoxItem)sender;

            this.ItemLostFocusCommand.TryExecute(item.DataContext as ChapterProfileViewModel);
        }
    }
}
