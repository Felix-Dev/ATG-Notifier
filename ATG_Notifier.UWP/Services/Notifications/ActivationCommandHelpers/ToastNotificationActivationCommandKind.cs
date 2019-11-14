using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATG_Notifier.UWP.Services
{
    public enum ToastNotificationActivationCommandKind
    {
        None = 0,
        CopyToClipboard = 1,

        // TODO: Remove?
        OpenWebsite = 2,
    }

    public static class ToastNotificationActivationCommandKindHelper
    {
        private const string CMD_STR_NONE = "none";
        private const string CMD_STR_COPY_TO_CLIPBOARD = "copyToClipboard";
        private const string CMD_STR_OPEN_WEBSITE = "openWebsite";

        public static string ConvertToString(this ToastNotificationActivationCommandKind kind)
        {
            switch (kind)
            {
                case ToastNotificationActivationCommandKind.None:
                    return CMD_STR_NONE;
                case ToastNotificationActivationCommandKind.CopyToClipboard:
                    return CMD_STR_COPY_TO_CLIPBOARD;
                case ToastNotificationActivationCommandKind.OpenWebsite:
                    return CMD_STR_OPEN_WEBSITE;
                default:
                    throw new NotImplementedException(kind.ToString());
            }
        }

        public static ToastNotificationActivationCommandKind ConvertToKind(string command)
        {
            string kind = command.Split('&', StringSplitOptions.RemoveEmptyEntries)[0];
            switch (kind)
            {
                case CMD_STR_NONE:
                    return ToastNotificationActivationCommandKind.None;
                case CMD_STR_COPY_TO_CLIPBOARD:
                    return ToastNotificationActivationCommandKind.CopyToClipboard;
                case CMD_STR_OPEN_WEBSITE:
                    return ToastNotificationActivationCommandKind.OpenWebsite;
                default:
                    throw new NotImplementedException(command);
            }
        }
    }
}
