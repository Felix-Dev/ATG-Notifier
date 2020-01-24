using System;
using System.Collections.Generic;
using System.Text;

namespace ATG_Notifier.Desktop.Activation
{
    internal class LaunchActivatedEventArgs : IActivatedEventArgs
    {
        public LaunchActivatedEventArgs(params string[] args)
        {
            this.Args = args;
        }

        public ActivationKind Kind => ActivationKind.Launch;

        public string[] Args { get; }
    }
}
