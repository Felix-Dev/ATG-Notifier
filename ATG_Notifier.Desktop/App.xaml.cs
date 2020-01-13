using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.Helpers;
using ATG_Notifier.Desktop.Models;
using ATG_Notifier.Desktop.Native.Win32;
using ATG_Notifier.Desktop.Services;
using ATG_Notifier.Desktop.Views;
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

#if DesktopPackage
using Windows.ApplicationModel;
#endif

namespace ATG_Notifier.Desktop
{
    internal partial class App : Application
    {
        private ILogService logService = null!;
        private DialogService dialogService = null!;
        private SettingsService2 settingsService = null!;

        private AppSettings appSettings = null!;
        private AppState appState = null!;

        private AppShell appShell = null!;

        public const string AppExitCmd = "Exit";

        // TODO: Implement IDisposable?
        private readonly AutoResetEvent appActivatedResetEvent;

        public App()
        {
            this.appActivatedResetEvent = new AutoResetEvent(false);

            this.Activated += (s, e) => this.appActivatedResetEvent.Set();
            this.Deactivated += (s, e) => this.appActivatedResetEvent.Reset();

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

        public void Activate()
        {
            AppShell.Current?.BringIntoView();
        }

        protected async override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            await AppInitialization.InitializeAsync(e.Args);

            this.logService = ServiceLocator.Current.GetService<ILogService>();
            this.dialogService = ServiceLocator.Current.GetService<DialogService>();
            this.settingsService = ServiceLocator.Current.GetService<SettingsService2>();

            this.appSettings = ServiceLocator.Current.GetService<AppSettings>();
            this.appState = ServiceLocator.Current.GetService<AppState>();

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            PrepareForExit();
        }

        private void OnSessionEnding(object? sender, SessionEndingCancelEventArgs e)
        {
            AppShell.Current?.SaveAndCleanup();

            PrepareForExit();

            Environment.Exit(0);
        }

        private void PrepareForExit()
        {
            // Stop the update service
            var updateService = ServiceLocator.Current.GetService<IUpdateService>();

            this.appState.WasUpdateServiceRunning = updateService.IsRunning;
            if (updateService.IsRunning)
            {
                updateService.Stop();
            }

            // Save user preferences and app state
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

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            this.logService.Log(LogType.Fatal, (e.ExceptionObject as Exception)?.ToString() + "\r\n" + (e.ExceptionObject as Exception)?.Message);

            // Stop the update service
            var updateService = ServiceLocator.Current.GetService<IUpdateService>();
            if (updateService.IsRunning)
            {
                updateService.Stop();
            }

            // Show the taskbar icon and set it to error mode to indicate to the user an error occured demanding their attention.
            if (!this.appShell.IsVisible)
            {
                CommonHelpers.RunOnUIThread(() => this.appShell.Show());
            }

            this.appShell.SetTaskbarButtonMode(AppTaskbarButtonMode.Error, TaskbarButtonResetMode.AppActivated);

            // Wait for the notifier app to enter the foreground.
            this.appActivatedResetEvent.WaitOne();

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

            CommonHelpers.RunOnUIThread(() => AppShell.Current?.SaveAndCleanup());

            PrepareForExit();

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

        private void RequestRestart()
        {
            int appType;

#if DesktopPackage
            string restarterPath = Path.Combine(Package.Current.InstalledLocation.Path, @"ATG_Notifier.Restarter\ATG_Notifier.Restarter.exe");
            string notifierStartString = Package.Current.Id.FamilyName;

            appType = 1;
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
            appType = 0;
#endif
            var currentProcess = Process.GetCurrentProcess();

            var restarterProcess = new Process();
            restarterProcess.StartInfo.FileName = restarterPath;
            restarterProcess.StartInfo.Arguments = $"{currentProcess.Id};{appType};{notifierStartString}";

            bool started = restarterProcess.Start();
            if (!started)
            {
                this.logService.Log(LogType.Error, "The notifier could not be restarted! The restart process could not be started.\n Additional info:\n" +
                    $"Restarter path: [{restarterPath}]\n" +
                    $"Restarter args: [{restarterProcess.StartInfo.Arguments}]");
            }

            //Application.Current.Shutdown();
            Environment.Exit(1);
        }
    }
}