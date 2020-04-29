using ATG_Notifier.Desktop.Activation;
using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.Controls;
using ATG_Notifier.Desktop.Helpers;
using ATG_Notifier.Desktop.Models;
using ATG_Notifier.Desktop.Native.Win32;
using ATG_Notifier.Desktop.Services;
using ATG_Notifier.Desktop.Views;
using ATG_Notifier.Desktop.Views.Shell;
using ATG_Notifier.ViewModels.Services;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

#if DesktopPackage || DesktopPackageDebug
using Windows.ApplicationModel;
#endif

namespace ATG_Notifier.Desktop
{
    internal partial class App : Application
    {
        private static string applicationWindowName = AppConfiguration.AppId;

        private ILogService logService = null!;

        private ActivationService activationService;

        private DialogService dialogService = null!;
        private SettingsService settingsService = null!;

        private AppSettings appSettings = null!;
        private AppState appState = null!;

        private AppShell appShell = null!;

        public const string AppExitCmd = "Exit";

        public App()
        {
            this.activationService = CreateActivationService();

            InitializeComponent();
        }

        public new static App Current => (App)Application.Current;

        public Window ActiveWindow
        {
            get
            {
                IntPtr activeHwnd = NativeMethods.GetActiveWindow();

                return this.Windows
                    .OfType<Window>()
                    .FirstOrDefault(window => new WindowInteropHelper(window).Handle == activeHwnd);
            }
        }

        public static bool RequestActivateRunningInstance()
        {
            return WindowWin32InteropHelper.RequestActivateWindow(applicationWindowName);
        }

        public static bool RequestCloseRunningInstance()
        {
            return WindowWin32InteropHelper.RequestCloseWindow(applicationWindowName);
        }

        public async Task ActivateAsync(IActivatedEventArgs args)
        {
            await this.activationService.ActivateAsync(args);
        }

        private ActivationService CreateActivationService()
        {
            var shellArgs = new ShellArgs(typeof(MainPage));
            return new ActivationService(new Lazy<ApplicationWindow>(CreateApplicationWindow), shellArgs);
        }

        private ApplicationWindow CreateApplicationWindow()
        {
            return new AppShell()
            {
                Title = App.applicationWindowName,
            };
        }

        protected async override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            await activationService.ActivateAsync(new LaunchActivatedEventArgs(e.Args));

            this.appShell = (AppShell)Application.Current.MainWindow;

            this.logService = ServiceLocator.Current.GetService<ILogService>();

            //this.navigationService = ServiceLocator.Current.GetService<INavigationService>();

            this.dialogService = ServiceLocator.Current.GetService<DialogService>();
            this.settingsService = ServiceLocator.Current.GetService<SettingsService>();

            this.appSettings = ServiceLocator.Current.GetService<AppSettings>();
            this.appState = ServiceLocator.Current.GetService<AppState>();

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            var chapterUpdateListeningService = ServiceLocator.Current.GetService<ChapterUpdateListeningService>();
            var chapterProfileServicePoint = ServiceLocator.Current.GetService<ChapterProfileServicePoint>();

            StopUpdateService();

            chapterUpdateListeningService.WaitAndStop();
            chapterProfileServicePoint.WaitAndStop();

            SaveUserPreferencesAndAppState();
        }

