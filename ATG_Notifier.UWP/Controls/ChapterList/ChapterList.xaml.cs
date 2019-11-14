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
using ATG_Notifier.UWP.ViewModels;
using ATG_Notifier.ViewModels.ViewModels;

namespace ATG_Notifier.UWP.Controls
{
    public sealed partial class ChapterList : UserControl
    {
        #region ItemTemplate

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(ChapterList), new PropertyMetadata(null));

        #endregion // ItemTemplate

        #region ViewModel

        public ChapterListViewModel ViewModel
        {
            get => (ChapterListViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(ChapterListViewModel), typeof(ChapterList), new PropertyMetadata(null));

        #endregion // ViewModel

        #region Commands

        #region ItemClickCommand

        public ICommand ItemClickCommand
        {
            get => (ICommand)GetValue(ItemClickCommandProperty);
            set => SetValue(ItemClickCommandProperty, value);
        }

        public static readonly DependencyProperty ItemClickCommandProperty =
            DependencyProperty.Register(nameof(ItemClickCommand), typeof(ICommand), typeof(ChapterList), new PropertyMetadata(null));

        #endregion // ItemClickCommand

        #region LostFocusCommand

        public ICommand LostFocusCommand
        {
            get => (ICommand)GetValue(LostFocusCommandProperty);
            set => SetValue(LostFocusCommandProperty, value);
        }

        public static readonly DependencyProperty LostFocusCommandProperty =
            DependencyProperty.Register(nameof(LostFocusCommand), typeof(ICommand), typeof(ChapterList), new PropertyMetadata(null));

        #endregion // LostFocusCommand

        #endregion // Commands

        public ChapterList()
        {
            this.InitializeComponent();
        }

        public object GetFocusedElement()
        {
            return FocusManager.GetFocusedElement() is GridViewItem focusedItem
                ? focusedItem.Content
                : null;
        }

        public void SetFocusOnChapter(long chapterId)
        {
            bool success = this.ChapterGridView.Focus(FocusState.Programmatic);
            if (!success)
            {
                return;
            }

            ChapterProfileViewModel chapterProfile = null;
            if (this.ChapterGridView.SelectedItem is ChapterProfileViewModel selectedProfile
                && selectedProfile.ChapterProfileId == chapterId)
            {
                chapterProfile = selectedProfile;
            }
            else
            {
                chapterProfile = this.ChapterGridView.Items.Cast<ChapterProfileViewModel>()
                                                           .FirstOrDefault(iProfile => iProfile.ChapterProfileId == chapterId);
                if (chapterProfile == null)
                {
                    return;
                }
            }

            this.ChapterGridView.ScrollIntoView(chapterProfile);
            ((GridViewItem)this.ChapterGridView.ContainerFromItem(chapterProfile))?.Focus(FocusState.Keyboard);
        }

        private void ChapterGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ItemClickCommand?.TryExecute(e.ClickedItem);
        }
    }
}
