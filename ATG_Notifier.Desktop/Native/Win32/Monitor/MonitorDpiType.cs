using System;
using System.Collections.Generic;
using System.Text;

namespace ATG_Notifier.Desktop.Native.Win32
{
    /// <devdoc>https://docs.microsoft.com/en-us/windows/win32/api/shellscalingapi/ne-shellscalingapi-monitor_dpi_type</devdoc>
    internal enum MonitorDpiType
    {
        MDT_EFFECTIVE_DPI = 0,
        MDT_ANGULAR_DPI = 1,
        MDT_RAW_DPI = 2,
    }
}
