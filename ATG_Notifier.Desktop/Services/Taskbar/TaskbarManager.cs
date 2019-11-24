using ATG_Notifier.Desktop.Native.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ATG_Notifier.Desktop.Services.Taskbar
{
    internal class TaskbarManager
    {
        private IntPtr ownerHandle;

        #region Creation

        private static readonly Lazy<TaskbarManager> lazy = new Lazy<TaskbarManager>(() => new TaskbarManager());

        public static TaskbarManager Current => lazy.Value;

        #endregion

        /// <summary>
        /// Sets the handle of the window whose taskbar button will be used
        /// to display progress.
        /// </summary>
        private IntPtr OwnerHandle
        {
            get
            {
                if (ownerHandle == IntPtr.Zero)
                {
                    var currentProcess = Process.GetCurrentProcess();
                    if (currentProcess?.MainWindowHandle == IntPtr.Zero)
                    {
                        return IntPtr.Zero;
                    }

                    ownerHandle = currentProcess.MainWindowHandle;
                }

                return ownerHandle;
            }
        }

        /// <summary>
        /// Displays or updates a progress bar hosted in the default taskbar button for the current instance
        /// to show the specific percentage completed of the full operation.
        /// </summary>
        /// <param name="currentValue">An application-defined value that indicates the proportion of the operation that has been completed at the time the method is called.</param>
        /// <param name="maximumValue">An application-defined value that specifies the value currentValue will have when the operation is complete.</param>
        /// <returns>
        /// <c>true</c> if the progress value could be set successfully; otherwise, <c>false</c>.
        /// </returns>
        public bool SetProgressValue(int currentValue, int maximumValue)
        {
            return TaskbarList.Instance
                .SetProgressValue(OwnerHandle, Convert.ToUInt32(currentValue), Convert.ToUInt32(maximumValue))
                == 0;
        }

        /// <summary>
        /// Displays or updates a progress bar hosted in a taskbar button of the given window handle 
        /// to show the specific percentage completed of the full operation.
        /// </summary>
        /// <param name="hWnd">The handle of the window whose associated taskbar button is being used as a progress indicator.
        /// This window belong to a calling process associated with the button's application and must be already loaded.</param>
        /// <param name="currentValue">An application-defined value that indicates the proportion of the operation that has been completed at the time the method is called.</param>
        /// <param name="maximumValue">An application-defined value that specifies the value currentValue will have when the operation is complete.</param>
        /// <returns>
        /// <c>true</c> if the progress value could be set successfully; otherwise, <c>false</c>.
        /// </returns>
        public static bool SetProgressValue(IntPtr hWnd, int currentValue, int maximumValue)
        {
            return TaskbarList.Instance
                .SetProgressValue(hWnd, Convert.ToUInt32(currentValue), Convert.ToUInt32(maximumValue))
                == 0;
        }

        /// <summary>
        /// Sets the type and state of the progress indicator displayed on default taskbar button for the current instance.
        /// </summary>
        /// <param name="state">Progress state of the progress button</param>
        /// <returns>
        /// <c>true</c> if the progress state could be set successfully; otherwise, <c>false</c>.
        /// </returns>
        public bool SetProgressState(TaskbarProgressBarState state)
        {
            return TaskbarList.Instance
                .SetProgressState(ownerHandle, (TBPFLAG)state) == 0;
        }

        /// <summary>
        /// Sets the type and state of the progress indicator displayed on a taskbar button 
        /// of the given window handle.
        /// </summary>
        /// <param name="hWnd">The handle of the window whose associated taskbar button is being used as a progress indicator.
        /// This window belong to a calling process associated with the button's application and must be already loaded.</param>
        /// <param name="state">Progress state of the progress button</param>
        /// <returns>
        /// <c>true</c> if the progress state could be set successfully; otherwise, <c>false</c>.
        /// </returns>
        public static bool SetProgressState(IntPtr hWnd, TaskbarProgressBarState state)
        {
            return TaskbarList.Instance
                .SetProgressState(hWnd, (TBPFLAG)state) == 0;
        }

        /// <summary>
        /// Sets the default taskbar button for the current instance in an error state (i.e. filled red background).
        /// </summary>
        /// <returns>
        /// <c>true</c> if the taskbar button was set in the error state successfully; otherwise, <c>false</c>.
        /// </returns>
        public bool SetErrorTaskbarButton()
        {
            return SetProgressValue(1, 1)
                && SetProgressState(TaskbarProgressBarState.Error);

        }

        /// <summary>
        /// Sets the taskbar button of the given window handle in an error state (i.e. filled red background).
        /// </summary>
        /// <param name="hWnd">The handle of the window to which the changed taskbar button belongs.</param>
        /// <returns>
        /// <c>true</c> if the taskbar button was set in the error state successfully; otherwise, <c>false</c>.
        /// </returns>
        public static bool SetErrorTaskbarButton(IntPtr hWnd)
        {
            return SetProgressValue(hWnd, 1, 1)
                && SetProgressState(hWnd, TaskbarProgressBarState.Error);

        }

        /// <summary>
        /// Unsets the default taskbar button for the current instance from an error state (i.e. remove the filled red background).
        /// </summary>
        /// <returns>
        /// <c>true</c> if the taskbar button was unset from an error state successfully; otherwise, <c>false</c>.
        /// </returns>
        public bool ClearErrorTaskbarButton()
        {
            return SetProgressValue(0, 1)
                && SetProgressState(TaskbarProgressBarState.NoProgress);
        }

        /// <summary>
        /// Unsets the taskbar button of the given window handle from an error state (i.e. remove the filled red background).
        /// </summary>
        /// <param name="hWnd">The handle of the window to which the changed taskbar button belongs.</param>
        /// <returns>
        /// <c>true</c> if the taskbar button was unset from an error state successfully; otherwise, <c>false</c>.
        /// </returns>
        public static bool ClearErrorTaskbarButton(IntPtr hWnd)
        {
            return SetProgressValue(hWnd, 0, 1)
                && SetProgressState(hWnd, TaskbarProgressBarState.NoProgress);
        }

        /// <summary>
        /// Flash the default taskbar button for the current instance for the specified number of times.
        /// </summary>
        /// <param name="count">The number of times to flash.</param>
        /// <returns>
        /// <c>true</c> if the taskbar button could be flashed successfully; otherwise, <c>false</c>.
        /// </returns>
        public bool FlashTaskbarButton(int count = 1)
        {
            FLASHWINFO fi = CreateFlashInfoStruct(this.OwnerHandle, FlashWindow.FLASHW_TRAY | FlashWindow.FLASHW_TIMERNOFG, (uint)count, 0);

            return NativeMethods.FlashWindowEx(ref fi);
        }

        /// <summary>
        /// Flash the taskbar button of the specified Window for the specified number of times.
        /// </summary>
        /// <param name="hWnd">The handle of the window to flash.</param>
        /// <param name="count">The number of times to flash.</param>
        /// <returns>
        /// <c>true</c> if the taskbar button could be flashed successfully; otherwise, <c>false</c>.
        /// </returns>
        public static bool FlashTaskbarButton(IntPtr hWnd, int count = 1)
        {
            FLASHWINFO fi = CreateFlashInfoStruct(hWnd, FlashWindow.FLASHW_TRAY | FlashWindow.FLASHW_TIMERNOFG, (uint)count, 0);

            return NativeMethods.FlashWindowEx(ref fi);
        }

        private static FLASHWINFO CreateFlashInfoStruct(IntPtr hWnd, FlashWindow flags, uint count, uint timeout)
        {
            var fi = new FLASHWINFO();

            fi.cbSize = Convert.ToUInt32(Marshal.SizeOf(fi));
            fi.hwnd = hWnd;
            fi.dwFlags = (uint)flags;
            fi.uCount = count;
            fi.dwTimeout = timeout;

            return fi;
        }
    }
}
