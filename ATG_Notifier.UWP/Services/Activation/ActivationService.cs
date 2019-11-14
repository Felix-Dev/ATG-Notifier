using ATG_Notifier.UWP.Activation;
using ATG_Notifier.UWP.BackgroundTasks;
using ATG_Notifier.UWP.Configuration;
using ATG_Notifier.UWP.Helpers;
using ATG_Notifier.UWP.Services.Activation;
using ATG_Notifier.UWP.Services.Navigation;
using ATG_Notifier.UWP.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ATG_Notifier.UWP.Services
{
    public class ActivationService
    {
        private readonly App app;
        private readonly Lazy<UIElement> shell;
        private readonly Type defaultNavItem;
        private readonly object argument;

        private readonly IList<ActivationHandler> foregoundActivationHandlers = new List<ActivationHandler>();
        private readonly IList<ActivationHandler> backgroundActivationHandlers = new List<ActivationHandler>();

        private readonly ActivationHandler<IActivatedEventArgs> defaultForegroundHandler;

        public ActivationService(App app, Type defaultNavItem, object argument = null /*Lazy<UIElement> shell = null*/)
        {
            this.app = app;
            this.defaultNavItem = defaultNavItem;
            this.argument = argument;

            this.defaultForegroundHandler = new DefaultLaunchActivationHandler(defaultNavItem, argument);
        }

        public static event EventHandler AppUiLoaded;

        public async Task ActivateAsync(IActivatedEventArgs activationArgs)
        {
            if (!(Window.Current.Content is Frame rootFrame))
            {
                // Register background tasks.
                await InitializeAsync();

                rootFrame = new Frame();

                if (activationArgs.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Load app settings
                //var settingsService = new SettingsService();
                //App.SettingsService = settingsService;
                //App.AppSettings = await settingsService.InitializeSettingsAsync();

                Window.Current.Content = rootFrame;

                await Startup.ConfigureAsync();
            }

            var activationHandler = this.foregoundActivationHandlers.FirstOrDefault(handler => handler.CanHandle(activationArgs));
            if (activationHandler != null)
            {
                await activationHandler.HandleAsync(activationArgs);
            }

            if (this.defaultForegroundHandler.CanHandle(activationArgs))
            {
                await this.defaultForegroundHandler.HandleAsync(activationArgs);
            }

            Window.Current.Activate();
        }

        public async Task ActivateBackgroundAsync(IBackgroundActivatedEventArgs e)
        {
            var deferral = e.TaskInstance.GetDeferral();

            await InitializeAsync();

            ActivationHandler activationHandler = this.backgroundActivationHandlers.FirstOrDefault(handler => handler.CanHandle(e));
            if (activationHandler != null)
            {
                await activationHandler.HandleAsync(e);
            }

            // connection established from the fulltrust process
            else if (e.TaskInstance?.TriggerDetails is AppServiceTriggerDetails d)
            {
                App.AppServiceDeferral = e.TaskInstance.GetDeferral();
                e.TaskInstance.Canceled += OnTaskCanceled;

                if (e.TaskInstance.TriggerDetails is AppServiceTriggerDetails details)
                {
                    App.ServiceConnection = details.AppServiceConnection;
                    App.RaiseAppServiceConnected();
                }
            }

            deferral.Complete();
        }

        private void OnTaskCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            if (App.AppServiceDeferral != null)
            {
                App.AppServiceDeferral.Complete();
            }
        }

        private void RootFrame_Loaded(object sender, RoutedEventArgs e)
        {
            AppUiLoaded?.Invoke(this, EventArgs.Empty);
        }

        private async Task InitializeAsync()
        {
            // Load app settings
            //var settingsService = new SettingsService();
            //App.SettingsService = settingsService;
            //App.AppSettings = await settingsService.InitializeSettingsAsync();

            // Add foreground activation handlers
            if (!this.foregoundActivationHandlers.Any(handler => handler.Name == "ToastNotificationActivationHandler"))
            {
                var toastNotificationActivationHandler = new ToastNotificationActivationHandler()
                {
                    Name = "ToastNotificationActivationHandler",
                };

                this.foregoundActivationHandlers.Add(toastNotificationActivationHandler);
            }

            // Register background tasks
            var toastNotificationBgTask
                = new ToastNotificationActivationBackgroundTask("BackgroundTask.ToastNotificationActivation", new ToastNotificationActionTrigger());

            var backgroundTaskService = Singleton<BackgroundTaskService>.Instance;
            var taskRegistration = backgroundTaskService.RegisterBackgroundTaskAsync(toastNotificationBgTask);

            this.backgroundActivationHandlers.Add(backgroundTaskService);

            // TODO: Enable later again
            //updateService.Start();
        }
    }
}
