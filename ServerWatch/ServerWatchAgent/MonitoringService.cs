using CosysLib.ExceptionManagement;
using Newtonsoft.Json;
using ServerWatchAgent.Backup;
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
        private Timer validationTimer;
        private Timer checkUpdateTimer;
        private Timer mirroringTimer;
        private Timer driverTimer;
        private Timer backupCheckTimer;

        private readonly DataSender dataSender;
        private readonly Updater updater;

        public MonitoringService()
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
            try
            {
                KeyContainerManager.Initialize();
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex, this.CollectRequestInfo(("OperationType", "KeyContainerInitialization")));
            }

            validationTimer = new Timer();
            validationTimer.Interval = 60 * 1000; // 1 minute
            validationTimer.Elapsed += ValidateAndStartTimers;
            validationTimer.Start();

            checkUpdateTimer = new Timer();
            checkUpdateTimer.Interval = 60 * 1000; // 1 minute
            checkUpdateTimer.Elapsed += CheckForUpdates;
            checkUpdateTimer.Start();

            mirroringTimer = new Timer();
            mirroringTimer.Interval = 60 * 1000; // 1 minute
            mirroringTimer.Elapsed += GatherAndSendMirroringDataAsync;

            driverTimer = new Timer();
            driverTimer.Interval = 60 * 1000; // 1 minute
            driverTimer.Elapsed += GatherAndSendDriverDataAsync;

            backupCheckTimer = new Timer();
            backupCheckTimer.Interval = 12* 60 * 60 * 1000; // 12 hour
            backupCheckTimer.Elapsed += GatherAndSendBackupDataAsync;
        }

        private async void TryExecuteAsync(string operationType, Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                var requestInfo = this.CollectRequestInfo(("OperationType", operationType));
                Exception current = ex;
                int innerLevel = 0;
                while (current != null)
                {
                    requestInfo.Add($"ServerWatchAgent.Exception.Level{innerLevel}", current.GetType().FullName + ": " + current.Message);
                    current = current.InnerException;
                    innerLevel++;
                }
                ExceptionManager.Publish(ex, requestInfo);
            }
        }

        private void ValidateAndStartTimers(object sender, ElapsedEventArgs e)
        {
            TryExecuteAsync("StartupValidation", async () =>
            {
                var approved = await dataSender.CheckApprovalStatusAsync();

                if (!approved)
                {
                    var serverToRegister = new ServerRegistration
                    {
                        GUID = KeyContainerManager.Guid,
                        PublicKey = KeyContainerManager.GetPublicKey()
                    };

                    string json = JsonConvert.SerializeObject(serverToRegister);

                    await dataSender.RegisterWithWebServiceAsync(json);

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

        private void GatherAndSendBackupDataAsync(object sender, ElapsedEventArgs e)
        {
            TryExecuteAsync("BackupStatusReporting", async () =>
            {
                string latestBackupFolder = await dataSender.GetBackupFolderPathAsync();

                if (!string.IsNullOrWhiteSpace(latestBackupFolder))
                {
                    BackupDataCollector.UpdateBackupFolderPath(latestBackupFolder);
                }

                var result = await BackupDataCollector.BackupCheckAndGetResultAsync();
                var jsonData = JsonConvert.SerializeObject(result);
                await dataSender.SendBackupDataAsync(jsonData);
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
            backupCheckTimer.Start();
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

            if (backupCheckTimer != null)
            {
                backupCheckTimer.Stop();
                backupCheckTimer.Dispose();
                backupCheckTimer = null;
            }
        }
    }
}
