using ATG_Notifier.Restarter.Native.Win32;
using System;
using System.Diagnostics;

namespace ATG_Notifier.Restarter
{
    internal class Program
    {
        [STAThread]
        internal static void Main(string[] args)
        {
            if (args.Length >= 1)
            {
                Console.WriteLine($"Started restarter with argument: {args[0]}");

                string[] argumentData = args[0].Split(';', StringSplitOptions.RemoveEmptyEntries);
                if (argumentData.Length < 3)
                {
                    Console.WriteLine("Error: Restarter was not supplied with the relevant data needed to perform the Notifier restart!");
                }
                else if (!int.TryParse(argumentData[0], out int procId) || !int.TryParse(argumentData[1], out int appType))
                {
                    Console.WriteLine("Error: process ID and/or appType have invalid values!");
                }
                else
                {
                    Process notifProcess = null;
                    try
                    {
                        notifProcess = Process.GetProcessById(procId);
                    }
                    catch (ArgumentException)
                    {
                        Console.WriteLine("Info: Notifier has already been terminated.");
                    }

                    Console.Write($"Waiting for the Notifier to close...");

                    // Wait for calling Notifier process to exit
                    notifProcess?.WaitForExit();

                    Console.WriteLine("OK!");

                    Console.Write("Attempting to restart the Notifier...");

                    bool restartSuccess = false;
                    if (appType == 0)
                    {
                        var process = new Process();
                        process.StartInfo.FileName = argumentData[2];
                        process.StartInfo.Arguments = "restart";

                        try
                        {
                            restartSuccess = process.Start();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed! Could not start process {argumentData[2]}. Technical details: {ex.Message}");
                        }
                        
                    }
                    else if (appType == 1)
                    {
                        var appActivationManager = new ApplicationActivationManager();
                        var result = appActivationManager.ActivateApplication($"{argumentData[2]}!App", "Requested Restart", ActivateOptions.None, out _);

                        restartSuccess = result.ToInt32() == 0;
                        if (!restartSuccess)
                        {
                            Console.WriteLine($"Failed! See error {result.ToInt32().ToString()} for more details!");
                        }
                    }

                    if (restartSuccess)
                    {
                        Console.WriteLine("OK!");
                        return;
                    }
                }
            }
            else
            {
                Console.WriteLine("Error: Restarter was not supplied with the relevant data needed to perform the Notifier restart!");
            }

            Console.Write("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
