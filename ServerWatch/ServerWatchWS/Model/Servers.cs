using System.ComponentModel.DataAnnotations;

namespace ServerWatchWS.Model
{
    public class Servers
    {
        [Key]
        public int Id { get; set; }
        public string GUID { get; set; }
        public string PublicKey { get; set; }
        public string? Partner { get; set; }
        public string? Server { get; set; }
        public string? Windows { get; set; }
        public bool IsApproved { get; set; }
    }
}
