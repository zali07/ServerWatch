using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ServerWatchAPI.Model
{
    public class DriverData
    {
        [Key]
        public int Id { get; set; }

        public string ServerGUID { get; set; }

        [JsonPropertyName("DeviceId")]
        public long DeviceId { get; set; }

        [JsonPropertyName("UniqueId")]
        public string UniqueId { get; set; }

        [JsonPropertyName("FriendlyName")]
        public string FriendlyName { get; set; }
        
        [JsonPropertyName("SerialNumber")]
        public string SerialNumber { get; set; }
        
        [JsonPropertyName("Model")]
        public string Model { get; set; }

        [JsonPropertyName("BusType")]
        public string BusType { get; set; }

        [JsonPropertyName("MediaType")]
        public string MediaType { get; set; }

        [JsonPropertyName("HealthStatus")]
        public string HealthStatus { get; set; }

        [JsonPropertyName("SizeGB")]
        public string SizeGB { get; set; }

        [JsonPropertyName("Temperature")]
        public int? Temperature { get; set; }

        [JsonPropertyName("TemperatureMax")]
        public int? TemperatureMax { get; set; }

        [JsonPropertyName("PowerOnHours")]
        public int? PowerOnHours { get; set; }

        [JsonPropertyName("WearLevel")]
        public int? WearLevel { get; set; }

        [Column(TypeName = "decimal(20,0)")]
        [JsonPropertyName("ReadLatencyMax")]
        public decimal ReadLatencyMax { get; set; }

        [Column(TypeName = "decimal(20,0)")]
        [JsonPropertyName("WriteLatencyMax")]
        public decimal WriteLatencyMax { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TS { get; set; }
    }
}
