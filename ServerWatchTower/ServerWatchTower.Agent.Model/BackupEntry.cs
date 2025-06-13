using System;

namespace ServerWatchTower.Agent.Model
{
    public class BackupEntry
    {
        public int Id { get; set; }
        public string ServerGUID { get; set; }
        public string DatabaseName { get; set; }
        public string Type { get; set; } // Daily, Weekly
        public DateTime? Date { get; set; }
        public string SizeGB { get; set; }
        public DateTime? TS { get; set; }
    }
}
