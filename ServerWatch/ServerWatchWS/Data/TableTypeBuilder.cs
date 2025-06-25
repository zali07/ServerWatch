using ServerWatchAPI.Model;
using System.Data;

namespace ServerWatchAPI.Data
{
    public static class TableTypeBuilder
    {
        public static DataTable CreateDriverEntryTable(IEnumerable<DriverData> entries)
        {
            var table = new DataTable();

            table.Columns.Add("ServerGUID", typeof(string));
            table.Columns.Add("DeviceId", typeof(long));
            table.Columns.Add("FriendlyName", typeof(string));
            table.Columns.Add("SerialNumber", typeof(string));
            table.Columns.Add("Model", typeof(string));
            table.Columns.Add("MediaType", typeof(string));
            table.Columns.Add("HealthStatus", typeof(string));
            table.Columns.Add("SizeGB", typeof(string));
            table.Columns.Add("Temperature", typeof(int));
            table.Columns.Add("TemperatureMax", typeof(int));
            table.Columns.Add("PowerOnHours", typeof(int));
            table.Columns.Add("WearLevel", typeof(int));
            table.Columns.Add("ReadLatencyMax", typeof(double));
            table.Columns.Add("WriteLatencyMax", typeof(double));
            table.Columns.Add("TS", typeof(DateTime));

            foreach (var entry in entries)
            {
                table.Rows.Add(
                    entry.ServerGUID ?? string.Empty,
                    entry.DeviceId,
                    entry.FriendlyName ?? string.Empty,
                    entry.SerialNumber ?? string.Empty,
                    entry.Model ?? string.Empty,
                    entry.MediaType ?? string.Empty,
                    entry.HealthStatus ?? string.Empty,
                    entry.SizeGB ?? string.Empty,
                    (object?)entry.Temperature ?? DBNull.Value,
                    (object?)entry.TemperatureMax ?? DBNull.Value,
                    (object?)entry.PowerOnHours ?? DBNull.Value,
                    (object?)entry.WearLevel ?? DBNull.Value,
                    entry.ReadLatencyMax,
                    entry.WriteLatencyMax,
                    entry.TS ?? DateTime.UtcNow
                );
            }

            return table;
        }

        public static DataTable CreateMirroringEntryTable(IEnumerable<MirroringData> entries)
        {
            var table = new DataTable();

            table.Columns.Add("ServerGUID", typeof(string));
            table.Columns.Add("DatabaseName", typeof(string));
            table.Columns.Add("Role", typeof(int));
            table.Columns.Add("MirroringState", typeof(int));
            table.Columns.Add("WitnessStatus", typeof(int));
            table.Columns.Add("LogGenerationRate", typeof(int));
            table.Columns.Add("UnsentLog", typeof(int));
            table.Columns.Add("SendRate", typeof(int));
            table.Columns.Add("UnrestoredLog", typeof(int));
            table.Columns.Add("RecoveryRate", typeof(int));
            table.Columns.Add("TransactionDelay", typeof(int));
            table.Columns.Add("TransactionsPerSec", typeof(int));
            table.Columns.Add("AverageDelay", typeof(int));
            table.Columns.Add("TimeRecorded", typeof(DateTime));
            table.Columns.Add("TimeBehind", typeof(DateTime));
            table.Columns.Add("LocalTime", typeof(DateTime));
            table.Columns.Add("TS", typeof(DateTime));

            foreach (var entry in entries)
            {
                table.Rows.Add(
                    entry.ServerGUID ?? string.Empty,
                    entry.DatabaseName ?? string.Empty,
                    (object?)entry.Role ?? DBNull.Value,
                    (object?)entry.MirroringState ?? DBNull.Value,
                    (object?)entry.WitnessStatus ?? DBNull.Value,
                    (object?)entry.LogGenerationRate ?? DBNull.Value,
                    (object?)entry.UnsentLog ?? DBNull.Value,
                    (object?)entry.SendRate ?? DBNull.Value,
                    (object?)entry.UnrestoredLog ?? DBNull.Value,
                    (object?)entry.RecoveryRate ?? DBNull.Value,
                    (object?)entry.TransactionDelay ?? DBNull.Value,
                    (object?)entry.TransactionsPerSec ?? DBNull.Value,
                    (object?)entry.AverageDelay ?? DBNull.Value,
                    (object?)entry.TimeRecorded ?? DBNull.Value,
                    (object?)entry.TimeBehind ?? DBNull.Value,
                    (object?)entry.LocalTime ?? DBNull.Value,
                    entry.TS ?? DateTime.UtcNow
                );
            }

            return table;
        }

        public static DataTable CreateBackupEntryTable(IEnumerable<BackupData> entries)
        {
            var table = new DataTable();
            table.Columns.Add("ServerGUID", typeof(string));
            table.Columns.Add("DatabaseName", typeof(string));
            table.Columns.Add("Type", typeof(string));
            table.Columns.Add("Date", typeof(DateTime));
            table.Columns.Add("SizeGB", typeof(string));
            table.Columns.Add("TS", typeof(DateTime));

            foreach (var entry in entries)
            {
                table.Rows.Add(
                    entry.ServerGUID,
                    entry.DatabaseName,
                    entry.Type,
                    entry.Date,
                    entry.SizeGB,
                    entry.TS ?? DateTime.UtcNow
                );
            }

            return table;
        }
    }
}
