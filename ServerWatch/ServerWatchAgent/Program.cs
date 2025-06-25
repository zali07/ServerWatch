using System;
using System.ServiceProcess;

namespace ServerWatchAgent
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                // Running as console application for debugging
                MonitoringService service = new MonitoringService();
                service.DebugRun(args);
            }
            else
            {
                ServiceBase[] ServicesToRun;

                ServicesToRun = new ServiceBase[]
                {
                new MonitoringService()
                };

                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
