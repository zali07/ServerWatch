namespace ServerWatchTower.Agent.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Threading;

    /// <summary>
    /// The view model for the Reports view.
    /// </summary>
    public partial class ReportsViewModel
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
            this.ServerCards = new ObservableCollection<ServerCard>();

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
                var servers = await this.AgentDataService.GetServerComponentStatusesAsync();
                this.ServerCards.Clear();

                foreach (var group in servers.GroupBy(s => s.ServerGuid))
                {
                    var displayName = group.Select(c => c.ServerName).FirstOrDefault(n => !string.IsNullOrEmpty(n)) ?? group.Key;

                    var card = new ServerCard
                    {
                        ServerName = displayName,
                        Components = group.Select(c => new ComponentStatus
                        {
                            ComponentName = c.ComponentName,
                            Status = c.Status
                        }).ToList()
                    };
                    this.ServerCards.Add(card);
                }
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
                var servers = await this.AgentDataService.GetServerComponentStatusesAsync();

                this.ServerCards.Clear();

                foreach (var group in servers.GroupBy(s => s.ServerGuid))
                {
                    var displayName = group.Select(c => c.ServerName).FirstOrDefault(n => !string.IsNullOrEmpty(n)) ?? group.Key;

                    var card = new ServerCard
                    {
                        ServerName = displayName,
                        Components = group.Select(c => new ComponentStatus
                        {
                            ComponentName = c.ComponentName,
                            Status = c.Status
                        }).ToList()
                    };

                    this.ServerCards.Add(card);
                }
            }
            finally
            {
                this.IsLoading = false;
                this.NotifyChangeOf(nameof(this.IsLoading));
            }
        }
    }

    /// <summary>
    /// Represents a server card with its name and component statuses.
    /// </summary>
    public class ServerCard
    {
        public string ServerName { get; set; }
        public List<ComponentStatus> Components { get; set; }
    }

    public class ComponentStatus
    {
        public string ComponentName { get; set; }
        public string Status { get; set; } // "Red - Critical", "Orange - Warning", "Green - OK"
    }
}
