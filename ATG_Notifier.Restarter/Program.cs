using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;

namespace ATG_Notifier.Restarter
{
    class Program
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [STAThread]
        static void Main(string[] args)
        {
            XmlDocument log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead("log4net.config"));

            var repo = log4net.LogManager.CreateRepository(
                Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));

            log4net.Config.XmlConfigurator.Configure(repo, log4netConfig["log4net"]);

            if (args.Length == 1)
            {
                //string[] commArgs = Environment.GetCommandLineArgs()[1].Split(';');
                string[] commArgs = args[0].Split(';');

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
                    newNotif.StartInfo.Arguments = "restarter;";

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
