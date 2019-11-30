using ATG_Notifier.Desktop.Configuration;
using ATG_Notifier.Desktop.Models;
using ATG_Notifier.Desktop.Native.Win32;
using ATG_Notifier.Desktop.Utilities;
using ATG_Notifier.Desktop.Views;
using ATG_Notifier.ViewModels.Services;
using System;
using System.Threading;
using System.Windows;

namespace ATG_Notifier.Desktop
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    internal partial class App : Application
    {
        private static ILogService logService;

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

            JumplistManager.ClearJumplistWpf();

            // Save user data
            SaveData();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            // Stop the update service
            ServiceLocator.Current.GetService<IUpdateService>().Stop();

            JumplistManager.ClearJumplistWpf();

            // Save user data
            SaveData();
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            if (e.Args.Length > 0)
            {
                //ProcessCommandLine(e.Args);
            }

            base.OnStartup(e);

            var window = new MainWindow2();
            window.Show();

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

        public void Activate()
        {
            this.MainWindow.Activate();
        }
    }
}