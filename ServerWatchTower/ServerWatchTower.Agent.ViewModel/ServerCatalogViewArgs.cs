namespace ServerWatchTower.Agent.ViewModel
{
    using System.ComponentModel.Composition;
    using ServerWatchTower.Agent.Model;
    using Cosys.SilverLib.Model;

    /// <summary>
    /// Class containing the arguments which can be passed to the Product mapping catalog when opening it.
    /// </summary>
    [Export("ServerWatchTower.Agent.ServersViewArgs"), PartCreationPolicy(CreationPolicy.NonShared)]
    public class ServerCatalogViewArgs
    {
        /// <summary>
        /// Gets or sets the base filtering to be used by the catalog.
        /// </summary>
        /// <remarks>
        /// <para>The <see cref="CatalogFilterArgs{TData}.QuickFilter"/> property of the filter will be ignored.</para>
        /// </remarks>
        public ServerCatalogFilterArgs BaseFilter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the catalog should be opened in selection mode.
        /// </summary>
        public bool SelectionMode { get; set; }

        /// <summary>
        /// Gets or sets the optional <see cref="PartnerMapping"/>.<see cref="PartnerMapping.Code"/> which
        /// should initially be selected in the catalog.
        /// </summary>
        public string SelectedServerGUID { get; set; }
    }
}
