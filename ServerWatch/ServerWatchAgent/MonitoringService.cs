using CosysLib.ExceptionManagement;
using Newtonsoft.Json;
using ServerWatchAgent.Driver;
using ServerWatchAgent.Mirroring;
using System;
using System.Collections.Specialized;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Timers;

namespace ServerWatchAgent
{
    public partial class MonitoringService : ServiceBase
    {
        private Timer checkUpdateTimer;
        private Timer mirroringTimer;
        private Timer driverTimer;
        private Timer validationTimer;

        private readonly DataSender dataSender;
        private readonly Updater updater;

        public MonitoringService()
        {
            InitializeComponent();
            dataSender = new DataSender();
            updater = new Updater();
        }

        protected override void OnStart(string[] args)
        {
            checkUpdateTimer = new Timer();
            checkUpdateTimer.Interval = 15000; // 15 sec interval
            checkUpdateTimer.Elapsed += CheckForUpdates;
            checkUpdateTimer.Start();

            mirroringTimer = new Timer();
            mirroringTimer.Interval = 30000; // 30 sec interval
            mirroringTimer.Elapsed += GatherAndSendMirroringDataAsync;

            driverTimer = new Timer();
            driverTimer.Interval = 20000; // 20 sec interval
            driverTimer.Elapsed += GatherAndSendDriverDataAsync;

            validationTimer = new Timer();
            validationTimer.Interval = 60000; // 1 min interval
            validationTimer.Elapsed += ValidateAndStartTimers;
            validationTimer.Start();
        }

        private async void TryExecuteAsync(string operationType, Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex, this.CollectRequestInfo(("OperationtType", operationType)));
            }
        }

        private void ValidateAndStartTimers(object sender, ElapsedEventArgs e)
        {
            TryExecuteAsync("StartupValidation", async () =>
            {
                var approved = await dataSender.CheckApprovalStatusAsync();

                if (!approved)
                {
                    await dataSender.RegisterWithWebServiceAsync(
                        JsonConvert.SerializeObject(KeyContainerManager.GetPublicKey()));

                    throw new Exception("ServerWatchAgent is not approved by server.");
                }

                validationTimer.Stop();
                StartTimers();
            });
        }

        private void GatherAndSendDriverDataAsync(object sender, ElapsedEventArgs e)
        {
            TryExecuteAsync("DriverStatusReporting", async () =>
            {
                var jsonData = DriverDataCollector.CheckDriversOnServer();
                await dataSender.SendDriverDataAsync(jsonData);
            });
        }

        private void GatherAndSendMirroringDataAsync(object sender, ElapsedEventArgs e)
        {
            TryExecuteAsync("MirroringStatusReporting", async () =>
            {
                var jsonData = MirroringDataCollector.CheckMirroringOnServer();
                await dataSender.SendMirroringDataAsync(jsonData);
            });
        }

        private void CheckForUpdates(object sender, ElapsedEventArgs e)
        {
            TryExecuteAsync("FetchUpdateInfo", async () =>
            {
                await updater.GetUpdateInfoAsync();
            });
        }

        private void StartTimers()
        {
            mirroringTimer.Start();
            driverTimer.Start();
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
            
            if (driverTimer != null)
            {
                driverTimer.Stop();
                driverTimer.Dispose();
                driverTimer = null;
            }

            if (validationTimer != null)
            {
                validationTimer.Stop();
                validationTimer.Dispose();
                validationTimer = null;
            }
        }
    }
}
