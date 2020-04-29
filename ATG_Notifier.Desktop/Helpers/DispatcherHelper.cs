using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.ViewModels.Services;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ATG_Notifier.Desktop.Helpers
{
    // TODO: Rethink exception handling 
    internal static class DispatcherHelper
    {
        private static readonly ILogService logService;

        static DispatcherHelper()
        {
            logService = ServiceLocator.Current.GetService<ILogService>();
        }

        public static void ExecuteOnUIThread(Action action, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (Application.Current.Dispatcher is Dispatcher dispatcher)
            {
                dispatcher.Invoke(action, priority);
            }
            else
            {
                throw new InvalidOperationException("Application dispatcher does not yet exist");
            }
        }

        public static Task ExecuteOnUIThreadAsync(Action action, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (Application.Current.Dispatcher is Dispatcher dispatcher)
            {
                return dispatcher.AwaitableRunAsync(action, priority);
            }
            else
            {
                throw new InvalidOperationException("Application dispatcher does not yet exist");
            }
        }

        public static T ExecuteOnUIThread<T>(Func<T> action, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (Application.Current.Dispatcher is Dispatcher dispatcher)
            {
                try
                {
                    return dispatcher.Invoke(action, priority);
                }
                catch (TaskCanceledException ex)
                {
                    logService.Log(LogType.Info, $"A task was canceled. Technical details: {ex.Message}");
                    throw ex;
                }
            }
            else
            {
                throw new InvalidOperationException("Application dispatcher does not yet exist");
            }
        }

        public static Task<T> ExecuteOnUIThreadAsync<T>(Func<T> action, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (Application.Current.Dispatcher is Dispatcher dispatcher)
            {
                return dispatcher.AwaitableRunAsync(action, priority);
            }
            else
            {
                throw new InvalidOperationException("Application dispatcher does not yet exist");
            }
        }

        private static Task<T> AwaitableRunAsync<T>(this Dispatcher dispatcher, Func<T> action, DispatcherPriority priority)
        {
            var taskCompletionSource = new TaskCompletionSource<T>();

            var ignored = dispatcher.BeginInvoke(() =>
            {
                try
                {
                    taskCompletionSource.SetResult(action());
                }
                catch (Exception e)
                {
                    taskCompletionSource.SetException(e);
                }

            }, priority);

            return taskCompletionSource.Task;
        }

        /// <summary>
        /// Extension method for Dispatcher. Offering an actual awaitable Task with optional result that will be executed on the given dispatcher
        /// </summary>
        /// <param name="dispatcher">Dispatcher of a thread to run <paramref name="function"/></param>
        /// <param name="function"> Function to be executed asynchrounously on the given dispatcher</param>
        /// <param name="priority">Dispatcher execution priority, default is normal</param>
        /// <returns>Awaitable Task</returns>
        private static Task AwaitableRunAsync(this Dispatcher dispatcher, Action function, DispatcherPriority priority)
        {
            return dispatcher.AwaitableRunAsync(() =>
            {
                    function();
                    return (object)null;
            }, priority);
        }
    }
}
