namespace ServerWatchTower.Agent.Model
{
    /// <summary>
    /// Represents the status of a component of a server.
    /// </summary>
    public class ServerComponentStatus
    {
        public string ServerGuid { get; set; }
        public string ServerName { get; set; }
        public string ComponentName { get; set; }
        public string Status { get; set; } // "OK", "Warning", "Critical"
    }
}
