using ATG_Notifier.Desktop.Native.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATG_Notifier.Desktop.Services.Taskbar
{
    /// <summary>
    /// Provides an API to access the <see cref="ITaskbarList3"/> interface.
    /// </summary>
    internal static class TaskbarList
    {
        private static readonly object syncLock = new object();

        private static ITaskbarList3? taskbarList;

        internal static ITaskbarList3 Instance
        {
            get
            {
                if (taskbarList == null)
                {
                    lock (syncLock)
                    {
                        if (taskbarList == null)
                        {
                            taskbarList = (ITaskbarList3)new CTaskbarList();
                            taskbarList.HrInit();
                        }
                    }
                }

                return taskbarList;
            }
        }
    }
}
