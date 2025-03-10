using Newtonsoft.Json;
using System;

namespace ServerWatchAgent.Mirroring
{
    public class MirroringData
    {
        [JsonProperty("server_name")]
        public string ServerName { get; set; }

        [JsonProperty("database_name")]
        public string DatabaseName { get; set; }

        [JsonProperty("role")]
        public int Role { get; set; }

        [JsonProperty("mirroring_state")]
        public int MirroringState { get; set; }

        [JsonProperty("witness_status")]
        public int WitnessStatus { get; set; }

        [JsonProperty("log_generation_rate")]
        public int LogGenerationRate { get; set; }

        [JsonProperty("unsent_log")]
        public int UnsentLog { get; set; }

        [JsonProperty("send_rate")]
        public int SendRate { get; set; }

        [JsonProperty("unrestored_log")]
        public int UnrestoredLog { get; set; }

        [JsonProperty("recovery_rate")]
        public int RecoveryRate { get; set; }

        [JsonProperty("transaction_delay")]
        public int TransactionDelay { get; set; }

        [JsonProperty("transactions_per_sec")]
        public int TransactionsPerSec { get; set; }

        [JsonProperty("average_delay")]
        public int AverageDelay { get; set; }

        [JsonProperty("time_recorded")]
        public DateTime TimeRecorded { get; set; }

        [JsonProperty("time_behind")]
        public DateTime TimeBehind { get; set; }

        [JsonProperty("local_time")]
        public DateTime LocalTime { get; set; }
    }
}
