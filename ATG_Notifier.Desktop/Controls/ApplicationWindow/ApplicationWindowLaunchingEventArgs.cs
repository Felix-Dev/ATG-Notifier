using System;
using System.Collections.Generic;
using System.Text;

namespace ATG_Notifier.Desktop.Controls
{
    internal class ApplicationWindowLaunchingEventArgs : EventArgs
    {
        public ApplicationWindowLaunchingEventArgs(ApplicationLaunchMode launchMode)
        {
            this.LaunchMode = launchMode;
        }

        public ApplicationLaunchMode LaunchMode { get; }
    }
}
