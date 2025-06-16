namespace ServerWatchTower.Agent.ViewModel
{
    using ServerWatchTower.Agent.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Threading;

    /// <summary>
    /// The view model for the Diagrams view.
    /// </summary>
    public partial class DiagramsViewModel
    {
        /// <summary>
        /// A timer which used to refreshes the data of the alerts every 5 min.
        /// </summary>
        private DispatcherTimer refreshTimer;

        public List<string> AvailableDiagramTypes { get; set; } = new List<string> { "DriversTemperature", "DriversReadLatency", "DriversWriteLatency", "Mirroring", "Backups" };

        private string _selectedDiagramType;
        public string SelectedDiagramType
        {
            get => _selectedDiagramType;
            set
            {
                _selectedDiagramType = value;
                _ = this.LoadChartDataAsync();
            }
        }

        public DateTime StartDate { get; set; } = DateTime.Today.AddDays(-7);
        public DateTime EndDate { get; set; } = DateTime.Today;
        public Server SelectedServer { get; set; }

        public Action<IEnumerable<IGrouping<string, ChartDataPoint>>> UpdateChartCallback { get; set; }

        private async Task LoadChartDataAsync()
        {
            if (this.SelectedServer == null || string.IsNullOrEmpty(this.SelectedDiagramType))
                return;

            var rawPoints = await this.AgentDataService.GetDiagramDataAsync(
                this.SelectedServer.GUID,
                this.SelectedDiagramType,
                this.StartDate,
                this.EndDate);

            var groupedData = rawPoints.GroupBy(p => p.Category);

            this.UpdateChartCallback?.Invoke(groupedData);
        }

        /// <inheritdoc/>
        protected override void OnIsLoadingChanged()
        {
            this.RefreshCommand.RaiseCanExecuteChanged();
        }

        /// <inheritdoc/>
        partial void PreInitialize()
        {
            this.MirroringEntries = new List<MirroringEntry>();
            this.DriverEntries = new List<DriverEntry>();
            this.BackupEntries = new List<BackupEntry>();

            this.refreshTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMinutes(5)
            };
            this.refreshTimer.Tick += this.OnRefreshTimerTick;
            this.refreshTimer.Start();
        }

        protected override void Dispose(bool disposing)
        {
            this.refreshTimer?.Stop();

            base.Dispose(disposing);
        }

        /// <summary>
        /// Method to be called when the timer ticks
        /// </summary>
        private void OnRefreshTimerTick(object sender, EventArgs e)
        {
            if (this.IsDisposed)
            {
                return;
            }

            if (this.CanExecuteRefresh())
            {
                this.ExecuteRefresh();
            }
        }

        /// <inheritdoc/>
        protected override async Task OnLoadAsync()
        {
            this.IsLoading = true;

            try
            {
                this.Servers = new List<Server>();
                var serverResult = await this.AgentDataService.GetServersAsync(new ServerCatalogFilterArgs());
                this.Servers = serverResult.Servers;

                this.NotifyChangeOf(nameof(this.Servers));
            }
            finally
            {
                this.IsLoading = false;
            }
        }

        /// <summary>
        /// Checks whether the <see cref="RefreshCommand"/> can currently be executed.
        /// </summary>
        /// <returns>True when the command can be executed; false otherwise.</returns>
        private bool CanExecuteRefresh()
        {
            return !this.IsLoading;
        }

        /// <summary>
        /// Executes the <see cref="RefreshCommand"/> by refreshing the content displayed on the view.
        /// </summary>
        private async void ExecuteRefresh()
        {
            if (this.IsLoading) return;

            this.IsLoading = true;
            this.NotifyChangeOf(nameof(this.IsLoading));

            try
            {

            }
            finally
            {
                this.IsLoading = false;
                this.NotifyChangeOf(nameof(this.IsLoading));
            }
        }
    }
}
