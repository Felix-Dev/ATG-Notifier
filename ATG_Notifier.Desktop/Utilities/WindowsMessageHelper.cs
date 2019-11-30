using ATG_Notifier.Desktop.Native.Win32;
using System;

namespace ATG_Notifier.Desktop.Utilities
{
    internal class WindowsMessageHelper
    {
        public static int WM_EXIT;
        public static readonly int WM_SHOWINSTANCE;

        static WindowsMessageHelper()
        {
            WM_EXIT = NativeMethods.RegisterWindowMessage("WM_EXIT");
            WM_SHOWINSTANCE = NativeMethods.RegisterWindowMessage("WM_SHOWINSTANCE");
        }

        public static void SendMessage(string windowTitle, int msgId)
        {
            SendMessage(windowTitle, msgId, IntPtr.Zero, IntPtr.Zero);
        }

        public static bool SendMessage(string windowTitle, int msgId, IntPtr wParam, IntPtr lParam)
        {
            IntPtr WindowToFind = NativeMethods.FindWindow(null, windowTitle);
            if (WindowToFind == IntPtr.Zero)
            {
                return false;
            }

            long result = NativeMethods.SendMessage(WindowToFind, msgId, wParam, lParam);

            return result == 0;
        }
    }
}
