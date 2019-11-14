using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using ATG_Notifier.Desktop.Native.Win32;

namespace ATG_Notifier.Desktop.Utilities
{
    public class WindowsMessageHelper
    {
        public static int TaskCloseArg;
        public static readonly int WM_SHOWINSTANCE;

        static WindowsMessageHelper()
        {
            TaskCloseArg = NativeMethods.RegisterWindowMessage("ATG-Notifier.Close");
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
