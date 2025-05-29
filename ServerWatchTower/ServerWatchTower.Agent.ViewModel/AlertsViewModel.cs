namespace ServerWatchTower.Agent.ViewModel
{
    using Cosys.SilverLib.Core;
    using Cosys.SilverLib.Shell;
    using ServerWatchTower.Agent.Model;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using System.Windows.Threading;

    /// <summary>
    /// The view model for the Alerts view.
    /// </summary>
    public partial class AlertsViewModel
    {
        /// <summary>
        /// Represents the different views that can be displayed on the alert view.
        /// </summary>
        private enum State
        {
            /// <summary>
            /// The Alerts view which contains the active alerts.
            /// </summary>
            Alerts = 0, // Default

            /// <summary>
            /// The history view which contains the archived alerts. (acknowledged or expired alerts)
            /// </summary>
            History,
        }

        /// <summary>
        /// A timer which used to refreshes the data of the alerts every 5 min.
        /// </summary>
        private DispatcherTimer refreshTimer;

        /// <summary>
        /// Gets or sets the property which represents whether the alerts should be shown or not.
        /// </summary>
        public bool ShowAlerts => this.viewState == State.Alerts;

        /// <summary>
        /// Gets or sets the property which represents whether the archived alerts should be shown or not.
        /// </summary>
        public bool ShowHistory => this.viewState == State.History;

        /// <summary>
        /// Gets a collection view with the alerts the operation would be executed on.
        /// </summary>
        public CatalogCollectionView Alerts { get; set; }

        /// <summary>
        /// The collection wrapper under the <see cref="Alerts"/> <c>CollectionView</c>.
        /// </summary>
        private CollectionWrapper<Alert> alertsWrapper;

        /// <summary>
        /// The field which stores the data of the <see cref="ViewState"/> property. Should not be accessed directly.
        /// </summary>
        private State viewState;

        /// <summary>
        /// Gets or sets the view switch of the alerts view.
        /// </summary>
        private State ViewState
        {
            get => this.viewState;
            set
            {
                if (this.viewState != value)
                {
                    this.viewState = value;
                    this.NotifyChangeOf(nameof(this.ShowAlerts));
                    this.NotifyChangeOf(nameof(this.ShowHistory));
                }
            }
        }

        /// <summary>
        /// Cache for the active alerts.
        /// </summary>
        private List<Alert> activeAlerts;

        /// <inheritdoc/>
        protected override void OnIsLoadingChanged()
        {
            this.RefreshCommand.RaiseCanExecuteChanged();
        }

        /// <inheritdoc/>
        partial void PreInitialize()
        {
            this.alertsWrapper = new CollectionWrapper<Alert>();
            this.Alerts = new CatalogCollectionView(this.alertsWrapper);
            using (this.Alerts.DeferRefresh())
            {
                this.Alerts.Filter += this.ContentFilter;
                this.Alerts.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));
                this.Alerts.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Descending));
            }

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

            this.Alerts?.Dispose();

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
                this.Application.HandleError(this.RefreshData(true));
            }
        }

        /// <inheritdoc/>
        protected override async Task OnLoadAsync()
        {
            this.IsLoading = true;

            var alerts = await this.AgentDataService.GetAlertsAsync();
            this.UpdateAlertCollection(alerts);
            this.ViewState = State.Alerts;

            this.IsLoading = false;
        }

        /// <summary>
        /// Reloads the data of the alerts view asynchronously.
        /// </summary>
        private async Task RefreshData(bool automaticRefresh = false)
        {
            if (automaticRefresh && this.ViewState != State.Alerts)
            {
                return;
            }

            this.IsLoading = true;

            try
            {
                List<Alert> alerts = null;

                switch (this.ViewState)
                {
                    case State.History:
                        alerts = await this.AgentDataService.GetAlertsHistoryAsync();
                        break;

                    case State.Alerts:
                        alerts = await this.AgentDataService.GetAlertsAsync();
                        this.activeAlerts = alerts;
                        break;

                    default:
                        break;
                }

                this.UpdateAlertCollection(alerts);
            }
            catch (Exception ex)
            {
                // throw exception if the refresh is not automatic
                if (!automaticRefresh)
                {
                    throw ex;
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
        private void ExecuteRefresh()
        {
            if (this.CanExecuteRefresh())
            {
                this.Application.HandleError(this.RefreshData());
            }
        }

        /// <summary>
        /// Executes the <see cref="AcknowledgeAlertCommand"/> by saving the acknowledgment of the alert into 
        /// the database and hiding the alert.
        /// </summary>
        /// <param name="parameter">The alert to be acknowledged.</param>
        private async void ExecuteAcknowledgeAlert(object parameter)
        {
            if (parameter is Alert alert)
            {
                if (alert.AcknowledgedOn.HasValue)
                {
                    throw new InvalidOperationException("The alert has already been acknowledged.");
                }

                await this.AgentDataService.AcknowledgeAlertAsync(alert.Id);

                this.ExecuteRefresh();

                this.IsCurrentListEmpty = this.Alerts.Count == 0;
            }
        }

        /// <summary>
        /// Executes the <see cref="OpenContractCommand"/> which opens a new view from the given contract. (shortcut)
        /// </summary>
        private void ExecuteOpenContract(object parameter)
        {
            string contract = parameter as string;

            var view = SilverApplication.Current.FindView(contract, null);

            if (view != null)
            {
                view.Activate();
            }
            else
            {
                view = SilverApplication.Current.CreateView(contract);

                view.Show();
            }
        }

        /// <summary>
        /// Executes the <see cref="OpenHistoryCommand"/> command which shows the history, if executed again
        /// the view shows the alerts again.
        /// </summary>
        private async void ExecuteOpenHistory(object sender)
        {
            this.IsLoading = true;
            this.IsLoadingArchive = true;

            try
            {
                List<Alert> alerts;

                if (this.ViewState != State.History)
                {
                    alerts = await this.AgentDataService.GetAlertsHistoryAsync();
                    this.ViewState = State.History;
                }
                else
                {
                    if (this.activeAlerts == null)
                    {
                        this.activeAlerts = await this.AgentDataService.GetAlertsAsync();
                    }
                    alerts = this.activeAlerts; // this isn't changing
                    this.ViewState = State.Alerts;
                }

                this.UpdateAlertCollection(alerts);
            }
            finally
            {
                this.IsLoadingArchive = false;
                this.IsLoading = false;
            }
        }

        /// <summary>
        /// Updates the AlertCollection with the given list of alerts.
        /// </summary>
        private void UpdateAlertCollection(List<Alert> alerts)
        {
            this.alertsWrapper.InnerCollection = alerts;
            this.IsCurrentListEmpty = this.Alerts.Count == 0;
        }

        /// <summary>
        /// Checks whether an item should be included into the <see cref="Alerts"/> collection view.
        /// </summary>
        /// <param name="item">The item to check whether to be included into the <see cref="Alerts"/> collection.</param>
        /// <returns>True when the item should be included, false otherwise.</returns>
        private bool ContentFilter(object item)
        {
            return item is Alert alert
                && (alert.AccessRights == null || alert.AccessRights.Length == 0
                    || this.SessionInfo.HasAnyRight(alert.AccessRights));
        }
    }
}
