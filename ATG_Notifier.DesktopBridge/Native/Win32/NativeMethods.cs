using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ATG_Notifier.DesktopBridge.Native.Win32
{
    internal class NativeMethods
    {
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern int SetClipboardData(int uFormat, IntPtr hMem);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool CloseClipboard();
    }
}
