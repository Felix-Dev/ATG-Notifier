using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ATG_Notifier.Desktop.Native.Win32
{
    /// <devdoc>https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-monitorinfo</devdoc>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
    internal class MONITORINFO
    {
        public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));
        public RECT rcMonitor = new RECT();
        public RECT rcWork = new RECT();
        public int dwFlags = 0;
    }
}
