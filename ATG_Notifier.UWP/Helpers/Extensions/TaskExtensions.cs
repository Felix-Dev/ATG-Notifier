using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATG_Notifier.UWP.Helpers.Extensions
{
    internal static class TaskExtensions
    {
        public static void FireAndForget(this Task task)
        {
            // This method allows you to call an async method without awaiting it.
            // Use it when you don't want or need to wait for the task to complete.
        }
    }
}
