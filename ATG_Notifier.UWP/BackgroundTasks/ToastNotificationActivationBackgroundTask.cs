using ATG_Notifier.UWP.Helpers;
using ATG_Notifier.UWP.Services;
using ATG_Notifier.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;

namespace ATG_Notifier.UWP.BackgroundTasks
{
    public class ToastNotificationActivationBackgroundTask : BackgroundTask
    {
        private volatile bool isCanceled = false;

        public ToastNotificationActivationBackgroundTask(string name, IBackgroundTrigger trigger)
            : base(name, trigger) { }

        protected override void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            this.isCanceled = true;

            Singleton<LogService>.Instance.Log(LogType.Warning, $"The background task <{sender.Task?.Name}> was cancelled " +
                $"due to the following reason: BackgroundTaskCancellationReason: {reason.ToString("g")}\n");
        }

        protected override Task RunAsyncInternal(IBackgroundTaskInstance taskInstance)
        {
            if (!this.isCanceled)
            {
                Singleton<ToastNotificationService>.Instance.HandleToastActivationAsync((ToastNotificationActionTriggerDetail)taskInstance.TriggerDetails);
            }

            return Task.CompletedTask;
        }
    }
}
