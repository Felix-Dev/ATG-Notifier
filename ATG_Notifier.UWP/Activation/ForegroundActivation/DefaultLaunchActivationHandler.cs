using ATG_Notifier.UWP.Helpers;
using ATG_Notifier.UWP.Services.Activation;
using ATG_Notifier.UWP.Services.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ATG_Notifier.UWP.Activation
{
    internal class DefaultLaunchActivationHandler : ActivationHandler<IActivatedEventArgs>
    {
        private readonly Type navigationTarget;
        private readonly object argument;

        public DefaultLaunchActivationHandler(Type navigationTarget, object argument = null)
        {
            this.navigationTarget = navigationTarget ?? throw new ArgumentNullException(nameof(navigationTarget));
            this.argument = argument;
        }

        protected override async Task HandleInternalAsync(IActivatedEventArgs args)
        {
            // When the navigation stack isn't restored, navigate to the first page and configure
            // the new page by passing required information in the navigation parameter

            if (Window.Current.Content is Frame frame && frame.Content == null)
            {
                frame.Navigate(this.navigationTarget, this.argument);
            }

            await Task.CompletedTask;
        }

        protected override bool CanHandleInternal(IActivatedEventArgs args)
        {
            // None of the ActivationHandlers has handled the app activation
            return !(Window.Current.Content is Frame frame) || frame.Content == null;
        }
    }
}
