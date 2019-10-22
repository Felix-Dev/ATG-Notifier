using ATG_Notifier.Native.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ATG_Notifier.Utilities
{
    internal static class FlashWindowHelper
    {
        /// <summary>
        /// Flash the taskbar button of the specified Window until it receives focus.
        /// </summary>
        /// <param name="hWnd">The handle of the Window to flash.</param>
        /// <returns></returns>
        public static bool FlashTaskbarButton(IntPtr hWnd)
        {
            if (hWnd == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(hWnd), "Error: The window handle is invalid!");
            }

            FLASHWINFO fi = CreateFlashInfoStruct(hWnd, FlashWindow.FLASHW_TRAY | FlashWindow.FLASHW_TIMERNOFG, uint.MaxValue, 0);

            return NativeMethods.FlashWindowEx(ref fi);
        }

        /// <summary>
        /// Flash the taskbar button of the specified Window for the specified number of times.
        /// </summary>
        /// <param name="hWnd">The handle of the window to flash.</param>
        /// <param name="count">The number of times to flash.</param>
        /// <returns></returns>
        public static bool FlashTaskbarButton(IntPtr hWnd, uint count)
        {
            if (hWnd == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(hWnd), "Error: The window handle is invalid!");
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Error: The number of times to flash the window cannot be negative.");
            }

            FLASHWINFO fi = CreateFlashInfoStruct(hWnd, FlashWindow.FLASHW_TRAY | FlashWindow.FLASHW_TIMERNOFG, count, 0);

            return NativeMethods.FlashWindowEx(ref fi);
        }

        public static bool StopFlash(IntPtr hWnd)
        {
            if (hWnd == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(hWnd), "Error: The window handle is invalid!");
            }

            FLASHWINFO fi = CreateFlashInfoStruct(hWnd, FlashWindow.FLASHW_STOP, uint.MaxValue, 0);

            return NativeMethods.FlashWindowEx(ref fi);
        }

        private static FLASHWINFO CreateFlashInfoStruct(IntPtr handle, FlashWindow flags, uint count, uint timeout)
        {
            FLASHWINFO fi = new FLASHWINFO();

            fi.cbSize = Convert.ToUInt32(Marshal.SizeOf(fi));
            fi.hwnd = handle;
            fi.dwFlags = (uint)flags;
            fi.uCount = count;
            fi.dwTimeout = timeout;

            return fi;
        }
    }
}
