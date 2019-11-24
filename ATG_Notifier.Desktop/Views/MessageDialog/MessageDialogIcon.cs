using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ATG_Notifier.Desktop.Views
{
    /// <summary>
    /// Specifies the icon that is displayed by a message box.
    /// </summary>
    internal enum MessageDialogIcon
    {
        /// <summary>No icon is displayed.</summary>
        None = MessageBoxImage.None,
        /// <summary>The message box contains a symbol consisting of white X in a circle with a red background.</summary>
        Error = MessageBoxImage.Error,
        /// <summary>The message box contains a symbol consisting of a question mark in a circle.</summary>
        Question = MessageBoxImage.Question,
        /// <summary>The message box contains a symbol consisting of an exclamation point in a triangle with a yellow background.</summary>
        Warning = MessageBoxImage.Warning,
        /// <summary>The message box contains a symbol consisting of a lowercase letter i in a circle.</summary>
        Information = MessageBoxImage.Information,
    }
}
