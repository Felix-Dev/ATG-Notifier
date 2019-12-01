using ATG_Notifier.Desktop.Components;
using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.Helpers;
using ATG_Notifier.Desktop.Models;
using ATG_Notifier.Desktop.Services;
using ATG_Notifier.Desktop.Services.Taskbar;
using ATG_Notifier.Desktop.Utilities;
using ATG_Notifier.Desktop.Views;
using ATG_Notifier.ViewModels.Services;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace ATG_Notifier.Desktop
{
    internal static class Program
    {
        public static MainWindow MainWindow { get; private set; }

        private static ILogService logService;

        private static DialogService dialogService;

        private static SettingsViewModel appSettings;

        [STAThread]
        static void Main(string[] args)
        {
            bool firstInstance = true;

#if DEBUG
            using (Mutex mtx = new Mutex(true, "ATG-Notfier.Debug.OneInstanceCheck", out firstInstance))
#else
            using (Mutex mtx = new Mutex(true, "ATG-Notfier.OneInstanceCheck", out firstInstance))
#endif
            {
                if (firstInstance)
                {
                    Startup.Configure();

                    AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
                    Application.ApplicationExit += OnApplicationExit;

                    logService = ServiceLocator.Current.GetService<ILogService>();

                    MainWindow = new MainWindow()
                    {
                        WindowState = FormWindowState.Normal
                    };

                    dialogService = ServiceLocator.Current.GetService<DialogService>();
                    dialogService.MainForm = MainWindow;
                    dialogService.DialogShown += OnDialogShown;

                    appSettings = ServiceLocator.Current.GetService<SettingsViewModel>();

                    /* Check if the notifier was started by the watchdog */
                    if (args.Length > 0)
                    {
                        string[] userCommands = args[0].Split(';');

                        if (userCommands?.Length == 2 && userCommands[0].Equals("restarter"))
                        {
                            logService.Log(LogType.Info, "Notifier was restarted by the Notifier restarter.");
                            MainWindow.WindowState = FormWindowState.Minimized;
                        }

                        if (args[0] == "ACTION_EXIT")
                        {
                            Application.Run(MainWindow);
                        }
                    }

                    Application.Run(MainWindow);
                }
                else
                {
                    // Send argument to running window
                    HandleCmdLineArgs(args);
                }
            }
        }

        public static void HandleCmdLineArgs(string[] args)
        {
            if (args.Length > 0)
            {
                switch (args[0])
                {
                    case JumplistManager.ActionExit:
                        WindowsMessageHelper.SendMessage(AppConfiguration.AppId, WindowsMessageHelper.WM_EXIT);
                        break;
                }
            }
            else
            {
                // send our Win32 message to make the currently running instance
                // jump on top of all the other windows
                WindowsMessageHelper.SendMessage(AppConfiguration.AppId, WindowsMessageHelper.WM_SHOWINSTANCE);
            }
        }

        private static void OnApplicationExit(object sender, EventArgs e)
        {
            // Stop the update service
            ServiceLocator.Current.GetService<IUpdateService>().Stop();

            // Save user data
            SaveData();

            Application.ApplicationExit -= OnApplicationExit;
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            logService.Log(LogType.Fatal, (e.ExceptionObject as Exception).ToString() + "\r\n" + (e.ExceptionObject as Exception).Message);

            CommonHelpers.RunOnUIThread(() =>
            {
                if (!Program.MainWindow.Visible)
                {
                    Program.MainWindow.Visible = true;
                }
            });

            TaskbarManager.Current.SetErrorTaskbarButton();

            string message = "ATG-Notifier encountered a critical error and cannot continue. Do you want to restart the app? Press Yes for restart, or No to shutdown the notifier.";
            DialogResult result = dialogService.ShowDialog("critical error", "Critical Error", message, MessageDialogButton.YesNo, MessageDialogIcon.Error, "Do you want to open the folder to the log file?", out bool openLogFile);

            if (openLogFile)
            {
                var psi = new ProcessStartInfo
                {
                    FileName = AppConfiguration.LogfileDirectory,
                    UseShellExecute = true
                };

                Process.Start(psi);
            }

            if (result == DialogResult.Yes)
            {
                RequestRestart();
            }
            else
            {
                Application.Exit();
            }
        }

        private static void OnDialogShown(object sender, DialogShownEventArgs e)
        {
            if (e.Id == "critical error")
            {
                TaskbarManager.Current.ClearErrorTaskbarButton();
            }
        }

#if DesktopPackage
        private static void RequestRestart()
        {
            /*  Start watchdog which restarts the notifier. */
            var installLocation = Windows.ApplicationModel.Package.Current.InstalledLocation;
            var package = Windows.ApplicationModel.Package.Current;

            string id = package.Id.FamilyName;
            var proc = Process.GetCurrentProcess();

            Process wd = new Process();
            wd.StartInfo.FileName = Path.Combine(installLocation.Path, @"ATG_Notifier.Restarter\Restarter.exe");
            wd.StartInfo.Arguments = $"{proc.Id};{id}";

            bool started = wd.Start();
            if (!started)
            {
                logService.Log(LogType.Error, "The notifier could not be restarted! The restart process could not be started.");
            }

            Application.Exit();
        }
#else
        private static void RequestRestart()
        {
            Application.Restart();
        }
#endif

        private static void PerformRecovery()
        {
            logService.Log(LogType.Info, "Attempting to restart the notifier...");

            /* Save data. */
            SaveData();

            /*  Start watchdog which restarts the notifier. */
            var asm = Assembly.GetExecutingAssembly();

            var proc = Process.GetCurrentProcess();

            Process wd = new Process();

            wd.StartInfo.FileName = Path.Combine(Path.GetDirectoryName(asm.Location), @"Restarter\Restarter.exe");
            wd.StartInfo.Arguments = proc.Id + ";" + asm.Location;
            wd.StartInfo.UseShellExecute = false;
            wd.StartInfo.CreateNoWindow = true;

            bool started = wd.Start();
            if (!started)
            {
                logService.Log(LogType.Error, "The notifier could not be restarted! The restart process could not be started.");
            }
        }

        private static void Restart()
        {
            /*  Start watchdog which restarts the notifier. */
            var asm = Assembly.GetExecutingAssembly();

            var proc = Process.GetCurrentProcess();

            Process wd = new Process();

            wd.StartInfo.FileName = Path.Combine(Path.GetDirectoryName(asm.Location), @"Restarter\Restarter.exe");
            wd.StartInfo.Arguments = proc.Id + ";" + asm.Location;
            wd.StartInfo.UseShellExecute = false;
            wd.StartInfo.CreateNoWindow = true;

            bool started = wd.Start();
            if (!started)
            {
                logService.Log(LogType.Error, "The notifier could not be restarted! The restart process could not be started.");
            }
        }

        /// <summary>
        /// Save data (user settings).
        /// </summary>
        private static void SaveData()
        {
            /* Save user settings. */
            appSettings.Save();
        }
    }
}
