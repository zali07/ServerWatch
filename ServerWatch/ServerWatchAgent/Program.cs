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
            ServiceBase[] ServicesToRun;

            ServicesToRun = new ServiceBase[]
            {
                new MonitoringService()
            };

            ServiceBase.Run(ServicesToRun);
        }
    }
}
