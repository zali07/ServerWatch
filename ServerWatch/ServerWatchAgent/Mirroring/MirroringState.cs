namespace ServerWatchAgent.Mirroring
{
    /// <summary>
    /// The state of the mirroring returned by the monitor results.
    /// </summary>
    public enum MirroringState
    {
        Suspended = 0,
        Disconnected = 1,
        Synchronizing = 2,
        PendingFailover = 3,
        Synchronized = 4
    }
}
