using System;
using System.Collections.Generic;
using System.Data;

namespace ServerWatchAgent.Mirroring
{
    public class MirroringDataCollector
    {
        public static Dictionary<string, string> CheckMirroringOnServer()
        {
            using (var dl = new DataLayer())
            {
                DataSet databases = dl.GetMirroredDatabases();

                if (databases == null || databases.Tables.Count == 0)
                {
                    throw new Exception("No databases found on server that are being mirrored.");
                }

                System.IO.File.AppendAllText(@"C:\ServiceLogs\MyServiceLog.txt", $"Databases: {databases}\r\n");


                int mirroredDbCount = 0;
                int desyncedDbCount = 0;
                string desyncedDatabases = "";

                foreach (DataRow dbRow in databases.Tables[0].Rows)
                {
                    string dbName = dbRow["name"].ToString();
                    dl.database = dbName;

                    try
                    {
                        DataSet dbStatus = dl.ExecuteMonitorResultsOnServer();
                        System.IO.File.AppendAllText(@"C:\ServiceLogs\MyServiceLog.txt", $"Status: {dbStatus}\r\n");

                        if (dbStatus.Tables.Count > 0 && dbStatus.Tables[0].Rows.Count > 0)
                        {
                            var firstRow = dbStatus.Tables[0].Rows[0];
                            string role = firstRow["role"].ToString();
                            string mirroringState = firstRow["mirroring_state"].ToString();

                            if (role == "1" && mirroringState != "4")
                            {
                                desyncedDatabases += $"\n{dbName}";
                                desyncedDbCount++;
                            }

                            mirroredDbCount++;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error checking database {dbName} on server.", ex);
                    }
                }

                Dictionary<string, string> result = new Dictionary<string, string>()
                {
                    { "Databases", databases.ToString()},
                    { "Mirrored_db_count", mirroredDbCount.ToString() },
                    { "Desynced_db_count", desyncedDbCount.ToString() }
                };

                return result;
            }
        }
    }
}
