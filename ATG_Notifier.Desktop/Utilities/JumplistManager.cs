using Win32Taskbar = Microsoft.WindowsAPICodePack.Taskbar;
using System;
using System.Reflection;
using System.IO;
using System.Windows.Shell;
using System.Windows;

namespace ATG_Notifier.Desktop.Utilities
{
    internal static class JumplistManager
    {
        public const string ActionExit = "Exit";

#if DesktopPackage2
        private static Windows.UI.StartScreen.JumpList appJumpList;

        public static async void BuildJumplistAsync()
        {
            // Get the app's jump list.
            JumplistManager.appJumpList = await Windows.UI.StartScreen.JumpList.LoadCurrentAsync();

            // Disable the system-managed jump list group.
            JumplistManager.appJumpList.SystemGroupKind = Windows.UI.StartScreen.JumpListSystemGroupKind.None;

            // Remove any previously added custom jump list items.
            JumplistManager.appJumpList.Items.Clear();

            var taskItem = Windows.UI.StartScreen.JumpListItem.CreateWithArguments(
                            ACTION_EXIT, "Exit");

            taskItem.Logo = new Uri("ms-appx:///Images/logo_16_ld4_icon.png");

            JumplistManager.appJumpList.Items.Add(taskItem);

            // Save the changes to the app's jump list.
            await JumplistManager.appJumpList.SaveAsync();
        }
#else
        private static Microsoft.WindowsAPICodePack.Taskbar.JumpList appJumpList;

        public static void BuildJumplist(string appId, IntPtr windowHandle)
        {
            // Set the application specific id.
            Win32Taskbar.TaskbarManager.Instance.ApplicationId = appId;

            JumplistManager.appJumpList = Win32Taskbar.JumpList.CreateJumpListForIndividualWindow(appId, windowHandle);

            var path = Assembly.GetExecutingAssembly().Location;

            JumplistManager.appJumpList.ClearAllUserTasks();

            // defining the JumpListLink "Close"
            Microsoft.WindowsAPICodePack.Taskbar.JumpListLink userActionLink = new Win32Taskbar.JumpListLink(path, ActionExit)
            {
                Arguments = ActionExit
            };

            JumplistManager.appJumpList.AddUserTasks(userActionLink);
            JumplistManager.appJumpList.Refresh();
        }
#endif

        public static async void ClearJumplist()
        {
#if DesktopPackage2
            JumplistManager.appJumpList?.Items.Clear();

            await JumplistManager.appJumpList?.SaveAsync();
#else
            JumplistManager.appJumpList?.ClearAllUserTasks();

            JumplistManager.appJumpList.Refresh();
#endif
        }

        public static void BuildJumplistWpf()
        {
            JumpTask task = new JumpTask
            {
                Title = "Exit",
                Arguments = JumplistManager.ActionExit,
                IconResourcePath = Assembly.GetEntryAssembly().CodeBase,
                ApplicationPath = Assembly.GetEntryAssembly().CodeBase.Replace(".dll", ".exe"),
            };

            var jumpList = JumpList.GetJumpList(Application.Current);
            if (jumpList == null)
            {
                jumpList = new JumpList();
            }

            jumpList.JumpItems.Clear();

            jumpList.ShowFrequentCategory = false;
            jumpList.ShowRecentCategory = false;

            jumpList.JumpItems.Add(task);
            
            JumpList.SetJumpList(Application.Current, jumpList);
        }

        public static void ClearJumplistWpf()
        {
            var jumpList = JumpList.GetJumpList(Application.Current);
            if (jumpList != null)
            {
                jumpList.JumpItems.Clear();

                JumpList.SetJumpList(Application.Current, jumpList);
            }
        }
    }
}
