using ATG_Notifier.UWP.Activation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.AppService;

namespace ATG_Notifier.UWP.Services.Activation
{
    public class AppServiceActivationHandler : ActivationHandler<BackgroundActivatedEventArgs>
    {
        public void RegisterAppServiceActivationHandler(string name)
        {

        }

        protected override bool CanHandleInternal(BackgroundActivatedEventArgs args)
        {
            return args.TaskInstance?.TriggerDetails is AppServiceTriggerDetails;
        }

        protected override Task HandleInternalAsync(BackgroundActivatedEventArgs args)
        {
            return null;
            //AppServiceDeferral = e.TaskInstance.GetDeferral();
            //args.TaskInstance.Canceled += OnTaskCanceled;

            //if (e.TaskInstance.TriggerDetails is AppServiceTriggerDetails details)
            //{
            //    ServiceConnection = details.AppServiceConnection;
            //    AppServiceConnected?.Invoke(this, null);
            //}
        }
    }
}
