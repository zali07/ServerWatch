namespace ServerWatchTower.Agent
{
    using Cosys.SilverLib.Core;
    using ServerWatchTower.Agent.Model;

    /// <summary>
    /// Class which defines the Registry module and its components.
    /// </summary>
    public partial class AgentModule : ModuleBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AgentModule"/> class.
        /// </summary>
        public AgentModule()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Update the switchboard of the module when it got loaded.
        /// </summary>
        protected override void OnLoaded()
        {
            var config = this.Application.ComposeObject<AgentConfiguration>();

            if (config != null)
            {
                
            }

            base.OnLoaded();
        }
    }
}
