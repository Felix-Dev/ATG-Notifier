using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace ATG_Notifier.UWP.Helpers
{
    public static class CommonHelpers
    {
        public static async void RunOnUIThread(Action action)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (CoreApplication.MainView?.Dispatcher != null)
            {
                await CoreApplication.MainView?.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action.Invoke());
            }
        }
    }
}
