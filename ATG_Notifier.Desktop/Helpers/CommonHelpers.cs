using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATG_Notifier.Desktop.Helpers
{
    public static class CommonHelpers
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
    }
}
