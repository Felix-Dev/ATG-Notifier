using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATG_Notifier.UI;
using ATG_Notifier.Utilities.Extensions;

namespace ATG_Notifier.Utilities
{
    public static class AppFeedbackManager
    {
        public static void FlashApplicationTaskbarButton()
        {
            Program.MainWindow.Invoke(() => WindowFeedbackManager.FlashTaskbarButton(Program.MainWindow));
        }
    }
}
