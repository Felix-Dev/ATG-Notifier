using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.Model;
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
    public static class Program
    {
        /// <summary>The minimum runtime of the notifier to be eligible for a restart. In seconds.</summary>
        private const int MinimumRuntimeForRestart = 60;

        public static MainWindow MainWindow { get; private set; }

        private static ILogService logService;

        private static Stopwatch runtimeStopwatch;

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

                    /* Check if the notifier was started by the watchdog */
                    if (args.Length > 0)
                    {
                        string[] userCommands = args[0].Split(';');

                        if (userCommands?.Length == 2 && userCommands[0].Equals("restarter"))
                        {
                            logService.Log(LogType.Info, "Notifier was restarted by the Notifier restarter.");
                            MainWindow.WindowState = FormWindowState.Minimized;
                        }
                    }

                    runtimeStopwatch = new Stopwatch();
                    runtimeStopwatch.Start();

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
                    case JumplistManager.ACTION_EXIT:
                        WindowsMessageHelper.SendMessage(AppConfiguration.AppId, WindowsMessageHelper.TaskCloseArg);
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

            runtimeStopwatch.Stop();
            if (runtimeStopwatch.Elapsed.CompareTo(TimeSpan.FromSeconds(MinimumRuntimeForRestart)) >= 0)
            {
                PerformRecovery();
                Application.Exit();
            }
            else
            {
                logService.Log(LogType.Info, $"The notifier crashed less than {MinimumRuntimeForRestart} seconds after its start. No restart will be performed.");
                Application.Exit();
            }
        }

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

        /// <summary>
        /// Save data (user settings).
        /// </summary>
        private static void SaveData()
        {
            /* Save user settings. */
            Settings.Instance.Save();
        }
    }
}
