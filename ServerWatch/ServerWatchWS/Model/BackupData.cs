using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ServerWatchAPI.Model
{
    public class BackupData
    {
        [Key]
        public int Id { get; set; }

        public string? ServerGUID { get; set; }

        [JsonPropertyName("DatabaseName")]
        public string? DatabaseName { get; set; }

        [JsonPropertyName("Type")]
        public string? Type { get; set; }

        [JsonPropertyName("Date")]
        public DateTime? Date { get; set; }

        [JsonPropertyName("SizeGB")]
        public string? SizeGB { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TS { get; set; }
    }
}
