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
                                    DatabaseName = statusRow["database_name"]?.ToString(),
                                    Role = SafeInt(statusRow["role"]),
                                    MirroringState = SafeInt(statusRow["mirroring_state"]),
                                    WitnessStatus = SafeInt(statusRow["witness_status"]),
                                    LogGenerationRate = SafeInt(statusRow["log_generation_rate"]),
                                    UnsentLog = SafeInt(statusRow["unsent_log"]),
                                    SendRate = SafeInt(statusRow["send_rate"]),
                                    UnrestoredLog = SafeInt(statusRow["unrestored_log"]),
                                    RecoveryRate = SafeInt(statusRow["recovery_rate"]),
                                    TransactionDelay = SafeInt(statusRow["transaction_delay"]),
                                    TransactionsPerSec = SafeInt(statusRow["transactions_per_sec"]),
                                    AverageDelay = SafeInt(statusRow["average_delay"]),
                                    TimeRecorded = SafeDateTime(statusRow["time_recorded"]),
                                    TimeBehind = SafeDateTime(statusRow["time_behind"]),
                                    LocalTime = SafeDateTime(statusRow["local_time"]),
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
        public static int? SafeInt(object value)
        {
            if (value == null || value is DBNull) return null;
            if (int.TryParse(value.ToString(), out int result)) return result;
            try { return Convert.ToInt32(value); } catch { return null; }
        }

        public static DateTime? SafeDateTime(object value)
        {
            if (value == null || value is DBNull) return null;
            if (DateTime.TryParse(value.ToString(), out DateTime result)) return result;
            try { return Convert.ToDateTime(value); } catch { return null; }
        }
    }
}
