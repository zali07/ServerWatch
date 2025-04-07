using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ServerWatchWS.Model
{
    public class DriverData
    {
        [Key]
        public int Id { get; set; }

        public string? ServerGUID { get; set; }

        [JsonPropertyName("DeviceId")]
        public uint DeviceId { get; set; }
        
        [JsonPropertyName("FriendlyName")]
        public string? FriendlyName { get; set; }
        
        [JsonPropertyName("SerialNumber")]
        public string? SerialNumber { get; set; }
        
        [JsonPropertyName("Model")]
        public string? Model { get; set; }

        [JsonPropertyName("MediaType")]
        public string? MediaType { get; set; }

        [JsonPropertyName("HealthStatus")]
        public string? HealthStatus { get; set; }

        [JsonPropertyName("SizeGB")]
        public string? SizeGB { get; set; }

        [JsonPropertyName("Temperature")]
        public int? Temperature { get; set; }

        [JsonPropertyName("TemperatureMax")]
        public int? TemperatureMax { get; set; }

        [JsonPropertyName("PowerOnHours")]
        public int? PowerOnHours { get; set; }

        [JsonPropertyName("WearLevel")]
        public int? WearLevel { get; set; }

        [JsonPropertyName("ReadLatencyMax")]
        public ulong? ReadLatencyMax { get; set; }
        
        [JsonPropertyName("WriteLatencyMax")]
        public ulong? WriteLatencyMax { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TS { get; set; }
    }
}
