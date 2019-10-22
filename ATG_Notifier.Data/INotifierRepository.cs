using System;
using System.Collections.Generic;
using System.Text;

namespace ATG_Notifier.Data
{
    public interface INotifierRepository
    {
        INotificationRepository Notifications { get; }
    }
}
