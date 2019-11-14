using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATG_Notifier.UWP.Services
{
    public class ToastNotificationActivationCommandsHelper
    {
        public static string BuildActivationCommand(ToastNotificationActivationCommand activationCommand)
        {
            if (activationCommand == null)
            {
                throw new ArgumentNullException(nameof(activationCommand));
            }

            switch (activationCommand.Kind)
            {
                case ToastNotificationActivationCommandKind.None:
                    return $"action={activationCommand.Kind.ConvertToString()}";
                case ToastNotificationActivationCommandKind.CopyToClipboard:
                    return $"action={activationCommand.Kind.ConvertToString()}&{activationCommand.Args}";
                default:
                    throw new NotImplementedException(activationCommand.Kind.ToString());
            }
        }

        public static ToastNotificationActivationCommand ParseActivationCommand(string command)
        {
            if (command == null || !command.StartsWith("action="))
            {
                throw new ArgumentException(nameof(command));
            }

            var delimiters = new char[] { '=', '&' };
            var parts = command.Split(delimiters, 4, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2)
            {
                throw new ArgumentException(nameof(command)); // Illegal command format
            }

            string kindString = parts[1];

            var kind = ToastNotificationActivationCommandKindHelper.ConvertToKind(kindString);
            switch (kind)
            {
                case ToastNotificationActivationCommandKind.None:
                    return new ToastNotificationActivationCommand(kind, null);

                case ToastNotificationActivationCommandKind.CopyToClipboard:
                    if (parts.Length != 3)
                    {
                        throw new ArgumentException(nameof(command));
                    }

                    return new ToastNotificationActivationCommand(kind, parts[2]);
                default:
                    throw new NotImplementedException(kind.ToString());
            }
        }
    }
}
