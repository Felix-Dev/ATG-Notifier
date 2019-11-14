using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATG_Notifier.UWP.Services.Navigation
{
    public class ShellArguments
    {
        public ShellArguments(Type targetPage, object targetPageArgument = null)
        {
            this.TargetPage = targetPage ?? throw new ArgumentNullException(nameof(targetPage));
            this.TargetPageArgument = targetPageArgument;
        }

        public Type TargetPage { get; set; }

        public object TargetPageArgument { get; set; }
    }
}
