using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace ATG_Notifier.Desktop.Native.Win32
{
    internal class NativeMethods
    {
        internal const int ICON_SMALL = 0;
        internal const int ICON_BIG = 1;

        #region Window_Feedback

        /// <devdoc>https://msdn.microsoft.com/en-us/library/windows/desktop/ms679347(v=vs.85).aspx</devdoc>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

        #endregion // Window_Feedback

        #region Window Messages

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern int RegisterWindowMessage(string? msgName);

        #endregion // WIndow Messages

        #region Window

        [DllImport("user32.dll")]
        internal static extern IntPtr FindWindow(string? lpClassName, string? lpWindowName);

        /// <devdoc>https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getdpiforwindow</devdoc>
        [DllImport("user32.dll")]
        internal static extern int GetDpiForWindow(IntPtr hWnd);

        #endregion // Window

        #region Monitor Window

        /// <devdoc>http://msdn.microsoft.com/en-us/library/dd144901%28v=VS.85%29.aspx</devdoc>
        [DllImport("user32", EntryPoint = "GetMonitorInfoW", ExactSpelling = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetMonitorInfo([In] IntPtr hMonitor, [Out] MONITORINFO lpmi);

        /// <devdoc>http://msdn.microsoft.com/en-us/library/dd145064%28v=VS.85%29.aspx</devdoc>
        [DllImport("user32")]
        internal static extern IntPtr MonitorFromWindow([In] IntPtr handle, [In] int flags);

        /// <devdoc>https://docs.microsoft.com/en-us/windows/win32/api/shellscalingapi/nf-shellscalingapi-getdpiformonitor</devdoc>
        [DllImport("shcore.dll")]
        internal static extern uint GetDpiForMonitor(IntPtr hmonitor, MonitorDpiType dpiType, out uint dpiX, out uint dpiY);

        #endregion // Monitor Window

        /// <devdoc>https://msdn.microsoft.com/en-us/library/bb762242(VS.85).aspx</devdoc>
        [DllImport("shell32.dll")]
        internal static extern int SHQueryUserNotificationState(out QUERY_USER_NOTIFICATION_STATE pquns);
    }
}
