using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ATG_Notifier.Desktop.Views.Shell
{
    internal abstract class ShellWindow : Window
    {
        public abstract Frame ContentFrame { get; }

        public virtual Task InitializeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
