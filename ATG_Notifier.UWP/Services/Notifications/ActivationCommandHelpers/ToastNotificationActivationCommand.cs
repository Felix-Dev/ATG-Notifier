using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATG_Notifier.UWP.Services
{
    public class ToastNotificationActivationCommand
    {
        public ToastNotificationActivationCommand(ToastNotificationActivationCommandKind kind, object args)
        {
            this.Kind = kind;
            this.Args = args;
        }

        public ToastNotificationActivationCommandKind Kind { get; }

        public object Args { get; }
    }
}
