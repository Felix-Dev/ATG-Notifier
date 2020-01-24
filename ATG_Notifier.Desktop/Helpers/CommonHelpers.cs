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
    internal static class CommonHelpers
    {
        private static readonly ILogService logService;

        //public static void RunOnUIThread(Action action)
        //{
        //    if (action is null)
        //    {
        //        throw new ArgumentNullException(nameof(action));
        //    }

        //    if (Program.MainWindow?.InvokeRequired == true)
        //    {

        //        Program.MainWindow.Invoke(action);
        //    }
        //    else
        //    {
        //        action.Invoke();
        //    }
        //}

        //public static T RunOnUIThread<T>(Func<T> action)
        //{
        //    if (action is null)
        //    {
        //        throw new ArgumentNullException(nameof(action));
        //    }

        //    if (Program.MainWindow?.InvokeRequired == true)
        //    {

        //        return (T)Program.MainWindow.Invoke(action);
        //    }
        //    else
        //    {
        //        return action.Invoke();
        //    }
        //}

        static CommonHelpers()
        {
            logService = ServiceLocator.Current.GetService<ILogService>();
        }

        public static void RunOnUIThread(Action action, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            try
            {
                // TODO: Can throw TaskCanceledException - once testcode is in place, test with super fast debug update service
                Application.Current.Dispatcher.Invoke(action, priority);
            }
            catch (TaskCanceledException)
            {

            }
        }

        public static T RunOnUIThread<T>(Func<T> action, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (Application.Current.Dispatcher is Dispatcher dispatcher)
            {
                try
                {
                    return Application.Current.Dispatcher.Invoke(action, priority);
                }
                catch (TaskCanceledException ex)
                {
                    logService.Log(LogType.Info, $"A task was canceled. Technical details: {ex.Message}");
                    return default;
                }
            }
            else
            {
                throw new InvalidOperationException("Error: Application/MainWindow object is <null>!");
            }
        }
    }
}
