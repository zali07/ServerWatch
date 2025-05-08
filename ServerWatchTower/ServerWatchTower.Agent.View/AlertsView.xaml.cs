namespace ServerWatchTower.Agent.View
{
    using System.ComponentModel.Composition;
    using System.Diagnostics;

    /// <summary>
    /// The view of Alerts. 
    /// </summary>
    [Export("ServerWatchTower.Agent.AlertsView"), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class AlertsView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AlertsView"/> class.
        /// </summary>
        public AlertsView()
        {
            this.InitializeComponent();
        }

        private void CosysLink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
