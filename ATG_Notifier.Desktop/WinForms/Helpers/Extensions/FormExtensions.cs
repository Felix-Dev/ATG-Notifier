using ATG_Notifier.Desktop.Native.Win32;
using System;
using System.Windows;
using System.Windows.Forms;

namespace ATG_Notifier.Desktop.WinForms.Helpers.Extensions
{
    internal static class FormExtensions
    {
        public static Rect GetScreen(this Form form)
        {
            if (form is null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            IntPtr monitor = NativeMethods.MonitorFromWindow(form.Handle, (int)MonitorOptions.MONITOR_DEFAULT_TO_NEAREST);

            var workSize = new Size(0, 0);
            if (monitor != IntPtr.Zero)
            {
                var monitorInfo = new MONITORINFO();

                NativeMethods.GetMonitorInfo(monitor, monitorInfo);

                RECT rcWorkArea = monitorInfo.rcWork;
                RECT rcMonitorArea = monitorInfo.rcMonitor;

                workSize = new Size(rcWorkArea.Right, rcWorkArea.Bottom);
            }

            return new Rect(workSize);
        }
    }
}
