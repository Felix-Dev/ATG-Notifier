using System;
using System.Collections.Generic;
using System.Text;

namespace ATG_Notifier.Desktop.Views
{
    internal class ShellArgs
    {
        public ShellArgs(Type viewModel)
        {
            this.ViewModel = viewModel;
        }

        public Type ViewModel { get; }

        public object? Parameter { get; set; }
    }
}
