using ATG_Notifier.UWP.Configuration;
using ATG_Notifier.ViewModels.Helpers;
using ATG_Notifier.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;

namespace ATG_Notifier.UWP.Utilities
{
    public static class WebUtility
    {
        public static async Task OpenWebsiteAsync(string url)
        {
            if (!UriHelper.TryCreateWebUri(url, out Uri uri))
            {
                throw new ArgumentException(nameof(url), "The specified url is not a valid HTTP URI scheme!");
            }

            // Launch the URI
            bool result = await Launcher.LaunchUriAsync(uri);
            if (!result)
            {
                ServiceLocator.Current.GetService<ILogService>().Log(LogType.Error, $"Failed to open the url {url} in the user's deault web browser!\n");
            }
        }
    }
}
