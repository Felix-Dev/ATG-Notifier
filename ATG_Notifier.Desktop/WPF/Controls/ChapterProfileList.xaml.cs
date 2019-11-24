using ATG_Notifier.Desktop.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ATG_Notifier.Desktop.WPF.Controls
{
    internal partial class ChapterProfileList : UserControl
    {
        public ChapterProfileList()
        {
            InitializeComponent();
        }

        #region ViewModel

        public ChapterProfilesListViewModel ViewModel
        {
            get => (ChapterProfilesListViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(ChapterProfilesListViewModel), typeof(ChapterProfileList), new PropertyMetadata(null, OnPropertyChanged));

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var profileList = (ChapterProfileList)d;
            if (e.Property == ViewModelProperty)
            {
                if (e.NewValue is ChapterProfilesListViewModel viewModel)
                {
                    profileList.DataContext = viewModel;
                }
            }
        }

        #endregion // ViewModel

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
    }
}
