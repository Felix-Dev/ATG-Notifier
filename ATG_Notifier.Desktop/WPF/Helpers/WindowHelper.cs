using ATG_Notifier.Desktop.Native.Win32;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;

namespace ATG_Notifier.Desktop.WPF.Helpers
{
    internal static class WindowHelper
    {
        public static Rect CurrentWindowBoundRectangle(Window window)
        {
            if (window == null)
            {
                throw new ArgumentNullException(nameof(window));
            }

            var wiHelper = new WindowInteropHelper(window);
            IntPtr handle = wiHelper.Handle;

            IntPtr monitor = NativeMethods.MonitorFromWindow(handle, (int)MONITORINFO.MonitorOptions.MONITOR_DEFAULT_TO_NEAREST);

            Size workSize = new Size(0, 0);
            if (monitor != IntPtr.Zero)
            {

                var monitorInfo = new MONITORINFO();
                NativeMethods.GetMonitorInfo(monitor, monitorInfo);

                RECT rcWorkArea = monitorInfo.rcWork;
                RECT rcMonitorArea = monitorInfo.rcMonitor;

                workSize = new Size(rcMonitorArea.Right, rcMonitorArea.Bottom);
            }

            return new Rect(workSize);
        }

        public static int SetWindowMaximizeArea(IntPtr hWnd, IntPtr lParam, Size maxSize)
        {
            MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));

            /* Adjust the maximized size and position to fit the work area of the correct monitor. */
            IntPtr monitor = NativeMethods.MonitorFromWindow(hWnd, (int)MONITORINFO.MonitorOptions.MONITOR_DEFAULT_TO_NEAREST);

            if (monitor != IntPtr.Zero)
            {
                MONITORINFO monitorInfo = new MONITORINFO();
                NativeMethods.GetMonitorInfo(monitor, monitorInfo);

                RECT workArea = monitorInfo.rcWork;
                RECT monitorArea = monitorInfo.rcMonitor;

                mmi.ptMaxPosition.x = Math.Abs(workArea.Left - monitorArea.Left);
                mmi.ptMaxPosition.y = Math.Abs(workArea.Top - monitorArea.Top);
                mmi.ptMaxSize.x = Math.Max((int)maxSize.Width, Math.Abs(workArea.Right - workArea.Left));
                mmi.ptMaxSize.y = Math.Max((int)maxSize.Height, Math.Abs(workArea.Bottom - workArea.Top));
            }
            Marshal.StructureToPtr(mmi, lParam, true);

            return 0;
        }
    }
}
