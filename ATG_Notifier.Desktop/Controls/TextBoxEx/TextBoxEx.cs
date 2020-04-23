using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ATG_Notifier.Desktop.Controls
{
    internal class TextBoxEx : TextBox
    {
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (e.ClickCount == 3)
            {
                this.SelectAll();
            }

            base.OnPreviewMouseLeftButtonDown(e);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            this.SelectionStart = 0;
            this.SelectionLength = 0;

            base.OnLostFocus(e);
        }
    }
}
