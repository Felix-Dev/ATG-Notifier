using System;
using System.Collections.Generic;
using System.Text;

namespace ATG_Notifier.Desktop.Activation
{
    internal class NotificationAreaActivatedEventArgs : IActivatedEventArgs
    {
        public ActivationKind Kind => ActivationKind.NotificationArea;
    }
}
