using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ATG_Notifier.Desktop.Native.Win32
{
    internal class NativeMethods
    {
        #region Window_Feedback

        /// <devdoc>https://msdn.microsoft.com/en-us/library/windows/desktop/ms679347(v=vs.85).aspx</devdoc>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

        #endregion // Window_Feedback

        #region Window Messages

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern int RegisterWindowMessage(string msgName);

        #endregion // WIndow Messages

        #region Window

        [DllImport("user32.dll")]
        internal static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        #endregion // Window

        /// <devdoc>https://msdn.microsoft.com/en-us/library/bb762242(VS.85).aspx</devdoc>
        [DllImport("shell32.dll")]
        internal static extern int SHQueryUserNotificationState(out QUERY_USER_NOTIFICATION_STATE pquns);
    }
}
