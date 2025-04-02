using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;

namespace ServerWatchAgent.Mirroring
{
    public class MirroringDataCollector
    {
        public static string CheckMirroringOnServer()
        {
            using (var dl = new DataLayer())
            {
                DataSet databases = dl.GetMirroredDatabases();

                if (databases == null || databases.Tables.Count == 0)
                {
                    throw new Exception("No databases found on server that are being mirrored.");
                }

                var allDbStatusData = new List<MirroringData>();

                foreach (DataRow dbRow in databases.Tables[0].Rows)
                {
                    string dbName = dbRow["name"].ToString();
                    dl.database = dbName;

                    try
                    {
                        DataSet dbStatus = dl.ExecuteMonitorResultsOnServer();

                        if (dbStatus != null && dbStatus.Tables.Count > 0)
                        {
                            foreach (DataRow statusRow in dbStatus.Tables[0].Rows)
                            {
                                var mirroringData = new MirroringData
                                {
                                    ServerName = dl.GetServer(),
                                    DatabaseName = statusRow["database_name"]?.ToString(),
                                    Role = Convert.ToInt32(statusRow["role"]),
                                    MirroringState = Convert.ToInt32(statusRow["mirroring_state"]),
                                    WitnessStatus = Convert.ToInt32(statusRow["witness_status"]),
                                    LogGenerationRate = Convert.ToInt32(statusRow["log_generation_rate"]),
                                    UnsentLog = Convert.ToInt32(statusRow["unsent_log"]),
                                    SendRate = Convert.ToInt32(statusRow["send_rate"]),
                                    UnrestoredLog = Convert.ToInt32(statusRow["unrestored_log"]),
                                    RecoveryRate = Convert.ToInt32(statusRow["recovery_rate"]),
                                    TransactionDelay = Convert.ToInt32(statusRow["transaction_delay"]),
                                    TransactionsPerSec = Convert.ToInt32(statusRow["transactions_per_sec"]),
                                    AverageDelay = Convert.ToInt32(statusRow["average_delay"]),
                                    TimeRecorded = Convert.ToDateTime(statusRow["time_recorded"]),
                                    TimeBehind = statusRow.Field<DateTime?>("time_behind") ?? null,
                                    LocalTime = Convert.ToDateTime(statusRow["local_time"])
                                };

                                allDbStatusData.Add(mirroringData);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error checking database {dbName} on server.", ex);
                    }
                }

                return JsonConvert.SerializeObject(allDbStatusData, Formatting.Indented);
            }
        }
    }
}
