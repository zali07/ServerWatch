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
                    if (parts.Length < 7) continue;

                    // parts[0] = dbName
                    // parts[1] = "backup"
                    // parts[2] = yyyy
                    // parts[3] = MM
                    // parts[4] = dd
                    // parts[5] = HHmmss
                    // parts[6] = (rest/unique)

                    string year = parts[2];
                    string month = parts[3];
                    string day = parts[4];
                    string time = parts[5];

                    if (time.Length != 6) continue;

                    if (int.TryParse(year, out int y) &&
                        int.TryParse(month, out int m) &&
                        int.TryParse(day, out int d) &&
                        int.TryParse(time.Substring(0, 2), out int hour) &&
                        int.TryParse(time.Substring(2, 2), out int minute) &&
                        int.TryParse(time.Substring(4, 2), out int second))
                    {
                        DateTime fileDateTime;
                        try
                        {
                            fileDateTime = new DateTime(y, m, d, hour, minute, second);
                        }
                        catch
                        {
                            continue;
                        }

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
                        Date = lastTime.Value,
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
