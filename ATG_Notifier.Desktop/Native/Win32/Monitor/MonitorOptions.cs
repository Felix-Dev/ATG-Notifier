using System;
using System.Collections.Generic;
using System.Text;

namespace ATG_Notifier.Desktop.Native.Win32
{
    /// <devdoc>https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-monitorfromwindow?redirectedfrom=MSDN#parameters</devdoc>
    internal enum MonitorOptions : uint
    {
        MONITOR_DEFAULT_TO_NULL = 0x00000000,
        MONITOR_DEFAULT_TO_PRIMARY = 0x00000001,
        MONITOR_DEFAULT_TO_NEAREST = 0x00000002
    }
}
