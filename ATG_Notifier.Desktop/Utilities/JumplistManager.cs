using Microsoft.WindowsAPICodePack.Taskbar;
using System;
using System.Reflection;

namespace ATG_Notifier.Desktop.Utilities
{
    internal static class JumplistManager
    {
        public const string ACTION_EXIT = "Exit";

        public static void BuildJumplist(string appId, IntPtr windowHandle)
        {
            // Set the application specific id.
            TaskbarManager.Instance.ApplicationId = appId;

            var jumpList = JumpList.CreateJumpListForIndividualWindow(appId, windowHandle);

            var path = Assembly.GetExecutingAssembly().Location;
            path = path.Replace(".dll", ".exe");

            // defining the JumpListLink "Close"
            JumpListLink userActionLink = new JumpListLink(path, ACTION_EXIT)
            {
                Arguments = ACTION_EXIT
            };

            jumpList.AddUserTasks(userActionLink);
            jumpList.Refresh();
        }
    }
}
