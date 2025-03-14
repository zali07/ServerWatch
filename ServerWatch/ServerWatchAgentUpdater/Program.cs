using System;
using System.Diagnostics;
using System.IO;

namespace ServerWatchAgentUpdater
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Usage: ServerWatchAgentUpdater.exe <serviceName> <newExePath> <targetExePath>");

                return 1;
            }

            string serviceName = args[0];
            string newExePath = args[1];
            string targetExePath = args[2];

            try
            {
                RunCommand($"net stop {serviceName}");

                System.Threading.Thread.Sleep(3000);

                File.Copy(newExePath, targetExePath, overwrite: true);

                RunCommand($"net start {serviceName}");

                Console.WriteLine("Update completed successfully!");

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");

                return 1;
            }
        }

        private static void RunCommand(string command)
        {
            var psi = new ProcessStartInfo("cmd.exe", $"/c {command}")
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = false,
                RedirectStandardError = false
            };

            using (var proc = Process.Start(psi))
            {
                proc.WaitForExit();
            }
        }
    }
}
