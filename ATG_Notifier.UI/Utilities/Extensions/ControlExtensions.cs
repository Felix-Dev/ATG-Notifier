using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATG_Notifier.Utilities.Extensions
{
    public static class ControlExtensions
    {
        public static void Invoke(this Control Control, Action Action)
        {
            Control.Invoke(Action);
        }
    }
}
