using ATG_Notifier.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATG_Notifier.ViewModels.Infrastructure
{
    public class NotificationCollectionChangedEventArgs : EventArgs
    {
        public NotificationCollectionChangedAction Action { get; private set; }

        public MenuNotification Notification { get; private set; }

        public NotificationCollectionChangedEventArgs(NotificationCollectionChangedAction action, MenuNotification notification)
        {
            Action = action;
            Notification = notification;
        }
    }
}
