using System;

namespace ATG_Notifier.Desktop.Helpers
{
    internal static class CommonHelpers
    {
        public static void RunOnUIThread(Action action)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (Program.MainWindow?.InvokeRequired == true)
            {

                Program.MainWindow.Invoke(action);
            }
            else
            {
                action.Invoke();
            }
        }

        public static T RunOnUIThread<T>(Func<T> action)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (Program.MainWindow?.InvokeRequired == true)
            {

                return (T)Program.MainWindow.Invoke(action);
            }
            else
            {
                return action.Invoke();
            }
        }
    }
}
