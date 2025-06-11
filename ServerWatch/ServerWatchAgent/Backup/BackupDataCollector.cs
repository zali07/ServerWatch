using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ServerWatchAgent.Backup
{
    public class BackupDataCollector
    {
        private static string backupFolderPath;

        public BackupDataCollector(string initialBackupFolderPath = null)
        {
            backupFolderPath = initialBackupFolderPath;
        }

        public class BackupCheckResult
        {
            public string DatabaseName { get; set; }
            public string Type { get; set; } // "Daily" or "Weekly"
            public DateTime Date { get; set; }
            public string SizeGB { get; set; }
        }

        public static async Task<List<BackupCheckResult>> BackupCheckAndGetResultAsync()
        {
            var today = DateTime.Today;
            if (today.DayOfWeek == DayOfWeek.Monday)
            {
                return await CheckWeeklyBackupResultAsync(today);
            }
            else
            {
                return await CheckDailyBackupResultAsync(today);
            }
        }

        private static Task<List<BackupCheckResult>> CheckDailyBackupResultAsync(DateTime date)
        {
            string dailyFolder = Path.Combine(backupFolderPath, "Daily");
            return CheckBackupExistsResultAsync(dailyFolder, date, "Daily");
        }

        private static Task<List<BackupCheckResult>> CheckWeeklyBackupResultAsync(DateTime date)
        {
            string weeklyFolder = Path.Combine(backupFolderPath, "Weekly");
            return CheckBackupExistsResultAsync(weeklyFolder, date, "Weekly");
        }

        private static Task<List<BackupCheckResult>> CheckBackupExistsResultAsync(string folder, DateTime date, string type)
        {
            var results = new List<BackupCheckResult>();

            if (!Directory.Exists(folder))
            {
                return Task.FromResult(results);
            }

            var dateString = date.ToString("yyyy_MM_dd");
            var subFolders = Directory.GetDirectories(folder);

            foreach (var dbFolder in subFolders)
            {
                var dbName = Path.GetFileName(dbFolder);

                var bakFiles = Directory.GetFiles(
                    dbFolder,
                    $"{dbName}_backup_{dateString}_*.bak"
                );

                if (bakFiles.Length == 0)
                {
                    results.Add(new BackupCheckResult
                    {
                        DatabaseName = dbName,
                        Type = type,
                        Date = date,
                        SizeGB = "0"
                    });
                    continue;
                }

                string lastBakFile = null;
                DateTime? lastTime = null;

                foreach (var file in bakFiles)
                {
                    var fileName = Path.GetFileNameWithoutExtension(file);

                    var parts = fileName.Split('_');

                    if (parts.Length < 8) continue;

                    // [dbName, backup, yyyy, MM, dd, HH, mm, ss]
                    if (int.TryParse(parts[5], out int hour) &&
                        int.TryParse(parts[6], out int minute) &&
                        int.TryParse(parts[7], out int second))
                    {
                        var fileDateTime = new DateTime(date.Year, date.Month, date.Day, hour, minute, second);

                        if (!lastTime.HasValue || fileDateTime > lastTime.Value)
                        {
                            lastTime = fileDateTime;
                            lastBakFile = file;
                        }
                    }
                }

                if (lastBakFile != null && lastTime.HasValue &&
                    lastTime.Value.Hour == 6 &&
                    lastTime.Value.Minute >= 0 &&
                    lastTime.Value.Minute <= 10) // Accept backups from 6:00 to 6:10
                {
                    double size = new FileInfo(lastBakFile).Length;

                    results.Add(new BackupCheckResult
                    {
                        DatabaseName = dbName,
                        Type = type,
                        Date = date,
                        SizeGB = (size / (1024.0 * 1024.0 * 1024.0)).ToString("F2")
                    });
                }
                else
                {
                    results.Add(new BackupCheckResult
                    {
                        DatabaseName = dbName,
                        Type = type,
                        Date = date,
                        SizeGB = "0"
                    });
                }
            }

            return Task.FromResult(results);
        }

        public static void UpdateBackupFolderPath(string newFolder)
        {
            if (!string.IsNullOrWhiteSpace(newFolder))
            {
                backupFolderPath = newFolder;
            }
        }
    }
}
