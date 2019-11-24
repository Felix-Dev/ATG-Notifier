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
    internal class KeyDownBehavior
    {
        #region KeyDownCommand

        public static DependencyProperty KeyDownCommandProperty = DependencyProperty.RegisterAttached(
            "KeyDownCommand",
            typeof(ICommand),
            typeof(KeyDownBehavior),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(KeyDownBehavior.OnKeyDownCommandChanged)));

        public static void SetKeyDownCommand(DependencyObject target, ICommand value)
        {
            target.SetValue(KeyDownCommandProperty, value);
        }

        public static ICommand GetKeyDownCommand(DependencyObject target)
        {
            return (ICommand)target.GetValue(KeyDownCommandProperty);
        }

        #endregion // KeyDownCommand

        #region KeyDownCommandParameter

        public static DependencyProperty KeyDownCommandParameterProperty = DependencyProperty.RegisterAttached(
            "KeyDownCommandParameter",
            typeof(object),
            typeof(KeyDownBehavior),
            new FrameworkPropertyMetadata(null));

        public static void SetKeyDownCommandParameter(DependencyObject target, ICommand value)
        {
            target.SetValue(KeyDownCommandParameterProperty, value);
        }

        public static ICommand GetKeyDownCommandParameter(DependencyObject target)
        {
            return (ICommand)target.GetValue(KeyDownCommandParameterProperty);
        }

        #endregion // KeyDownCommandParameter

        #region Key

        public static DependencyProperty KeyProperty = DependencyProperty.RegisterAttached(
            "Key",
            typeof(Key),
            typeof(KeyDownBehavior),
            new FrameworkPropertyMetadata(null));

        public static void SetKey(DependencyObject target, Key value)
        {
            target.SetValue(KeyProperty, value);
        }

        public static Key GetKey(DependencyObject target)
        {
            return (Key)target.GetValue(KeyProperty);
        }

        #endregion

        private static void OnKeyDownCommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (target is Control element)
            {
                if ((e.NewValue != null) && (e.OldValue == null))
                {
                    element.KeyDown += OnElementKeyDown;
                }
                else if ((e.NewValue == null) && (e.OldValue != null))
                {
                    element.KeyDown -= OnElementKeyDown;
                }
            }
        }

        private static void OnElementKeyDown(object sender, KeyEventArgs e)
        {
            var element = (UIElement)sender;

            if (e.Key == (Key)element.GetValue(KeyProperty))
            {
                ICommand command = (ICommand)element.GetValue(KeyDownCommandProperty);
                object cmdParam = element.GetValue(KeyDownCommandParameterProperty);

                command.Execute(cmdParam);
            }           
        }
    }
}
