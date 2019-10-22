using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ATG_Notifier.Native.Win32
{
    internal class NativeMethods
    {
        #region Window_Feedback

        /// <devdoc>https://msdn.microsoft.com/en-us/library/windows/desktop/ms679347(v=vs.85).aspx</devdoc>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

        #endregion

        /// <devdoc>https://msdn.microsoft.com/en-us/library/bb762242(VS.85).aspx</devdoc>
        [DllImport("shell32.dll")]
        internal static extern int SHQueryUserNotificationState(out QUERY_USER_NOTIFICATION_STATE pquns);
    }
}
