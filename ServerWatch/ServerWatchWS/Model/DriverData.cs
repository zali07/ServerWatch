using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ServerWatchWS.Model
{
    public class DriverData
    {
        [Key]
        public int Id { get; set; }

        [JsonPropertyName("ServerName")]
        public string ServerName { get; set; }

        [JsonPropertyName("GUID")]
        public string GUID { get; set; }

        [JsonPropertyName("DeviceId")]
        public uint DeviceId { get; set; }
        
        [JsonPropertyName("UniqueId")]
        public string UniqueId { get; set; }
        
        [JsonPropertyName("FriendlyName")]
        public string FriendlyName { get; set; }
        
        [JsonPropertyName("SerialNumber")]
        public string SerialNumber { get; set; }
        
        [JsonPropertyName("Model")]
        public string Model { get; set; }

        [JsonPropertyName("MediaType")]
        public string MediaType { get; set; }

        [JsonPropertyName("HealthStatus")]
        public string HealthStatus { get; set; }

        [JsonPropertyName("SizeGB")]
        public string SizeGB { get; set; }

        [JsonPropertyName("TemperatureC")]
        public int TemperatureC { get; set; }

        [JsonPropertyName("TemperatureMaxC")]
        public int TemperatureMaxC { get; set; }

        [JsonPropertyName("PowerOnHours")]
        public int PowerOnHours { get; set; }

        [JsonPropertyName("WearLevel")]
        public int WearLevel { get; set; }

        [JsonPropertyName("ReadLatencyMax")]
        public ulong ReadLatencyMax { get; set; }
        
        [JsonPropertyName("WriteLatencyMax")]
        public ulong WriteLatencyMax { get; set; }
    }
}
