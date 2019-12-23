using ATG_Notifier.Desktop.Helpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Shell;

namespace ATG_Notifier.Desktop.Components
{
    internal static class JumplistManager
    {
        public static void BuildJumplist()
        {
            JumpTask task = new JumpTask
            {
                Title = "Exit",
                Arguments = App.AppExitCmd,
                IconResourcePath = Assembly.GetEntryAssembly()?.CodeBase ?? "",
                ApplicationPath = Assembly.GetEntryAssembly()?.CodeBase?.Replace(".dll", ".exe") ?? "",
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

        public static void ClearJumplist()
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
