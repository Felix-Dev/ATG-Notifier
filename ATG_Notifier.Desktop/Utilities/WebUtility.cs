using System;
using System.Diagnostics;

namespace ATG_Notifier.Desktop.Utilities
{
    internal static class WebUtility
    {
        public static void OpenWebsite(string url)
        {
            if (!url.StartsWith("http:") && !url.StartsWith("https:"))
            {
                throw new ArgumentException(nameof(url), "The specified url is not a valid HTTP URI scheme!");
            }

            var psi = new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };

            Process.Start(psi);
        }
    }
}
