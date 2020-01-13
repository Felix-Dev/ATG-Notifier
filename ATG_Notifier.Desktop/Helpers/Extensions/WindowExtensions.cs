using ATG_Notifier.Desktop.Native.Win32;
using System;
using System.Windows;
using System.Windows.Interop;

namespace ATG_Notifier.Desktop.Helpers.Extensions
{
    internal static class WindowExtensions
    {
        public static IntPtr GetHandle(this Window window)
        {
            return new WindowInteropHelper(window).Handle;
        }

        public static Rect? GetScreenBounds(this Window window)
        {
            IntPtr monitor = NativeMethods.MonitorFromWindow(window.GetHandle(), (int)MonitorOptions.MONITOR_DEFAULT_TO_NEAREST);
            if (monitor == IntPtr.Zero)
            {
                return null;
            }

            var monitorInfo = new MONITORINFO();

            NativeMethods.GetMonitorInfo(monitor, monitorInfo);

            RECT rcWorkArea = monitorInfo.rcWork;
            RECT rcMonitorArea = monitorInfo.rcMonitor;

            return new Rect(0, 0, rcWorkArea.Right, rcWorkArea.Bottom);
        }
    }
}
