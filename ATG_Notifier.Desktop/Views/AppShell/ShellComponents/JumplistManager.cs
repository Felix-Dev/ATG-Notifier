using System.Reflection;
using System.Windows;
using System.Windows.Shell;

namespace ATG_Notifier.Desktop.Views.Shell
{
    internal static class JumplistManager
    {
        public static void BuildJumplist()
        {
            JumpTask task = new JumpTask
            {
                Title = "Quit",
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
