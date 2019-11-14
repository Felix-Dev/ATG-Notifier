using Microsoft.WindowsAPICodePack.Taskbar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ATG_Notifier.Desktop.Utilities
{
    public static class JumplistManager
    {
        public const string ACTION_EXIT = "Exit";

        public static void BuildJumplist(string appId, IntPtr windowHandle)
        {
            // Set the application specific id.
            TaskbarManager.Instance.ApplicationId = appId;

            var jumpList = JumpList.CreateJumpListForIndividualWindow(appId, windowHandle);

            // defining the JumpListLink "Close"
            JumpListLink userActionLink = new JumpListLink(Assembly.GetEntryAssembly().Location, ACTION_EXIT)
            {
                Arguments = ACTION_EXIT
            };

            jumpList.AddUserTasks(userActionLink);
            jumpList.Refresh();
        }
    }
}
