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
    internal class PreviewClickBehavior
    {
        #region ClickCommand

        public static DependencyProperty ClickCommandProperty = DependencyProperty.RegisterAttached("ClickCommand",
               typeof(ICommand),
               typeof(PreviewClickBehavior),
               new FrameworkPropertyMetadata(null, new PropertyChangedCallback(PreviewClickBehavior.OnClickCommandChanged)));

        public static void SetClickCommand(DependencyObject target, ICommand value)
        {
            target.SetValue(PreviewClickBehavior.ClickCommandProperty, value);
        }

        public static ICommand GetClickCommand(DependencyObject target)
        {
            return (ICommand)target.GetValue(ClickCommandProperty);
        }

        #endregion // ClickCommand

        #region ClickCommandParameter

        public static DependencyProperty ClickCommandParameterProperty = DependencyProperty.RegisterAttached("ClickCommandParameter",
               typeof(object),
               typeof(PreviewClickBehavior),
               new FrameworkPropertyMetadata(null));

        public static void SetClickCommandParameter(DependencyObject target, object value)
        {
            target.SetValue(PreviewClickBehavior.ClickCommandParameterProperty, value);
        }

        public static object GetClickCommandParameter(DependencyObject target, object value)
        {
            return target.GetValue(ClickCommandParameterProperty);
        }

        #endregion // ClickCommandParameter

        private static void OnClickCommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            if (target is Control element)
            {
                if ((e.NewValue != null) && (e.OldValue == null))
                {
                    element.MouseUp += OnElementPreviewMouseLeftButtonUp;
                }
                else if ((e.NewValue == null) && (e.OldValue != null))
                {
                    element.MouseUp -= OnElementPreviewMouseLeftButtonUp;
                }
            }
        }

        private static void OnElementPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var element = (UIElement)sender;

                var command = (ICommand)element.GetValue(ClickCommandProperty);
                var cmdParam = element.GetValue(ClickCommandParameterProperty);

                command.Execute(cmdParam);
            }           
        }
    }
}
