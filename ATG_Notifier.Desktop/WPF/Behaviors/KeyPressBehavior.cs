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
    public class KeyPressBehavior
    {
        public static DependencyProperty KeyPressedCommandProperty = DependencyProperty.RegisterAttached("KeyPressedCommand",
               typeof(ICommand),
               typeof(KeyPressBehavior),
               new FrameworkPropertyMetadata(null, new PropertyChangedCallback(KeyPressBehavior.OnKeyPressCommandChanged)));

        public static DependencyProperty KeyPressedCommandParameterProperty = DependencyProperty.RegisterAttached("KeyPressedCommandParameter",
               typeof(object),
               typeof(KeyPressBehavior),
               new FrameworkPropertyMetadata(null));

        public static DependencyProperty KeyProperty = DependencyProperty.RegisterAttached("Key",
               typeof(Key),
               typeof(KeyPressBehavior),
               new FrameworkPropertyMetadata(null));

        //public static Key Key { get; set; }

        public static void SetKeyPressedCommand(DependencyObject target, ICommand value)
        {
            target.SetValue(KeyPressedCommandProperty, value);
        }

        public static ICommand GetKeyPressedCommand(DependencyObject target)
        {
            return (ICommand)target.GetValue(KeyPressedCommandProperty);
        }

        public static void SetKeyPressedCommandParameter(DependencyObject target, ICommand value)
        {
            target.SetValue(KeyPressedCommandParameterProperty, value);
        }

        public static ICommand GetKeyPressedCommandParameter(DependencyObject target)
        {
            return (ICommand)target.GetValue(KeyPressedCommandParameterProperty);
        }

        public static void SetKey(DependencyObject target, Key value)
        {
            target.SetValue(KeyProperty, value);
        }

        public static Key GetKey(DependencyObject target)
        {
            return (Key)target.GetValue(KeyProperty);
        }

        private static void OnKeyPressCommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (target is Control element)
            {
                if ((e.NewValue != null) && (e.OldValue == null))
                {
                    element.KeyDown += Element_OnKeyDown; ;
                }
                else if ((e.NewValue == null) && (e.OldValue != null))
                {
                    element.KeyDown -= Element_OnKeyDown;
                }
            }
        }

        private static void Element_OnKeyDown(object sender, KeyEventArgs e)
        {
            UIElement element = (UIElement)sender;

            if (e.Key == (Key)element.GetValue(KeyProperty))
            {
                ICommand command = (ICommand)element.GetValue(KeyPressedCommandProperty);
                object cmdParam = (object)element.GetValue(KeyPressedCommandParameterProperty);

                command.Execute(cmdParam);
            }           
        }
    }
}
