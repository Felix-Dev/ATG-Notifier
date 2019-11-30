using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ATG_Notifier.Desktop.Native.Win32
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
    internal class MONITORINFO
    {
        public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));
        public RECT rcMonitor = new RECT();
        public RECT rcWork = new RECT();
        public int dwFlags = 0;

        public enum MonitorOptions : uint
        {
            MONITOR_DEFAULT_TO_NULL = 0x00000000,
            MONITOR_DEFAULT_TO_PRIMARY = 0x00000001,
            MONITOR_DEFAULT_TO_NEAREST = 0x00000002
        }
    }
}
