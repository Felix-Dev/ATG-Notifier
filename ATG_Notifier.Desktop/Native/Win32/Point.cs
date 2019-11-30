using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ATG_Notifier.Desktop.Native.Win32
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct POINT
    {
        public int x;
        public int y;

        public POINT(int x, int y) { this.x = x; this.y = y; }
    }
}
