using Newtonsoft.Json;
using ServerWatchAgent.Mirroring;
using System;
using System.Diagnostics;
using System.Reflection;
using System.ServiceProcess;
using System.Timers;

namespace ServerWatchAgent
{
    public partial class Service1 : ServiceBase
    {
        private Timer checkUpdateTimer;
        private Timer mirroringTimer;
        private readonly DataSender dataSender;

        public Service1()
        {
            InitializeComponent();
            dataSender = new DataSender();
        }

        public void DebugRun(string[] args)
        {
            OnStart(args);

            Console.WriteLine("Service running... Press any key to stop.");
            Console.ReadKey();

            OnStop();
        }

        protected override void OnStart(string[] args)
        {
#if DEBUG
            System.Diagnostics.Debugger.Launch();
#endif
            //checkUpdateTimer = new Timer();
            //checkUpdateTimer.Interval = 30000; // 30 sec interval
            //checkUpdateTimer.Elapsed += CheckForUpdates;
            //checkUpdateTimer.Start();

            mirroringTimer = new Timer();
            //mirroringTimer.Interval = 3600000; // 1h interval
            mirroringTimer.Interval = 30000; // 30 sec interval
            mirroringTimer.Elapsed += GatherAndSendMirroringDataAsync;
            mirroringTimer.Start();
        }

        private async void GatherAndSendMirroringDataAsync(object sender, ElapsedEventArgs e)
        {
            try
            {
                var jsonData = MirroringDataCollector.CheckMirroringOnServer();

                System.IO.File.AppendAllText(@"C:\ServiceLogs\MyServiceLog.txt", $"{jsonData}\r\n");
                
                await dataSender.SendMirroringDataAsync(jsonData);
            }
            catch (Exception ex)
            {
                string text = $"Error while gathering and sending mirroring data: {ex.Message}\r\n";
                System.IO.File.AppendAllText(@"C:\ServiceLogs\MyServiceLog.txt", text);
            }
            finally
            {
                string text = $"Mirroring data gathered and sent.\r\n";
                System.IO.File.AppendAllText(@"C:\ServiceLogs\MyServiceLog.txt", text);
            }
        }

        private void CheckForUpdates(object sender, ElapsedEventArgs e)
        {
            try
            {
                string oldExePath = Assembly.GetExecutingAssembly().Location;
                string newExePath = @"C:\Users\Zalan\Desktop\Disszertáció\ServerWatch\ServerWatch\ServerWatchAgent\bin\Release\ServerWatchAgent.exe";
                
                var currentVersion = new Version(FileVersionInfo.GetVersionInfo(oldExePath).FileVersion);

                var availableVersion = new Version(FileVersionInfo.GetVersionInfo(newExePath).FileVersion);

                if (currentVersion.CompareTo(availableVersion) < 0)
                {
                    StartUpdaterProcess(serviceName: "ServerWatchAgent", newExePath, oldExePath);

                    string text = $"Service updated successfully. {currentVersion} - {availableVersion}\r\n";
                    System.IO.File.AppendAllText(@"C:\ServiceLogs\MyServiceLog.txt", text);
                }
            }
            catch (Exception ex)
            {
                string text = $"Error while checking for updates: {ex.Message}\r\n";
                System.IO.File.AppendAllText(@"C:\ServiceLogs\MyServiceLog.txt", text);
            }
            finally
            {
                string text = $"Service checked for update.\r\n";
                System.IO.File.AppendAllText(@"C:\ServiceLogs\MyServiceLog.txt", text);
            }
        }

        private void StartUpdaterProcess(string serviceName, string newExePath, string oldExePath)
        {
            var updaterExePath = @"C:\Users\Zalan\Desktop\Disszertáció\ServerWatch\ServerWatch\ServerWatchAgentUpdater\bin\Debug\netcoreapp3.1\ServerWatchAgentUpdater.exe";

            // arguments:  <serviceName> <newExePath> <targetExePath>
            var processInfo = new ProcessStartInfo
            {
                FileName = updaterExePath,
                Arguments = $"\"{serviceName}\" \"{newExePath}\" \"{oldExePath}\"",
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process.Start(processInfo);
        }

        protected override void OnStop()
        {
            if (checkUpdateTimer != null)
            {
                checkUpdateTimer.Stop();
                checkUpdateTimer.Dispose();
                checkUpdateTimer = null;
            }

            if (mirroringTimer != null)
            {
                mirroringTimer.Stop();
                mirroringTimer.Dispose();
                mirroringTimer = null;
            }
        }
    }
}
