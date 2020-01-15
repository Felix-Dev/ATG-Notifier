using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATG_Notifier_Watchdog
{
    public static class Program
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(typeof(Program));

        [STAThread]
        static void Main()
        {
            if (Environment.GetCommandLineArgs().Length == 2)
            {
                string[] commArgs = Environment.GetCommandLineArgs()[1].Split(';');

                if (int.TryParse(commArgs[0], out int procId))
                {
                    Process notifProcess = null;

                    try
                    {
                        notifProcess = Process.GetProcessById(procId);
                    }
                    catch (ArgumentException)
                    {
                        log.Info("ATG_Notifier process is no longer running");
                    }

                    // Wait for calling ATG_Notifier process to exit
                    notifProcess?.WaitForExit();

                    Process newNotif = new Process();
                    newNotif.StartInfo.FileName = commArgs[1];
                    newNotif.StartInfo.Arguments = "watchdog;";

                    bool started = newNotif.Start();
                    if (!started)
                    {
                        log.Error("ERROR: ATG_Notifier Process couldn't be started!\n" +
                            $"Attempted to start process {commArgs[1]}");
                    }
                    else
                    {
                        log.Info("OK: ATG_Notifier Process could be started!");
                    }
                }
                
                else
                {
                    log.Error("Error: First argument is NOT a process ID");
                }
            }

            else
            {
                log.Error("Error: Arguments mismatch!");
            }
        }

    }
}
