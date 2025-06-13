using ServerWatchAPI.Model;
using System;
using System.Collections.Generic;
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
                    entry.Temperature.HasValue ? (object)entry.Temperature.Value : DBNull.Value,
                    entry.TemperatureMax.HasValue ? (object)entry.TemperatureMax.Value : DBNull.Value,
                    entry.PowerOnHours.HasValue ? (object)entry.PowerOnHours.Value : DBNull.Value,
                    entry.WearLevel.HasValue ? (object)entry.WearLevel.Value : DBNull.Value,
                    entry.ReadLatencyMax,
                    entry.WriteLatencyMax,
                    entry.TS.HasValue ? entry.TS.Value : DateTime.UtcNow
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
                    entry.Role.HasValue ? (object)entry.Role.Value : DBNull.Value,
                    entry.MirroringState.HasValue ? (object)entry.MirroringState.Value : DBNull.Value,
                    entry.WitnessStatus.HasValue ? (object)entry.WitnessStatus.Value : DBNull.Value,
                    entry.LogGenerationRate.HasValue ? (object)entry.LogGenerationRate.Value : DBNull.Value,
                    entry.UnsentLog.HasValue ? (object)entry.UnsentLog.Value : DBNull.Value,
                    entry.SendRate.HasValue ? (object)entry.SendRate.Value : DBNull.Value,
                    entry.UnrestoredLog.HasValue ? (object)entry.UnrestoredLog.Value : DBNull.Value,
                    entry.RecoveryRate.HasValue ? (object)entry.RecoveryRate.Value : DBNull.Value,
                    entry.TransactionDelay.HasValue ? (object)entry.TransactionDelay.Value : DBNull.Value,
                    entry.TransactionsPerSec.HasValue ? (object)entry.TransactionsPerSec.Value : DBNull.Value,
                    entry.AverageDelay.HasValue ? (object)entry.AverageDelay.Value : DBNull.Value,
                    entry.TimeRecorded.HasValue ? (object)entry.TimeRecorded.Value : DBNull.Value,
                    entry.TimeBehind.HasValue ? (object)entry.TimeBehind.Value : DBNull.Value,
                    entry.LocalTime.HasValue ? (object)entry.LocalTime.Value : DBNull.Value,
                    entry.TS.HasValue ? entry.TS.Value : DateTime.UtcNow
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
                    entry.ServerGUID ?? string.Empty,
                    entry.DatabaseName ?? string.Empty,
                    entry.Type ?? string.Empty,
                    entry.Date,
                    entry.SizeGB ?? string.Empty,
                    entry.TS.HasValue ? entry.TS.Value : DateTime.UtcNow
                );
            }

            return table;
        }
    }
}
