using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ATG_Notifier.Desktop.WPF.Behaviors
{
    internal class FocusBehavior
    {
        #region LostFocusCommand

        public static DependencyProperty LostFocusCommandProperty = DependencyProperty.RegisterAttached("LostFocusCommand",
               typeof(ICommand),
               typeof(FocusBehavior),
               new FrameworkPropertyMetadata(null, new PropertyChangedCallback(FocusBehavior.LostFocusCommandChanged)));

        public static void SetLostFocusCommand(DependencyObject target, ICommand value)
        {
            target.SetValue(FocusBehavior.LostFocusCommandProperty, value);
        }

        public static ICommand GetLostFocusCommand(DependencyObject target)
        {
            return (ICommand)target.GetValue(FocusBehavior.LostFocusCommandProperty);
        }

        #endregion // LostFocusCommand

        #region LostFocusCommandProperty

        public static DependencyProperty LostFocusCommandParameterProperty = DependencyProperty.RegisterAttached("LostFocusCommandParameter",
               typeof(object),
               typeof(FocusBehavior),
               new FrameworkPropertyMetadata(null));

        public static void SetLostFocusCommandParameter(DependencyObject target, object value)
        {
            target.SetValue(FocusBehavior.LostFocusCommandParameterProperty, value);
        }

        public static object GetLostFocusCommand(DependencyObject target, object value)
        {
            return (object)target.GetValue(FocusBehavior.LostFocusCommandParameterProperty);
        }

        #endregion // LostFocusCommandPropery

        private static void LostFocusCommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (target is Control element)
            {
                if ((e.NewValue != null) && (e.OldValue == null))
                {
                    element.LostFocus += OnElementLostFocus;
                }
                else if ((e.NewValue == null) && (e.OldValue != null))
                {
                    element.LostFocus -= OnElementLostFocus;
                }
            }
        }

        private static void OnElementLostFocus(object sender, RoutedEventArgs e)
        {
            var element = (UIElement)sender;

            ICommand command = (ICommand)element.GetValue(FocusBehavior.LostFocusCommandProperty);
            object cmdParam = (object)element.GetValue(FocusBehavior.LostFocusCommandParameterProperty);

            command.Execute(cmdParam);
        }
    }
}
