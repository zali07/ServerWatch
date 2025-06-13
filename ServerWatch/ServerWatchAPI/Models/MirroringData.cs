using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ServerWatchAPI.Model
{
    public class MirroringData
    {
        [Key]
        public int Id { get; set; }

        public string ServerGUID { get; set; }

        [JsonPropertyName("database_name")]
        public string DatabaseName { get; set; }

        [JsonPropertyName("role")]
        public int? Role { get; set; }

        [JsonPropertyName("mirroring_state")]
        public int? MirroringState { get; set; }

        [JsonPropertyName("witness_status")]
        public int? WitnessStatus { get; set; }

        [JsonPropertyName("log_generation_rate")]
        public int? LogGenerationRate { get; set; }

        [JsonPropertyName("unsent_log")]
        public int? UnsentLog { get; set; }

        [JsonPropertyName("send_rate")]
        public int? SendRate { get; set; }

        [JsonPropertyName("unrestored_log")]
        public int? UnrestoredLog { get; set; }

        [JsonPropertyName("recovery_rate")]
        public int? RecoveryRate { get; set; }

        [JsonPropertyName("transaction_delay")]
        public int? TransactionDelay { get; set; }

        [JsonPropertyName("transactions_per_sec")]
        public int? TransactionsPerSec { get; set; }

        [JsonPropertyName("average_delay")]
        public int? AverageDelay { get; set; }

        [Column(TypeName = "smalldatetime")]
        [JsonPropertyName("time_recorded")]
        public DateTime? TimeRecorded { get; set; }

        [Column(TypeName = "smalldatetime")]
        [JsonPropertyName("time_behind")]
        public DateTime? TimeBehind { get; set; }

        [Column(TypeName = "smalldatetime")]
        [JsonPropertyName("local_time")]
        public DateTime? LocalTime { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TS { get; set; }
    }
}
