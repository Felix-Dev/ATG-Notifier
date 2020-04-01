using System;
using System.Collections.Generic;
using System.Text;

namespace ATG_Notifier.Desktop.Activation
{
    internal class NotificationAreaActivatedEventArgs : EventArgs, IActivatedEventArgs
    {
        public ActivationKind Kind => ActivationKind.NotificationArea;
    }
}
