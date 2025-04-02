using CosysLib.ExceptionManagement;
using ServerWatchAgent.Driver;
using ServerWatchAgent.Mirroring;
using System;
using System.Collections.Specialized;
using System.ServiceProcess;
using System.Timers;

namespace ServerWatchAgent
{
    public partial class Service1 : ServiceBase
    {
        private Timer checkUpdateTimer;
        private Timer mirroringTimer;
        private Timer driverTimer;

        private readonly DataSender dataSender;
        private readonly Updater updater;

        public Service1()
        {
            InitializeComponent();
            dataSender = new DataSender();
            updater = new Updater();
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
            checkUpdateTimer = new Timer();
            checkUpdateTimer.Interval = 15000; // 15 sec interval
            checkUpdateTimer.Elapsed += CheckForUpdates;
            checkUpdateTimer.Start();

            //mirroringTimer = new Timer();
            //mirroringTimer.Interval = 30000; // 30 sec interval
            //mirroringTimer.Elapsed += GatherAndSendMirroringDataAsync;
            //mirroringTimer.Start();

            driverTimer = new Timer();
            driverTimer.Interval = 20000; // 20 sec interval
            driverTimer.Elapsed += GatherAndSendDriverDataAsync;
            driverTimer.Start();
        }

        private async void GatherAndSendDriverDataAsync(object sender, ElapsedEventArgs e)
        {
            try
            {
                var jsonData = DriverDataCollector.CheckDriversOnServer();

                await dataSender.SendDriverDataAsync(jsonData);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex, this.CollectRequestInfo(("OperationtType", "Mirroring")));
            }
        }

        private async void GatherAndSendMirroringDataAsync(object sender, ElapsedEventArgs e)
        {
            try
            {
                var jsonData = MirroringDataCollector.CheckMirroringOnServer();
                
                await dataSender.SendMirroringDataAsync(jsonData);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex, this.CollectRequestInfo(("OperationtType", "Mirroring")));
            }
        }

        private async void CheckForUpdates(object sender, ElapsedEventArgs e)
        {
            try
            {
                await updater.GetUpdateInfoAsync();
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex, this.CollectRequestInfo(("OperationtType", "Update")));
            }
        }

        private NameValueCollection CollectRequestInfo(params ValueTuple<string, string>[] operationInfo)
        {
            var requestInfo = new NameValueCollection();

            const string app = "ServerWatchAgent";

            if (operationInfo != null && operationInfo.Length > 0)
            {
                foreach (var i in operationInfo)
                {
                    requestInfo.Add(app + "." + i.Item1, i.Item2);
                }
            }

            return requestInfo;
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
