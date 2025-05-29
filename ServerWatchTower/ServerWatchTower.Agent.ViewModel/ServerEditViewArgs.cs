namespace ServerWatchTower.Agent.ViewModel
{
    using System.ComponentModel.Composition;
    using ServerWatchTower.Agent.Model;
    using Cosys.SilverLib.Model;

    /// <summary>
    /// Class containing the arguments which can be passed to the Product mapping catalog when opening it.
    /// </summary>
    [Export("ServerWatchTower.Agent.ServerEditViewArgs"), PartCreationPolicy(CreationPolicy.NonShared)]
    public class ServerEditViewArgs
    {
        /// <summary>
        /// Gets or sets the optional <see cref="PartnerMapping"/>.<see cref="PartnerMapping.Code"/> which
        /// should initially be selected in the catalog.
        /// </summary>
        public string SelectedServerGUID { get; set; }
    }
}
