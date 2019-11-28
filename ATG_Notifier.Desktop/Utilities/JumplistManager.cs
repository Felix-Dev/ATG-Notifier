using Win32Taskbar = Microsoft.WindowsAPICodePack.Taskbar;
using UWPTaskbar = Windows.UI.StartScreen;
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
            Win32Taskbar.TaskbarManager.Instance.ApplicationId = appId;

            var jumpList = Win32Taskbar.JumpList.CreateJumpListForIndividualWindow(appId, windowHandle);

            var path = Assembly.GetExecutingAssembly().Location;
            path = path.Replace(".dll", ".exe");

            // defining the JumpListLink "Close"
            Win32Taskbar.JumpListLink userActionLink = new Win32Taskbar.JumpListLink(path, ACTION_EXIT)
            {
                Arguments = ACTION_EXIT
            };

            jumpList.AddUserTasks(userActionLink);
            jumpList.Refresh();
        }

        public static async void BuildJumplistAsync()
        {
            // Get the app's jump list.
            var jumpList = await UWPTaskbar.JumpList.LoadCurrentAsync();

            // Disable the system-managed jump list group.
            jumpList.SystemGroupKind = UWPTaskbar.JumpListSystemGroupKind.None;

            // Remove any previously added custom jump list items.
            jumpList.Items.Clear();

            // Save the changes to the app's jump list.
            await jumpList.SaveAsync();

            var taskItem = UWPTaskbar.JumpListItem.CreateWithArguments(
                            ACTION_EXIT, "Exit");

            taskItem.Logo = new Uri("ms-appx:///Images/logo_16_ld4_icon.png");

            jumpList.Items.Add(taskItem);

            // Save the changes to the app's jump list.
            await jumpList.SaveAsync();
        }
    }
}
