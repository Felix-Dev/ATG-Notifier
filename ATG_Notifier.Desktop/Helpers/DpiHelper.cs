using ATG_Notifier.Desktop.Native.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Interop;
using System.Windows.Media;

namespace ATG_Notifier.Desktop.Helpers
{
    internal static class DpiHelper
    {
        private const double DefaultDpiValue = 96.0;

        public static double GetDpiScaleForWindow(System.Windows.Window window)
        {
            var dpiScale = VisualTreeHelper.GetDpi(window);
            return dpiScale.DpiScaleX;
        }

        //public static double GetDpiScaleForWindow(System.Windows.Window window)
        //{
        //    var interopHelper = new WindowInteropHelper(window);
        //    IntPtr windowHandle = interopHelper.Handle;

        //    if (windowHandle == IntPtr.Zero)
        //    {
        //        throw new InvalidOperationException("The specified window does now have a window handle!");
        //    }

        //    if (NativeMethodsHelper.DoesWin32MethodExist("user32.dll", "GetDpiForWindow2"))
        //    {
        //        // Windows 10 SDK 1607 and above 

        //        var dpi = NativeMethods.GetDpiForWindow(interopHelper.Handle);
        //        return dpi / DefaultDpiValue;
        //    }
        //    else if (NativeMethodsHelper.DoesWin32MethodExist("shcore.dll", "GetDpiForMonitor2"))
        //    {
        //        // Windows 8.1 and above

        //        IntPtr monitorHandle = NativeMethods.MonitorFromWindow(windowHandle, (int)MonitorOptions.MONITOR_DEFAULT_TO_NEAREST);
        //        NativeMethods.GetDpiForMonitor(monitorHandle, MonitorDpiType.MDT_EFFECTIVE_DPI, out uint dpiX, out uint dpiY);

        //        return dpiX / DefaultDpiValue;
        //    }
        //    else
        //    {
        //        // Basic DPI support for Windows OSs. 
        //        var dpiScale = VisualTreeHelper.GetDpi(window);
        //        return dpiScale.DpiScaleX;
        //    }
        //}
    }
}
