using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATG_Notifier.Desktop.Utilities
{
    public static class WindowFeedbackManager
    {
        public static void FlashTaskbarButton(Form window)
        {
            FlashTaskbarButton(window, 1);
        }

        public static void FlashTaskbarButton(Form window, int count)
        {
            if (window == null)
            {
                throw new ArgumentNullException(nameof(window), "Error: Illegal window!");
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Error: count cannot be a negative number!");
            }

            var hWnd = window.Handle;
            FlashWindowHelper.FlashTaskbarButton(hWnd, (uint)count);

        }

        public static void StopFlashWindow(Form window)
        {
            if (window == null)
            {
                throw new ArgumentNullException(nameof(window), "Error: Illegal window!");
            }

            var hWnd = window.Handle;
            FlashWindowHelper.StopFlash(hWnd);
        }
    }
}
