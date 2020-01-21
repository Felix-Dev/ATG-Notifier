using ATG_Notifier.Desktop.ViewModels;
using ATG_Notifier.ViewModels.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using ATG_Notifier.ViewModels.Helpers.Extensions;
using ATG_Notifier.Desktop.Helpers;

namespace ATG_Notifier.Desktop.Controls
{
    internal partial class ChapterProfileList : UserControl
    {
        private readonly ScrollViewer chapterListScrollViewer;

        public ChapterProfileList()
        {
            InitializeComponent();

            if (VisualTreeHelperEx.FindDescendantByType<ScrollViewer>(this) is ScrollViewer scrollViewer)
            {
                this.chapterListScrollViewer = scrollViewer;
            }
            else
            {
                throw new InvalidOperationException("Error: ChapterProfileList does not have a ScrollViewer.");
            }
        }          

        public ChapterProfilesListViewModel? ViewModel => this.DataContext as ChapterProfilesListViewModel;

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

        public object? GetDataItem(UIElement element)
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

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (e.Key == Key.Home
                || (e.Key == Key.Up && e.KeyboardDevice.Modifiers == ModifierKeys.Control))
            {
                this.chapterListScrollViewer.ScrollToHome();
                e.Handled = true;
            }
            else if (e.Key == Key.End
                || (e.Key == Key.Down && e.KeyboardDevice.Modifiers == ModifierKeys.Control))
            {
                this.chapterListScrollViewer.ScrollToEnd();
                e.Handled = true;
            }
            else if (e.Key == Key.PageUp)
            {
                this.chapterListScrollViewer.PageUp();
                e.Handled = true;
            }
            else if (e.Key == Key.PageDown)
            {
                this.chapterListScrollViewer.PageDown();
                e.Handled = true;
            }
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
            var item = ItemsControl.ContainerFromElement(this.ChapterList, e.OriginalSource as DependencyObject);
            if (item is ListBoxItem lbItem && lbItem.DataContext is ChapterProfileViewModel chapterProfileViewModel)
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
