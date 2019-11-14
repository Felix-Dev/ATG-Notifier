using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace ATG_Notifier.UWP.BackgroundTasks
{
    public abstract class BackgroundTask
    {
        private string name;

        private IBackgroundTrigger trigger;

        private BackgroundTaskCanceledEventHandler canceledEventHandler;

        public BackgroundTask(string name, IBackgroundTrigger trigger)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.Trigger = trigger ?? throw new ArgumentNullException(nameof(trigger));
        }

        public string TaskEntryPoint { get; set; }

        public string Name
        {
            get => name;
            set => name = value ?? throw new ArgumentNullException(nameof(value));
        }

        public IBackgroundTrigger Trigger
        {
            get => trigger;
            set => trigger = value ?? throw new ArgumentNullException(nameof(value));
        }

        public IBackgroundCondition Condition { get; set; }

        public Task RunAsync(IBackgroundTaskInstance taskInstance)
        {
            AttachTaskEventHandlers(taskInstance);

            return RunAsyncInternal(taskInstance);
        }

        protected abstract Task RunAsyncInternal(IBackgroundTaskInstance taskInstance);

        protected abstract void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason);

        protected void AttachTaskEventHandlers(IBackgroundTaskInstance taskInstance)
        {
            this.canceledEventHandler = new BackgroundTaskCanceledEventHandler(OnCanceled);

            taskInstance.Canceled -= this.canceledEventHandler;
            taskInstance.Canceled += this.canceledEventHandler;
        }

        protected void DetachTaskEventHandlers(IBackgroundTaskInstance taskInstance)
        {
            taskInstance.Canceled -= this.canceledEventHandler;
        }
    }
}
