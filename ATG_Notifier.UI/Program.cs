using ATG_Notifier.Data;
using ATG_Notifier.Data.Sql;
using ATG_Notifier.UI.Services;
using ATG_Notifier.UI.Utilities;
using ATG_Notifier.Utilities;
using ATG_Notifier.ViewModels.Services;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace ATG_Notifier.UI
{
    public static class Program
    {
        public static MainWindow MainWindow { get; private set; }

        /// <summary>
        /// Pipeline for interacting with backend service or database.
        /// </summary>
        public static INotifierRepository Repository { get; private set; }

        public static CommonServices CommonServices { get; private set; }

        private static readonly string DB_DIR = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) +
                @"\ATG_Notifier\");

        /// <summary>
        /// Der Pfad der Datenbank. Sie wird in den lokalen Ordner der App gespeichert.
        /// </summary>
        private static readonly string DB_NAME = "Notifications.db";

        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
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
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    Repository = new NotifierRepository(Path.Combine(DB_DIR, DB_NAME));

                    CommonServices = new CommonServices(new LogService());

                    MainWindow = new MainWindow()
                    {
                        WindowState = FormWindowState.Normal
                    };

                    /* Check if the notifier was started by the watchdog */
                    if (Environment.GetCommandLineArgs().Length > 1)
                    {
                        string[] userCommands = Environment.GetCommandLineArgs()[1].Split(';');

                        if (userCommands?.Length == 2 && userCommands[0].Equals("watchdog"))
                        {
                            MainWindow.WindowState = FormWindowState.Minimized;
                        }
                    }

                    Application.Run(MainWindow);
                }

                else
                {
                    // Send argument to running window
                    HandleCmdLineArgs();
                }
            }                     
        }

        public static void HandleCmdLineArgs()
        {
            if (Environment.GetCommandLineArgs().Length > 1)
            {
                switch (Environment.GetCommandLineArgs()[1])
                {
                    case JumplistManager.ACTION_EXIT:
                        WindowsMessageHelper.SendMessage("ATG-Notifier", WindowsMessageHelper.TaskCloseArg);
                        break;
                }
            }
        }
    }
}
