using ATG_Notifier.Restarter.Native.Win32;
using System;
using System.Diagnostics;

namespace ATG_Notifier.Restarter
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
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
                    Console.WriteLine("Error: process ID is invalid!");
                }
                else
                {
                    // Attempt to retrieve the current notifier process and wait for it to stop.
                    // If the notifier process is no longer running, we will directly start it again.
                    Process? currentNotifierProcess = null;
                    try
                    {
                        currentNotifierProcess = Process.GetProcessById(procId);
                    }
                    catch (ArgumentException)
                    {
                        Console.WriteLine("Info: Notifier has already been terminated.");
                    }

                    Console.Write($"Waiting for the Notifier to close...");

                    // Wait for calling Notifier process to exit
                    currentNotifierProcess?.WaitForExit();

                    Console.WriteLine("OK!");

                    Console.Write("Attempting to restart the Notifier...");

                    bool restartSuccess = false;

                    // Launch a new notifier process.

#if DesktopPackage
                    restartSuccess = RestartPackagedApp(argumentData[1], out string? errorMessage);
#else
                    restartSuccess = RestartApp(argumentData[1], out string? errorMessage);
#endif

                    if (!restartSuccess)
                    {
                        Console.WriteLine(errorMessage);
                    }
                    else
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

        private static bool RestartPackagedApp(string appId, out string? errorMessage)
        {
            var appActivationManager = new ApplicationActivationManager();

            bool success;
            errorMessage = null;
            try
            {
                var result = appActivationManager.ActivateApplication($"{appId}!App", "Requested Restart", ActivateOptions.None, out _);

                success = result.ToInt32() == 0;
                if (!success)
                {
                    errorMessage = $"Failed! See error {result.ToInt32().ToString()} for more details!";
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Failed! Could not start application {appId}.\nTechnical details: {ex.Message}";
                return false;
            }

            return success;
        }

        private static bool RestartApp(string appPath, out string? errorMessage)
        {
            var process = new Process();
            process.StartInfo.FileName = appPath;
            process.StartInfo.Arguments = "restart";

            bool success;
            errorMessage = null;
            try
            {
                success = process.Start();
            }
            catch (Exception ex)
            {
                errorMessage = $"Failed! Could not start process {appPath}.\nTechnical details: {ex.Message}";
                return false;
            }

            if (!success)
            {
                errorMessage = "A new process instance was not started. Perhaps the process is already running?";
            }

            return success;
        }
    }
}
