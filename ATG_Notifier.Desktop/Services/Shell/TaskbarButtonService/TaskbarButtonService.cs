using ATG_Notifier.Desktop.Helpers;
using ATG_Notifier.Desktop.Native.Win32;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Shell;

namespace ATG_Notifier.Desktop.Services
{
    internal class TaskbarButtonService
    {
        private IntPtr ownerHandle;
        private Window? ownerWindow;

        public static TaskbarButtonService GetForApp()
        {
            return new TaskbarButtonService(null);
        }

        public static TaskbarButtonService GetForWindow(Window window)
        {
            return new TaskbarButtonService(window);
        }

        private TaskbarButtonService(Window? ownerWindow)
        {
            this.ownerWindow = ownerWindow;
        }

        private Window? OwnerWindow
        {
            get
            {
                if (this.ownerWindow == null)
                {
                    DispatcherHelper.ExecuteOnUIThread(() => this.ownerWindow = Application.Current.MainWindow);              
                }

                return this.ownerWindow;
            }
        }

        private IntPtr OwnerHandle
        {
            get
            {
                if (this.ownerHandle == IntPtr.Zero)
                {
                    var ownerWindow = this.OwnerWindow;
                    if (ownerWindow != null )
                    {
                        DispatcherHelper.ExecuteOnUIThread(() => this.ownerHandle = new WindowInteropHelper(ownerWindow).Handle);
                    }
                }

                return this.ownerHandle;
            }
        }

        /// <summary>
        /// Set the state of the taskbar button.
        /// </summary>
        /// <param name="state">The new state of the taskbar button.</param>
        public void SetButtonState(TaskbarButtonState state)
        {
            DispatcherHelper.ExecuteOnUIThread(() =>
            {
                TaskbarItemInfo taskbarItemInfo = GetTaskbarItemInfoInstance() ?? throw new InvalidOperationException("The window has not yet been initialized!");

                switch (state)
                {
                    case TaskbarButtonState.None:
                        taskbarItemInfo.ProgressValue = 0;
                        taskbarItemInfo.ProgressState = TaskbarItemProgressState.None;
                        break;
                    case TaskbarButtonState.Paused:
                        taskbarItemInfo.ProgressValue = 1;
                        taskbarItemInfo.ProgressState = TaskbarItemProgressState.Paused;
                        break;
                    case TaskbarButtonState.Error:
                        taskbarItemInfo.ProgressValue = 1;
                        taskbarItemInfo.ProgressState = TaskbarItemProgressState.Error;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(state));

                }
            });
        }

        private TaskbarItemInfo? GetTaskbarItemInfoInstance()
        {
            if (this.OwnerWindow is Window window)
            {
                if (window.TaskbarItemInfo == null)
                {
                    window.TaskbarItemInfo = new TaskbarItemInfo();
                }

                return window.TaskbarItemInfo;
            }

            return null;
        }

        /// <summary>
        /// Flash the default taskbar button for the current instance for the specified number of times.
        /// </summary>
        /// <param name="count">The number of times to flash.</param>
        /// <returns>
        /// <c>true</c> if the taskbar button could be flashed successfully; otherwise, <c>false</c>.
        /// </returns>
        public bool FlashButton(int count = 1)
        {
            IntPtr handle = this.OwnerHandle;
            if (handle == IntPtr.Zero)
            {
                return false;
            }

            FLASHWINFO fi = CreateFlashInfoStruct(handle, FlashWindow.FLASHW_TRAY | FlashWindow.FLASHW_TIMERNOFG, (uint)count, 0);

            return NativeMethods.FlashWindowEx(ref fi);
        }

        private FLASHWINFO CreateFlashInfoStruct(IntPtr hWnd, FlashWindow flags, uint count, uint timeout)
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
