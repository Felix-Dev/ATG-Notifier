using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATG_Notifier.Desktop.Native.Win32
{
    /// <summary>
    /// Reresents the available window class styles.
    /// </summary>
    /// <devdoc>https://docs.microsoft.com/en-us/windows/win32/winmsg/window-class-styles</devdoc>
    [Flags]
    internal enum WindowClassStyles
    {
        /// <summary>Disables Close on the window menu.</summary>
        CS_NOCLOSE = 0x200,
    }
}
