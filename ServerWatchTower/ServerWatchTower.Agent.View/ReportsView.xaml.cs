namespace ServerWatchTower.Agent.View
{
    using System.ComponentModel.Composition;

    /// <summary>
    /// The view of Reports. 
    /// </summary>
    [Export("ServerWatchTower.Agent.ReportsView"), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class ReportsView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportsView"/> class.
        /// </summary>
        public ReportsView()
        {
            this.InitializeComponent();
        }
    }
}
