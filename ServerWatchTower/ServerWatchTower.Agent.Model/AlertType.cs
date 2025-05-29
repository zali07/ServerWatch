namespace ServerWatchTower.Agent.Model
{
    /// <summary>
    /// The enumeration of the types of alerts which can be displayed on the alert view.
    /// </summary>
    public enum AlertType
    {
        /// <summary>
        /// The alert contains an informational message.
        /// </summary>
        Info = 'I',

        /// <summary>
        /// The alert contains a warning message about something that needs attention.
        /// </summary>
        Warning = 'W',

        /// <summary>
        /// The alert contains an error message about something that needs to be fixed.
        /// </summary>
        Error = 'E',
    }
}
