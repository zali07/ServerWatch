using System;

namespace ServerWatchTower.Agent.Model
{
    public class MirroringEntry
    {
        public int Id { get; set; }
        public string ServerGUID { get; set; }
        public string DatabaseName { get; set; }
        public int? Role { get; set; }
        public int? MirroringState { get; set; }
        public int? WitnessStatus { get; set; }
        public int? LogGenerationRate { get; set; }
        public int? UnsentLog { get; set; }
        public int? SendRate { get; set; }
        public int? UnrestoredLog { get; set; }
        public int? RecoveryRate { get; set; }
        public int? TransactionDelay { get; set; }
        public int? TransactionsPerSec { get; set; }
        public int? AverageDelay { get; set; }
        public DateTime? TimeRecorded { get; set; }
        public DateTime? TimeBehind { get; set; }
        public DateTime? LocalTime { get; set; }
        public DateTime? TS { get; set; }
    }
}
