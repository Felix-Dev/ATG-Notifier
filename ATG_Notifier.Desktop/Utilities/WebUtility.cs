using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATG_Notifier.Desktop.Utilities
{
    public static class WebUtility
    {
        public static void OpenWebsite(string url)
        {
            if (url is null || (!url.StartsWith("http:") && !url.StartsWith("https:")))
            {
                throw new ArgumentException(nameof(url), "The specified url is not a valid HTTP URI scheme!");
            }

            Process.Start(url);
        }
    }
}
