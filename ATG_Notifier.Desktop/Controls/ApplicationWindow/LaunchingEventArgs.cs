using System;

namespace ATG_Notifier.Desktop.Controls
{
    internal class LaunchingEventArgs : EventArgs
    {
        public LaunchingEventArgs(ApplicationLaunchMode launchMode)
        {
            this.LaunchMode = launchMode;
        }

        public ApplicationLaunchMode LaunchMode { get; }
    }
}