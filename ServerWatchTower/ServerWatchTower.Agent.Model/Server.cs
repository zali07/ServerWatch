namespace ServerWatchTower.Agent.Model
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    /// <summary>
    /// Freezable data class containing the main data of a server.
    /// </summary>
    public partial class Server
    {
        /// <summary>
        /// Checks whether the specified <see cref="Server"/> keys are equal.
        /// </summary>
        /// <param name="key1">The first key.</param>
        /// <param name="key2">The second key.</param>
        /// <returns>True when the specified keys are equal according to the appropriate equality comparer.</returns>
        public static bool AreKeysEqual(string key1, string key2)
        {
            return ServerCatalog.KeyComparer.Equals(key1, key2);
        }

        /// <summary>
        /// Gets the localized indicator letters of the <see cref="Flag"/>.
        /// </summary>
        /// <remarks>
        /// <para>This property is initialized by the <see cref="PersonCatalog"/> when it is loading its content.</para>
        /// </remarks>
        [IgnoreDataMember, EditorBrowsable(EditorBrowsableState.Never)]
        public string FlagIndicators
        {
            get;
            internal set;
        }
    }
}
