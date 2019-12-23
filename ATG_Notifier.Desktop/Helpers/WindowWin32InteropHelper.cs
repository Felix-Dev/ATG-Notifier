using ATG_Notifier.Desktop.Native.Win32;
using System;

namespace ATG_Notifier.Desktop.Helpers
{
    internal class WindowWin32InteropHelper
    {
        public static int WM_EXIT;
        public static readonly int WM_SHOWINSTANCE;

        static WindowWin32InteropHelper()
        {
            WM_EXIT = NativeMethods.RegisterWindowMessage(nameof(WM_EXIT));
            WM_SHOWINSTANCE = NativeMethods.RegisterWindowMessage(nameof(WM_SHOWINSTANCE));
        }

        public static void DisableCloseButton(IntPtr handle)
        {
            IntPtr menuHandle = NativeMethods.GetSystemMenu(handle, false);
            if (menuHandle != IntPtr.Zero)
            {
                NativeMethods.EnableMenuItem(menuHandle, NativeMethods.SC_CLOSE, NativeMethods.MF_BYCOMMAND | NativeMethods.MF_GRAYED);
            }
        }

        public static void CollapseIcon(IntPtr handle)
        {
            // Change the extended window style to not show a window icon
            int extendedStyle = NativeMethods.GetWindowLong(handle, NativeMethods.GWL_EXSTYLE);
            NativeMethods.SetWindowLong(handle, NativeMethods.GWL_EXSTYLE, extendedStyle | NativeMethods.WS_EX_DLGMODALFRAME);

            NativeMethods.SendMessage(handle, (int)WM.SETICON, NativeMethods.ICON_SMALL, IntPtr.Zero);
            NativeMethods.SendMessage(handle, (int)WM.SETICON, NativeMethods.ICON_BIG, IntPtr.Zero);

            // Update the window's non-client area to reflect the changes
            NativeMethods.SetWindowPos(handle, IntPtr.Zero, 0, 0, 0, 0,
                NativeMethods.SWP_NOMOVE | NativeMethods.SWP_NOSIZE | NativeMethods.SWP_NOZORDER | NativeMethods.SWP_FRAMECHANGED);
        }

        public static bool SendMessage(string windowTitle, int msgId)
        {
            return SendMessage(windowTitle, msgId, IntPtr.Zero, IntPtr.Zero);
        }

        public static bool SendMessage(string windowTitle, int msgId, IntPtr wParam, IntPtr lParam)
        {
            IntPtr handle = NativeMethods.FindWindow(null, windowTitle);
            if (handle == IntPtr.Zero)
            {
                return false;
            }

            long result = NativeMethods.SendMessage(handle, msgId, wParam, lParam);
            return result == 1;
        }
    }
}
