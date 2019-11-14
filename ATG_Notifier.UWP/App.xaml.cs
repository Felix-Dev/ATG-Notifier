using ATG_Notifier.Data;
using ATG_Notifier.UWP.BackgroundTasks;
using ATG_Notifier.UWP.Helpers;
using ATG_Notifier.UWP.Services;
using ATG_Notifier.UWP.Services.Navigation;
using ATG_Notifier.UWP.ViewModels;
using ATG_Notifier.UWP.Views;
using ATG_Notifier.ViewModels.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.ExtendedExecution;
using Windows.ApplicationModel.ExtendedExecution.Foreground;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace ATG_Notifier.UWP
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
        /// <summary>The name of the database.</summary>
        private const string databaseName = "Notifier";

        /// <summary>The version of the database.</summary>
        private const string databaseVersion = "1.10";

        /// <summary>The path to the directory the database file will be written to.</summary>
        private static readonly string databaseDirectory = ApplicationData.Current.LocalFolder.Path;

        /// <summary>The database's file name.</summary>
        private static readonly string fullDatabaseName = $"{databaseName}.{databaseVersion}.db";

        /// <summary>The path to the database file.</summary>
        public static string DatabaseConnectionString { get; } = Path.Combine(databaseDirectory, fullDatabaseName);

        private ActivationService activationService;

        private static App instance;

        public static IDataServiceFactory DataServiceFactory { get; } = new DataServiceFactory();

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();

            App.instance = this;

            activationService = CreateActivationService();

            this.Suspending += OnSuspending;
        }

        public static event EventHandler AppServiceConnected;

        public static BackgroundTaskDeferral AppServiceDeferral { get; set; } = null;

        public static AppServiceConnection ServiceConnection { get; set; } = null;

        public static ILogService LogService { get; } = new LogService();

        public static FrameworkElement RootElement { get; set; }

        public static void RaiseAppServiceConnected()
        {
            AppServiceConnected?.Invoke(App.instance, null);
        }

        private ActivationService CreateActivationService()
        {
            //return new ActivationService(this, typeof(AppShell), null);
            return new ActivationService(this, typeof(AppShell), new ShellArguments(typeof(ChaptersView)));
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            if (!e.PrelaunchActivated)
            {
                await activationService.ActivateAsync(e);
            }
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            await activationService.ActivateAsync(args);
        }

        protected override async void OnBackgroundActivated(BackgroundActivatedEventArgs args)
        {
            await activationService.ActivateBackgroundAsync(args);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: Save application state and stop any background activity

            deferral.Complete();
        }
    }
}
