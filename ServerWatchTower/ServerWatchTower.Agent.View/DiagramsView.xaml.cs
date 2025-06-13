namespace ServerWatchTower.Agent.View
{
    using System.ComponentModel.Composition;

    /// <summary>
    /// The view of Diagrams. 
    /// </summary>
    [Export("ServerWatchTower.Agent.DiagramsView"), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class DiagramsView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DiagramsView"/> class.
        /// </summary>
        public DiagramsView()
        {
            this.InitializeComponent();
        }
    }
}
