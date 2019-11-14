using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ATG_Notifier.UWP.Controls
{
    public static class GridViewItemFocusBehavior
    {
        public static DependencyProperty LostFocusCommandProperty = DependencyProperty.RegisterAttached(
            "LostFocusCommand",
            typeof(ICommand),
            typeof(GridViewItemFocusBehavior),
            new PropertyMetadata(null, new PropertyChangedCallback(GridViewItemFocusBehavior.LostFocusCommandChanged)));

        public static void SetLostFocusCommand(DependencyObject target, ICommand value)
        {
            target.SetValue(GridViewItemFocusBehavior.LostFocusCommandProperty, value);
        }

        public static ICommand GetLostFocusCommand(DependencyObject target)
        {
            return (ICommand)target.GetValue(LostFocusCommandProperty);
        }

        private static void LostFocusCommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (target is GridViewItem item)
            {
                if ((e.NewValue != null) && (e.OldValue == null))
                {
                    item.LostFocus += Element_OnLostFocus;
                }
                else if ((e.NewValue == null) && (e.OldValue != null))
                {
                    item.LostFocus -= Element_OnLostFocus;
                }
            }
        }

        private static void Element_OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is GridViewItem item && item.Content != null)
            {
                var command = (ICommand)item.GetValue(GridViewItemFocusBehavior.LostFocusCommandProperty);
                command.Execute(item.Content);
            }
        }
    }
}
