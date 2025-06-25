using System.ComponentModel.DataAnnotations;

namespace ServerWatchAPI.Model
{
    public class Servers
    {
        [Key]
        public int Id { get; set; }
        public required string GUID { get; set; }
        public required string PublicKey { get; set; }
        public string? Partner { get; set; }
        public string? Server { get; set; }
        public string? Windows { get; set; }
        public string? BackupRoot { get; set; }
        public int Flag { get; set; } = 0;
    }
}