        protected override void OnSessionEnding(SessionEndingCancelEventArgs e)
        {
            base.OnSessionEnding(e);

            AppShell.Current?.SaveAndCleanup();

            var chapterUpdateListeningService = ServiceLocator.Current.GetService<ChapterUpdateListeningService>();
            var chapterProfileServicePoint = ServiceLocator.Current.GetService<ChapterProfileServicePoint>();

            StopUpdateService();

            chapterUpdateListeningService.WaitAndStop();
            chapterProfileServicePoint.WaitAndStop();

            SaveUserPreferencesAndAppState();

            Environment.Exit(0);
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            this.logService.Log(LogType.Fatal, (e.ExceptionObject as Exception)?.ToString() + "\r\n" + (e.ExceptionObject as Exception)?.Message);

            // Stop the update service.
            StopUpdateService();

            // Show the taskbar icon and set it to error mode to indicate to the user an error occured demanding their attention.
            if (!this.appShell.IsVisible)
            {
                DispatcherHelper.ExecuteOnUIThread(() => this.appShell.Show());
            }

            this.appShell.SetTaskbarButtonMode(AppTaskbarButtonMode.Error, TaskbarButtonResetMode.AppActivated);

            // Wait for the notifier app to enter the foreground.
            this.appShell.WaitUntilIsActivated();

            // Show our error message as soon as we are the foreground app.

            string message = "ATG-Notifier encountered a critical error and cannot continue. Do you want to restart the app? Press Yes for restart, or No to shutdown the notifier.";
            MessageDialogResult result = dialogService.ShowDialog(message, "Critical Error", MessageDialogButton.YesNo, MessageDialogIcon.Error,
                "Do you want to open the folder to the log file?", false, out bool openLogFileDirRequested);

            // Open the directory of the log file in the file explorer.
            if (openLogFileDirRequested)
            {
                var psi = new ProcessStartInfo
                {
                    FileName = AppConfiguration.LogfileDirectory,
                    UseShellExecute = true
                };

                Process.Start(psi);
            }

            DispatcherHelper.ExecuteOnUIThread(() => AppShell.Current?.SaveAndCleanup());

            // Save user preferences and app state.
            SaveUserPreferencesAndAppState();

            // Process the reply of the user about whether or not to restart the notifier app.
            if (result == MessageDialogResult.Yes)
            {
                RequestRestart();
            }
            else
            {
                Environment.Exit(1);
            }
        }

        private void StopUpdateService()
        {
            // Stop the update service
            var updateService = ServiceLocator.Current.GetService<IUpdateService>();

            this.appState.WasUpdateServiceRunning = updateService.IsRunning;
            if (updateService.IsRunning)
            {
                updateService.Stop();
            }
        }

        private void SaveUserPreferencesAndAppState()
        {
            try
            {
                this.settingsService.SaveAppSetings(this.appSettings);
                this.settingsService.SaveAppState(this.appState);
            }
            catch (Exception ex)
            {
                this.logService.Log(LogType.Error, $"Could not save app state and/or user preferences.\nTechnical details: {ex.Message}");
            }
        }

        private void RequestRestart()
        {
#if DesktopPackage || DesktopPackageDebug
            string restarterPath = Path.Combine(Package.Current.InstalledLocation.Path, @"ATG_Notifier.Restarter\ATG_Notifier.Restarter.exe");
            string notifierStartString = Package.Current.Id.FamilyName;
#else
            string notifierPath = Assembly.GetExecutingAssembly().Location;

            string? notifierDir = Path.GetDirectoryName(notifierPath);
            if (notifierDir == null)
            {
                this.logService.Log(LogType.Error, $"The notifier could not be restarted! Failed to retrieve the parent directory of the notifier executable with path <{notifierPath}>.");
                Environment.Exit(1);
            }

            string restarterPath = Path.Combine(notifierDir, @"Restarter\ATG_Notifier.Restarter.exe");
            string notifierStartString = notifierPath.Replace(".dll", ".exe");
#endif
            var currentProcess = Process.GetCurrentProcess();

            var restarterProcess = new Process();
            restarterProcess.StartInfo.FileName = restarterPath;
            restarterProcess.StartInfo.Arguments = $"{currentProcess.Id};{notifierStartString}";

            bool started = restarterProcess.Start();
            if (!started)
            {
                this.logService.Log(LogType.Error, "The notifier could not be restarted! The restart process could not be started.\n Additional info:\n" +
                    $"Restarter path: [{restarterPath}]\n" +
                    $"Restarter args: [{restarterProcess.StartInfo.Arguments}]");
            }

            Environment.Exit(1);
        }
    }
}