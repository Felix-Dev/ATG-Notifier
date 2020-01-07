using ATG_Notifier.Desktop.Native.Win32;
using System;
using System.Windows;
using System.Windows.Interop;

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

        public static IntPtr GetHwnd(Window window)
        {
            return new WindowInteropHelper(window).Handle;
        }

        public static void DisableCloseButton(Window window)
        {
            DisableCloseButton(GetHwnd(window));
        }

        public static void DisableCloseButton(IntPtr hWnd)
        {
            IntPtr menuHandle = NativeMethods.GetSystemMenu(hWnd, false);
            if (menuHandle != IntPtr.Zero)
            {
                NativeMethods.EnableMenuItem(menuHandle, NativeMethods.SC_CLOSE, NativeMethods.MF_BYCOMMAND | NativeMethods.MF_GRAYED);
            }
        }

        public static void HideIcon(Window window)
        {
            HideIcon(new WindowInteropHelper(window).Handle);
        }

        public static void HideIcon(IntPtr hWnd)
        {
            // Change the extended window style to not show a window icon
            int extendedStyle = NativeMethods.GetWindowLong(hWnd, NativeMethods.GWL_EXSTYLE);
            NativeMethods.SetWindowLong(hWnd, NativeMethods.GWL_EXSTYLE, extendedStyle | NativeMethods.WS_EX_DLGMODALFRAME);

            NativeMethods.SendMessage(hWnd, (int)WM.SETICON, NativeMethods.ICON_SMALL, IntPtr.Zero);
            NativeMethods.SendMessage(hWnd, (int)WM.SETICON, NativeMethods.ICON_BIG, IntPtr.Zero);

            // Update the window's non-client area to reflect the changes
            NativeMethods.SetWindowPos(hWnd, IntPtr.Zero, 0, 0, 0, 0,
                NativeMethods.SWP_NOMOVE | NativeMethods.SWP_NOSIZE | NativeMethods.SWP_NOZORDER | NativeMethods.SWP_FRAMECHANGED);
        }

        public static bool SendMessage(string windowTitle, int msgId)
        {
            return SendMessage(windowTitle, msgId, IntPtr.Zero, IntPtr.Zero);
        }

        public static bool SendMessage(string windowTitle, int msgId, IntPtr wParam, IntPtr lParam)
        {
            IntPtr hWnd = NativeMethods.FindWindow(null, windowTitle);
            if (hWnd == IntPtr.Zero)
            {
                return false;
            }

            long result = NativeMethods.SendMessage(hWnd, msgId, wParam, lParam);
            return result == 1;
        }
    }
}
