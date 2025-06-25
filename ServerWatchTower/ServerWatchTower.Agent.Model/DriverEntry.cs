using System;

namespace ServerWatchTower.Agent.Model
{
    public class DriverEntry
    {
        public int Id { get; set; }
        public string ServerGUID { get; set; }
        public long? DeviceId { get; set; }
        public string FriendlyName { get; set; }
        public string SerialNumber { get; set; }
        public string Model { get; set; }
        public string MediaType { get; set; }
        public string HealthStatus { get; set; }
        public string SizeGB { get; set; }
        public int? Temperature { get; set; }
        public int? TemperatureMax { get; set; }
        public int? PowerOnHours { get; set; }
        public int? WearLevel { get; set; }
        public decimal? ReadLatencyMax { get; set; }
        public decimal? WriteLatencyMax { get; set; }
        public DateTime? TS { get; set; }
    }
}
