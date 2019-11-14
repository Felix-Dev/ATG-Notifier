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
    public class ClickBehavior
    {
        public static DependencyProperty DoubleClickCommandProperty = DependencyProperty.RegisterAttached("DoubleClickCommand",
               typeof(ICommand),
               typeof(ClickBehavior),
               new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ClickBehavior.OnDoubleClickCommandChanged)));

        public static DependencyProperty DoubleClickCommandParameterProperty = DependencyProperty.RegisterAttached("DoubleClickCommandParameter",
               typeof(object),
               typeof(ClickBehavior),
               new FrameworkPropertyMetadata(null));

        public static void SetDoubleClickCommand(DependencyObject target, ICommand value)
        {
            target.SetValue(ClickBehavior.DoubleClickCommandProperty, value);
        }

        public static ICommand GetDoubleClickCommand(DependencyObject target)
        {
            return (ICommand)target.GetValue(DoubleClickCommandProperty);
        }

        public static void SetDoubleClickCommandParameter(DependencyObject target, object value)
        {
            target.SetValue(ClickBehavior.DoubleClickCommandParameterProperty, value);
        }

        public static object GetDoubleClickCommand(DependencyObject target, object value)
        {
            return target.GetValue(DoubleClickCommandParameterProperty);
        }

        private static void OnDoubleClickCommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (target is Control element)
            {
                if ((e.NewValue != null) && (e.OldValue == null))
                {
                    element.MouseDoubleClick += Element_OnMouseDoubleClick;
                }
                else if ((e.NewValue == null) && (e.OldValue != null))
                {
                    element.MouseDoubleClick -= Element_OnMouseDoubleClick;
                }
            }
        }

        private static void Element_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var element = (UIElement)sender;

            var command = (ICommand)element.GetValue(DoubleClickCommandProperty);
            var cmdParam = element.GetValue(DoubleClickCommandParameterProperty);

            command.Execute(cmdParam);
        }
    }
}
