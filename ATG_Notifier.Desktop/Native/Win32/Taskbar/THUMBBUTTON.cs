using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ATG_Notifier.Desktop.Native.Win32
{
    [StructLayout(LayoutKind.Sequential, Pack = 8, CharSet = CharSet.Unicode)]
    internal struct THUMBBUTTON
    {
        public THUMBBUTTONMASK dwMask;
        public uint iId;
        public uint iBitmap;
        public IntPtr hIcon;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 259)]
        public string szTip;

        public THUMBBUTTONFLAGS dwFlags;
    }
}
