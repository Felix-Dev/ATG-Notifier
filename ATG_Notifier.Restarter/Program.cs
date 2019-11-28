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
                if (argumentData.Length < 2)
                {
                    Console.WriteLine("Error: Restarter was not supplied with the relevant data needed to perform the Notifier restart!");
                }
                else if (!int.TryParse(argumentData[0], out int procId))
                {
                    Console.WriteLine("Error: First argument is NOT a process ID!");
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

                    var appActivationManager = new ApplicationActivationManager();
                    var result = appActivationManager.ActivateApplication($"{argumentData[1]}!App", "Requested Restart", ActivateOptions.None, out _);
                    if (result.ToInt32() == 0)
                    {
                        Console.WriteLine("OK!");

                        return;
                    }
                    else
                    {
                        Console.WriteLine($"Failed with error {result.ToInt32().ToString()}!");
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
