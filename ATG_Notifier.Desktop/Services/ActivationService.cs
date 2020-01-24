using ATG_Notifier.Desktop.Activation;
using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.Controls;
using ATG_Notifier.Desktop.Models;
using ATG_Notifier.Desktop.ViewModels;
using ATG_Notifier.Desktop.Views;
using ATG_Notifier.Desktop.Views.Shell;
using ATG_Notifier.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ATG_Notifier.Desktop.Services
{
    internal class ActivationService
    {
        private AppShell? appShell;

        private ILogService logService = null!;
        private INavigationService navigationService = null!;

        private readonly Lazy<ApplicationWindow> shell;
        private readonly ShellArgs shellArgs;

        public ActivationService(Lazy<ApplicationWindow> shell, ShellArgs shellArgs)
        {
            this.shell = shell;
            this.shellArgs = shellArgs;
        }

        public async Task ActivateAsync(IActivatedEventArgs args)
        {
            if (args is LaunchActivatedEventArgs launchArgs)
            {
                await LaunchAsync(launchArgs);
            }
            else if (args is NotificationAreaActivatedEventArgs notificationAreaArgs)
            {
                ActivateShell();
            }
            else if (args is ToastNotificationActivatedEventArgs toastArgs)
            {
                await ToastActivateAsync(toastArgs);
            }
            else
            {
                throw new InvalidOperationException($"ActivationService: Invalid app activation arguments <{args}>!");
            }
        }

        private async Task LaunchAsync(LaunchActivatedEventArgs args)
        {
            // 
            // ---- Below is some technical info about UWP vs WPF differences ---
            // Application.Current.MainWindow == null is our conceptually equivalent to UWP version Window.Current.Content == null.
            // 
            // In UWP, a window already exists and the user only has to create a frame which will hold the window content (such as a page).
            //
            // In our WPF implementation, we don't start with a window. Instead, we have to create a window ourself and then navigate to 
            // the content we want to display.
            //
            if (Application.Current.MainWindow == null)
            {
                // Initialize things like registering background task before the app is loaded
                await InitializeAsync();

                // Initialize the shell and set it as the main window of the app
                ApplicationWindow appWindow = this.shell.Value;
                Application.Current.MainWindow = appWindow;

                // we support a minimized app shell on startup
                bool startMinimized = ShouldStartMinimized(args.Args);

                //
                // The app window can be launched in two different states: 
                //  - Normal: An application window will be shown and a taskbar button exists
                //  - Minimized: No application window will be shown and no taskbar button exists.
                //
                // In the former case, we navigate to our chapter profile page which will load the database content
                // In the latter case, we will defer navigation to the chapter profile page and thus defer loading the 
                // database content until the application window is loaded for the _first_ time.
                //
                // The idea here is that without a UI ready to display the dabatase content (chapter profiles), there is no reason 
                // to load the database content into memory already.
                //

                appWindow.Launching += (s, e) =>
                {
                    if (e.LaunchMode == ApplicationLaunchMode.Normal)
                    {
                        // Navigate to the app shell content since we are about to display the app shell.
                        this.navigationService.Navigate(shellArgs.ViewModel, shellArgs.Parameter);
                    }
                };

                if (startMinimized)
                {
                    appWindow.InitiallyLoaded += OnAppWindowInitiallyLoaded;

                    appWindow.Launch(ApplicationLaunchMode.Minimized);
                }
                else
                {
                    appWindow.Launch(ApplicationLaunchMode.Normal);
                }

                // tasks after activation
                await StartupAsync();
            }
            else
            {
                // The app shell already exists, that means an app instance is already running and the user
                // attempted to start a second instance of this app. Since this app is a single-instance app,
                // instead of launching another notifier process, we attempt to bring the existing instance to the 
                // foreground.

                ActivateShell();
            }
        }

        private void OnAppWindowInitiallyLoaded(object? sender, InitiallyLoadedEventArgs e)
        {
            this.navigationService.Navigate(shellArgs.ViewModel, shellArgs.Parameter);
        }

        private Task ToastActivateAsync(ToastNotificationActivatedEventArgs args)
        {
            var appWindow = (ApplicationWindow)Application.Current.MainWindow;
            if (!appWindow.IsLoaded)
            {
                appWindow.InitiallyLoaded -= OnAppWindowInitiallyLoaded;

                var mainPageArgs = new MainPageArgs(args.ChapterProfileViewModel);
                appWindow.InitiallyLoaded += (s, e) => this.navigationService.Navigate(typeof(MainPage), mainPageArgs);

                ActivateShell();
            }
            else
            {
                ActivateShell();

                ServiceLocator.Current.GetService<ChapterProfilesViewModel>().ListViewModel.SelectedItem = args.ChapterProfileViewModel;
            }

            return Task.CompletedTask;
        }

        private async Task InitializeAsync()
        {
            await ServiceLocator.ConfigureAsync();

            this.logService = ServiceLocator.Current.GetService<ILogService>();
            this.navigationService = ServiceLocator.Current.GetService<INavigationService>();
        }

        private void ActivateShell()
        {
            // When attempting to activate the app shell, it can currently be in one of the following two different states:
            // State 1: Opened in the foreground or the background in which case we simply activate it
            // State 2: Minimized in the taskbar or minimized to the notiifcation area. In the former case, we have to
            //          restore the window. In the latter case we need to additionally make its taskbar icon visible again.
            if (Application.Current.MainWindow.WindowState == WindowState.Minimized)
            {
                if (!Application.Current.MainWindow.IsVisible)
                {
                    Application.Current.MainWindow.Show();
                }

                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
            else
            {
                // ensure current window is active
                Application.Current.MainWindow.Activate();
            }
        }

        private bool ShouldStartMinimized(string[] args)
        {
            var appSettings = ServiceLocator.Current.GetService<AppSettings>();
            return appSettings.IsStartMinimizedEnabled;
        }

        /// <summary>
        /// After app initialization start any required app services.
        /// </summary>
        private Task StartupAsync()
        {
            // start the chapter update listening service
            var chapterUpdateListeningService = ServiceLocator.Current.GetService<ChapterUpdateListeningService>();
            chapterUpdateListeningService.Initialize();

            // start the update service
            var appState = ServiceLocator.Current.GetService<AppState>();
            var updateService = ServiceLocator.Current.GetService<IUpdateService>();

            if (appState.WasUpdateServiceRunning)
            {
                updateService.Start();
            }

            return Task.CompletedTask;
        }
    }
}
