namespace ServerWatchTower.Agent.ViewModel
{
    using ServerWatchTower.Agent.Model;
    using System;
    using System.Collections.Generic;
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
                //var servers = await this.AgentDataService.GetServerComponentStatusesAsync();
                //this.ServerCards.Clear();

                //foreach (var group in servers.GroupBy(s => s.ServerGuid))
                //{
                //    var displayName = group.Select(c => c.ServerName).FirstOrDefault(n => !string.IsNullOrEmpty(n)) ?? group.Key;

                //    var card = new ServerCard
                //    {
                //        ServerName = displayName,
                //        Components = group.Select(c => new ComponentStatus
                //        {
                //            ComponentName = c.ComponentName,
                //            Status = c.Status
                //        }).ToList()
                //    };
                //    this.ServerCards.Add(card);
                //}
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
                //var servers = await this.AgentDataService.GetServerComponentStatusesAsync();

                //this.ServerCards.Clear();

                //foreach (var group in servers.GroupBy(s => s.ServerGuid))
                //{
                //    var displayName = group.Select(c => c.ServerName).FirstOrDefault(n => !string.IsNullOrEmpty(n)) ?? group.Key;

                //    var card = new ServerCard
                //    {
                //        ServerName = displayName,
                //        Components = group.Select(c => new ComponentStatus
                //        {
                //            ComponentName = c.ComponentName,
                //            Status = c.Status
                //        }).ToList()
                //    };

                //    this.ServerCards.Add(card);
                //}
            }
            finally
            {
                this.IsLoading = false;
                this.NotifyChangeOf(nameof(this.IsLoading));
            }
        }
    }
}
