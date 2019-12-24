using System;
using System.Runtime.InteropServices;

namespace ATG_Notifier.Desktop.Native.Win32
{
    internal class NativeMethods
    {
        public const int ICON_SMALL = 0,
            ICON_BIG = 1;

        public const int MF_BYCOMMAND = 0x00000000,
            MF_BYPOSITION = 0x00000400,
            MF_GRAYED = 0x00000001,
            MF_ENABLED = 0x00000000,
            MF_DISABLED = 0x00000002;

        public const int SC_CLOSE = 0xF060;

        public const int GWL_STYLE = -16,
            GWL_EXSTYLE = -20;

        public const int WS_EX_DLGMODALFRAME = 0x0001,
            WS_MAXIMIZEBOX = 0x00010000,
            WS_MINIMIZEBOX = 0x00020000,
            WS_SYSMENU = 0x00080000;

        public const int SWP_NOSIZE = 0x0001,
            SWP_NOMOVE = 0x0002,
            SWP_NOZORDER = 0x0004,
            SWP_FRAMECHANGED = 0x0020;



        #region Window 

        /// <devdoc>https://msdn.microsoft.com/en-us/library/windows/desktop/ms679347(v=vs.85).aspx</devdoc>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern int RegisterWindowMessage(string? msgName);

        [DllImport("user32.dll")]
        internal static extern IntPtr FindWindow(string? lpClassName, string? lpWindowName);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetActiveWindow();

        /// <devdoc>https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getdpiforwindow</devdoc>
        [DllImport("user32.dll")]
        internal static extern int GetDpiForWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        internal static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter,
         int x, int y, int width, int height, uint flags);

        [DllImport("user32.dll")]
        internal static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        internal static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

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

        #region Notifications

        /// <devdoc>https://msdn.microsoft.com/en-us/library/bb762242(VS.85).aspx</devdoc>
        [DllImport("shell32.dll")]
        internal static extern int SHQueryUserNotificationState(out QUERY_USER_NOTIFICATION_STATE pquns);

        #endregion // Notifications

        #region Menus

        [DllImport("user32.dll")]
        internal static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        internal static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

        [DllImport("user32.dll")]
        internal static extern IntPtr DestroyMenu(IntPtr hWnd);

        #endregion // Menus
    }
}
