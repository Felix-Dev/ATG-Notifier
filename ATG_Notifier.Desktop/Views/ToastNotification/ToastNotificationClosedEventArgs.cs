using System;
using System.Collections.Generic;
using System.Text;

namespace ATG_Notifier.Desktop.Views.ToastNotification
{
    internal class ToastNotificationClosedEventArgs : EventArgs
    {
        public ToastNotificationClosedEventArgs(CloseReason reason)
        {
            this.Reason = reason;
        }

        public CloseReason Reason { get; }
    }
}
