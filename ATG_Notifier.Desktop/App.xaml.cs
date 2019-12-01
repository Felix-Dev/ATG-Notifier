using ATG_Notifier.Desktop.Components;
using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.Helpers;
using ATG_Notifier.Desktop.Models;
using ATG_Notifier.Desktop.Native.Win32;
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
using System.Windows;

#if DesktopPackage
using Windows.ApplicationModel;
#endif

namespace ATG_Notifier.Desktop
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    internal partial class App : Application
    {
        private static ILogService logService;
        private static DialogService dialogService;

        public static new MainWindow MainWindow { get; private set; }

        /// <summary>
        /// Application Entry Point.
        /// </summary>
        [STAThreadAttribute()]
        public static void Main(string[] args)
        {
            bool firstInstance;
#if DEBUG
            using (Mutex mtx = new Mutex(true, "ATG-Notfier.Debug.OneInstanceCheck", out firstInstance))
#else
            using (Mutex mtx = new Mutex(true, "ATG-Notfier.OneInstanceCheck", out firstInstance))
#endif
            {
                if (firstInstance)
                {
                    Configuration.Startup.Configure();

                    AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

                    logService = ServiceLocator.Current.GetService<ILogService>();
                    dialogService = ServiceLocator.Current.GetService<DialogService>();

                    var app = new App();
                    app.InitializeComponent();

                    app.Run();
                }
                else
                {
                    HandleArguments(args);
                }
            }
        }

        private void OnSessionEnding(object sender, SessionEndingCancelEventArgs e)
        {
            // Stop the update service
            ServiceLocator.Current.GetService<IUpdateService>().Stop();

            JumplistManager.ClearJumplist();

            // Save user data
            SaveData();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            // Stop the update service
            ServiceLocator.Current.GetService<IUpdateService>().Stop();

            JumplistManager.ClearJumplist();

            App.MainWindow.Close();

            // Save user data
            SaveData();
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            logService.Log(LogType.Fatal, (e.ExceptionObject as Exception).ToString() + "\r\n" + (e.ExceptionObject as Exception).Message);

            CommonHelpers.RunOnUIThread(() =>
            {
                if (!App.MainWindow.Visible)
                {
                    App.MainWindow.Visible = true;
                }
            });

            TaskbarManager.Current.SetErrorTaskbarButton();

            string message = "ATG-Notifier encountered a critical error and cannot continue. Do you want to restart the app? Press Yes for restart, or No to shutdown the notifier.";
            System.Windows.Forms.DialogResult result = dialogService.ShowDialog("critical error", "Critical Error", message, MessageDialogButton.YesNo, MessageDialogIcon.Error, "Do you want to open the folder to the log file?", out bool openLogFile);

            if (openLogFile)
            {
                var psi = new ProcessStartInfo
                {
                    FileName = AppConfiguration.LogfileDirectory,
                    UseShellExecute = true
                };

                Process.Start(psi);
            }

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                RequestRestart();
            }
            else
            {
                Application.Current.Shutdown();
            }
        }
        private static void RequestRestart()
        {
            int appType;

#if DesktopPackage
            /*  Start watchdog which restarts the notifier. */
            var installLocation = Package.Current.InstalledLocation.Path;
            var package = Package.Current;

            string id = package.Id.FamilyName;

            appType = 1;
#else
            string location = Assembly.GetExecutingAssembly().Location;
            string installLocation = Path.GetDirectoryName(location);

            string id = location.Replace(".dll", ".exe");
            appType = 0;
#endif
            var proc = Process.GetCurrentProcess();

            Process wd = new Process();
            wd.StartInfo.FileName = Path.Combine(installLocation, @"ATG_Notifier.Restarter\Restarter.exe");
            wd.StartInfo.Arguments = $"{proc.Id};{appType};{id}";

            bool started = wd.Start();
            if (!started)
            {
                logService.Log(LogType.Error, "The notifier could not be restarted! The restart process could not be started.");
            }

            Application.Current.Shutdown();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            if (e.Args.Length > 0)
            {
                //ProcessCommandLine(e.Args);
            }

            base.OnStartup(e);

            App.MainWindow = new MainWindow();
            dialogService.MainForm = App.MainWindow;

            App.MainWindow.Show();

        }

        private  static void HandleArguments(string[] args)
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

                MessageBox.Show("Test");

                WindowsMessageHelper.SendMessage(AppConfiguration.AppId, WindowsMessageHelper.WM_SHOWINSTANCE);
            }
        }

        /// <summary>
        /// Save data (user settings).
        /// </summary>
        private static void SaveData()
        {
            /* Save user settings. */
            ServiceLocator.Current.GetService<SettingsViewModel>().Save();
        }
    }
}