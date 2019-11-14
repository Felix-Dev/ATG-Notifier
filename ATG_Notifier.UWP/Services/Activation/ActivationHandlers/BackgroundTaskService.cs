using ATG_Notifier.UWP.Activation;
using ATG_Notifier.UWP.BackgroundTasks;
using ATG_Notifier.UWP.Helpers;
using ATG_Notifier.UWP.Helpers.Extensions;
using ATG_Notifier.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;

namespace ATG_Notifier.UWP.Services
{
    public class BackgroundTaskService : ActivationHandler<BackgroundActivatedEventArgs>
    {
        private readonly IDictionary<string, BackgroundTaskRegistrationInfo> registeredBackgroundTasks = new Dictionary<string, BackgroundTaskRegistrationInfo>();

        public async Task<BackgroundTaskRegistration> RegisterBackgroundTaskAsync(BackgroundTask task)
        {
            if (task is null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            // Check for existing registrations of this background task.
            foreach (var cBackgroundTask in BackgroundTaskRegistration.AllTasks)
            {
                if (cBackgroundTask.Value.Name == task.Name)
                {
                    var taskRegistration1 = (BackgroundTaskRegistration)cBackgroundTask.Value;
                    registeredBackgroundTasks[task.Name] = new BackgroundTaskRegistrationInfo(task, taskRegistration1);
                    return taskRegistration1;
                }
            }

            // See https://docs.microsoft.com/en-us/windows/uwp/launch-resume/respond-to-system-events-with-background-tasks#register-the-background-task
            //
            // TODO: We might be doing too much work here, i.e. whenever we register a task, we remove the app
            // from the list of allowed apps to run background tasks and and request re-permission.
            // The documentation states we only need to call RemoveAccess() after an app update.
            BackgroundExecutionManager.RemoveAccess();
            var result = await BackgroundExecutionManager.RequestAccessAsync();

            if (result == BackgroundAccessStatus.DeniedBySystemPolicy
                || result == BackgroundAccessStatus.DeniedByUser)
            {
                Singleton<LogService>.Instance.Log(LogType.Warning, $"The request to register the background task <{task.Name}> " +
                    $"was not granted. Reason: {result.ToString("g")}");
                return null;
            }

            // Register the background task.
            var builder = new BackgroundTaskBuilder
            {
                Name = task.Name,
            };

            // Check if the background task is an in-process or an out-of-process task.
            if (!string.IsNullOrEmpty(task.TaskEntryPoint))
            {
                // The task is an out-of-process task.
                builder.TaskEntryPoint = task.TaskEntryPoint;
            }

            builder.SetTrigger(task.Trigger);

            if (task.Condition != null)
            {
                builder.AddCondition(task.Condition);

                // If the condition changes while the background task is executing then it will
                // be canceled.
                builder.CancelOnConditionLoss = true;
            }

            BackgroundTaskRegistration taskRegistration = builder.Register();

            registeredBackgroundTasks[taskRegistration.Name] = new BackgroundTaskRegistrationInfo(task, taskRegistration);
            return taskRegistration;
        }

        public void UnregisterBackgroundTask(string taskName)
        {
            if (taskName is null)
            {
                throw new ArgumentNullException(nameof(taskName));
            }

            if (!this.registeredBackgroundTasks.TryGetValue(taskName, out BackgroundTaskRegistrationInfo taskRegistrationInfo))
            {
                return;
            }

            taskRegistrationInfo.TaskRegistration.Unregister(true);

            this.registeredBackgroundTasks.Remove(taskName);
        }

        public void UnregisterBackgroundTasks()
        {
            foreach (var taskRegistrationInfo in registeredBackgroundTasks.Values)
            {
                taskRegistrationInfo.TaskRegistration.Unregister(true);
            }
        }

        protected override bool CanHandleInternal(BackgroundActivatedEventArgs args)
        {
            return registeredBackgroundTasks.ContainsKey(args.TaskInstance?.Task?.Name);
        }

        protected override async Task HandleInternalAsync(BackgroundActivatedEventArgs args)
        {
            // Check the background task to handle has been registered with this service.
            if (!this.registeredBackgroundTasks.TryGetValue(args.TaskInstance?.Task?.Name, out BackgroundTaskRegistrationInfo taskRegistrationInfo))
            {
                // The task has not been registered -> do nothing
                return;
            }

            taskRegistrationInfo.Task
                .RunAsync(args.TaskInstance)
                .FireAndForget();

            await Task.CompletedTask;
        }

        private class BackgroundTaskRegistrationInfo
        {
            public BackgroundTaskRegistrationInfo(BackgroundTask task, BackgroundTaskRegistration taskRegistration)
            {
                this.Task = task ?? throw new ArgumentNullException(nameof(task));
                this.TaskRegistration = taskRegistration ?? throw new ArgumentNullException(nameof(taskRegistration));
            }

            public BackgroundTask Task { get; }

            public BackgroundTaskRegistration TaskRegistration { get; }
        }
    }
}
