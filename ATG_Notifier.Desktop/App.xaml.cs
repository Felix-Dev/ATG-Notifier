using ATG_Notifier.Desktop.Components;
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
using System.Windows;
using System.Windows.Interop;

#if DesktopPackage
using Windows.ApplicationModel;
#endif

namespace ATG_Notifier.Desktop
{
    internal partial class App : Application
    {
        private static ILogService logService;
        private static DialogService dialogService;
        private static TaskbarButtonService taskbarButtonService;

        public static new MainView MainWindow => (MainView)Application.Current.MainWindow;

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
            App.MainWindow?.BringIntoView();
        }

        public bool HandleArguments(string[] args)
        {
            if (args.Length > 0)
            {
                switch (args[0])
                {
                    case JumplistManager.ActionExit:
                        WindowWin32InteropHelper.SendMessage(AppConfiguration.AppId, WindowWin32InteropHelper.WM_EXIT);
                        return true;
                }
            }

            return false;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            ServiceLocator.Configure();

            logService = ServiceLocator.Current.GetService<ILogService>();
            dialogService = ServiceLocator.Current.GetService<DialogService>();
            taskbarButtonService = ServiceLocator.Current.GetService<TaskbarButtonService>();

            JumplistManager.BuildJumplist();

            var mainView = new MainView();
            mainView.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            SaveAndCleanup();
        }

        private void OnSessionEnding(object? sender, SessionEndingCancelEventArgs e)
        {
            SaveAndCleanup();

            Environment.Exit(0);
        }

        private void SaveAndCleanup()
        {
            // Stop the update service
            ServiceLocator.Current.GetService<IUpdateService>().Stop();

            // clear jumplist from added custom entries
            JumplistManager.ClearJumplist();

            // Save user data
            ServiceLocator.Current.GetService<SettingsViewModel>().Save();
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            logService.Log(LogType.Fatal, (e.ExceptionObject as Exception)?.ToString() + "\r\n" + (e.ExceptionObject as Exception)?.Message);

            App.Current.SaveAndCleanup();

            // Show the taskbar icon and set it to error mode to indicate to the user an error occured demanding their attention.
            CommonHelpers.RunOnUIThread(() =>
            {
                if (Application.Current.MainWindow.Visibility == Visibility.Hidden)
                {
                    Application.Current.MainWindow.Visibility = Visibility.Visible;
                }
            });

            taskbarButtonService.SetErrorMode();

            // Wait for the notifier app to enter the foreground.
            App.Current.appActivatedResetEvent.WaitOne();

            // As soon as we are the foreground app, clear the error mode of the taskbar button
            // and show our error message.
            taskbarButtonService.ClearButton();
            ShowAndProcessErrorDialog();

            void ShowAndProcessErrorDialog()
            {
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
        }

        private void RequestRestart()
        {
            int appType;

#if DesktopPackage
            /*  Start watchdog which restarts the notifier. */
            string restarterPath = Path.Combine(Package.Current.InstalledLocation.Path, @"ATG_Notifier.Restarter\ATG_Notifier.Restarter.exe");
            string notifierStartString = Package.Current.Id.FamilyName;

            appType = 1;
#else
            string notifierPath = Assembly.GetExecutingAssembly().Location;

            string? notifierDir = Path.GetDirectoryName(notifierPath);
            if (notifierDir == null)
            {
                logService.Log(LogType.Error, $"The notifier could not be restarted! Failed to retrieve the parent directory of the notifier executable with path <{notifierPath}>.");
                Environment.Exit(1);
            }

            string restarterPath = Path.Combine(notifierDir, @"Restarter\ATG_Notifier.Restarter.exe");
            string notifierStartString = notifierPath.Replace(".dll", ".exe");
            appType = 0;
#endif
            var proc = Process.GetCurrentProcess();

            var restarterProcess = new Process();
            restarterProcess.StartInfo.FileName = restarterPath;
            restarterProcess.StartInfo.Arguments = $"{proc.Id};{appType};{notifierStartString}";

            bool started = restarterProcess.Start();
            if (!started)
            {
                logService.Log(LogType.Error, "The notifier could not be restarted! The restart process could not be started.\n Additional info:\n" +
                    $"Restarter path: [{restarterPath}]\n" +
                    $"Restarter args: [{restarterProcess.StartInfo.Arguments}]");
            }

            Environment.Exit(1);
        }
    }
}