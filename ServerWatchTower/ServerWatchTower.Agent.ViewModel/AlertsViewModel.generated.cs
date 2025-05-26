//-----------------------------------------------------------------------
// <copyright file="AlertsViewModel.generated.cs" company="Cosys SRL.">
//     Copyright (c) 2012, 2025 Cosys SRL. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ServerWatchTower.Agent.ViewModel
{
    using System;
    using System.Collections.Generic;
	using System.Collections.Specialized;
    using System.ComponentModel;
    using System.ComponentModel.Composition;
    using System.Diagnostics;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Data;
    using Cosys.SilverLib.Core;
    using Cosys.SilverLib.Model;
	using ServerWatchTower.Agent.Model;

    /// <summary>
    /// The view model class of the Server Alerts.
    /// </summary>
    [Export("ServerWatchTower.Agent.AlertsViewModel"), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class AlertsViewModel : ViewModelBase
    {
        #region Private fields

        /// <summary>
        /// The store of some of the boolean properties of the view model.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private BitVector32 bitValues;

        #endregion

        #region Imports

        /// <summary>
        /// Gets or sets the configuration settings of the Agent module.
        /// </summary>
        [Import]
        public ServerWatchTower.Agent.Model.AgentConfiguration Config
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the rights of the current user in the Agent module.
        /// </summary>
        [Import]
        public ServerWatchTower.Agent.Model.AgentRights Rights
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="IAgentDataService"/> implementation to be used by the view model.
        /// </summary>
        [Import]
        public IAgentDataService AgentDataService
        {
            get;
            set;
        }

        #endregion

        #region Command properties

        /// <summary>
        /// Gets the open contract command.
        /// </summary>
        public DelegateCommand OpenContractCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the command of acknowledging and hiding an alert on the alert view.
        /// </summary>
        public DelegateCommand AcknowledgeAlertCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the command of opening the alerts history.
        /// </summary>
        public DelegateCommand OpenHistoryCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the command of refreshing the content of the view.
        /// </summary>
        public DelegateCommand RefreshCommand
        {
            get;
            private set;
        }

        #endregion

        /// <summary>
        /// Gets or sets the property which represents whether the current list in view is empty.
        /// </summary>
        public bool IsCurrentListEmpty
        {
            get => this.bitValues[1];

            private set
            {
                if (this.bitValues[1] == value)
                {
                    return;
                }

                this.bitValues[1] = value;

                this.NotifyChangeOf(nameof(this.IsCurrentListEmpty));
            }
        }

        /// <summary>
        /// Gets or sets the property which represents whether the archive is currently loading.
        /// </summary>
        public bool IsLoadingArchive
        {
            get => this.bitValues[2];

            private set
            {
                if (this.bitValues[2] == value)
                {
                    return;
                }

                this.bitValues[2] = value;

                this.NotifyChangeOf(nameof(this.IsLoadingArchive));
            }
        }

        /// <summary>
        /// Initializes the view model when it is being created, but is not yet bound to the view.
        /// </summary>
        /// <param name="initArguments">The initialization arguments, as passed to
        /// the <see cref="ISilverApplication.CreateView(string,object)"/> method.</param>
        protected override void OnInitialize(object initArguments)
        {
            this.PreInitialize();

            this.BuildCommands();

            this.Initialize();
        }

        /// <summary>
        /// Initializes the command properties by creating the commands of the <see cref="AlertsViewModel"/>.
        /// </summary>
        private void BuildCommands()
        {
            this.OpenContractCommand = new DelegateCommand(this.ExecuteOpenContract);
            this.AcknowledgeAlertCommand = new DelegateCommand(this.ExecuteAcknowledgeAlert);
            this.OpenHistoryCommand = new DelegateCommand(this.ExecuteOpenHistory);
            this.RefreshCommand = new DelegateCommand(this.ExecuteRefresh, this.CanExecuteRefresh);
        }

        /// <summary>
        /// Refreshes the availability of the commands of the view model.
        /// </summary>
        private void RefreshCommandState()
        {
            this.RefreshCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Starts loading the data of the simple catalogs for which collection properties have been defined asynchronously.
        /// </summary>
        /// <returns>The <see cref="Task"/> which will load the data of the catalogs in the background.</returns>
        private Task LoadCatalogsAsync()
        {
            return TaskHelper.CompletedTask();
        }

        /// <summary>
        /// Disposes the components held by the generated properties of the view model.
        /// </summary>
        private void DisposeComponents()
        {
        }

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void PreInitialize();

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void Initialize();

        /* Method stub: PreInitialize

        /// <summary>
        /// Checks the arguments passed to the view and the access rights of the user, before the actual initialization of the view model.
        /// </summary>
        partial void PreInitialize()
        {
            //// TODO: implement this. Note that no async operations may be started from this method.
        }

        */

        /* Method stub: Initialize

        /// <summary>
        /// Initializes the view model when it is being created, but is not yet bound to the view.
        /// </summary>
        partial void Initialize()
        {
            //// TODO: implement this. Note that no async operations may be started from this method.
        }

        */

        /* Method stub: OnLoadAsync

        /// <summary>
        /// Loads the data of the view asynchronously when it is being shown.
        /// </summary>
        /// <returns>The <see cref="Task"/> which will load the data of the view asynchronously, so the view could be displayed.</returns>
        protected override async Task OnLoadAsync()
        {
            await this.LoadCatalogsAsync();

            //// TODO: Implement this. You may call this.View.DisplayAsync() at the point where the view should be displayed.
        }

        */

        /* Method stub: Dispose

        /// <summary>
        /// Releases the unmanaged resources used by the ViewModel and removes the event handlers attached to long-lived objects.
        /// </summary>
        /// <param name="disposing">True when disposing is called explicitly; false when the method is called by the finalizer.</param>
        protected override void Dispose(bool disposing)
        {
            this.DisposeComponents();

            //// TODO: your stuff comes here

            base.Dispose(disposing);
        }

        */

        /* Command stubs: OpenContractCommand

        /// <summary>
        /// Executes the <see cref="OpenContractCommand"/> by ...
        /// </summary>
        private void ExecuteOpenContract()
        {
            //// TODO: implement this
        }

        */

        /* Command stubs: AcknowledgeAlertCommand

        /// <summary>
        /// Executes the <see cref="AcknowledgeAlertCommand"/> by ...
        /// </summary>
        private void ExecuteAcknowledgeAlert()
        {
            //// TODO: implement this
        }

        */

        /* Command stubs: OpenHistoryCommand

        /// <summary>
        /// Executes the <see cref="OpenHistoryCommand"/> by ...
        /// </summary>
        private void ExecuteOpenHistory()
        {
            //// TODO: implement this
        }

        */

        /* Command stubs: RefreshCommand

        /// <summary>
        /// Checks whether the <see cref="RefreshCommand"/> can currently be executed.
        /// </summary>
        /// <returns>True when the command can be executed; false otherwise.</returns>
        private bool CanExecuteRefresh()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Executes the <see cref="RefreshCommand"/> by ...
        /// </summary>
        private void ExecuteRefresh()
        {
            //// TODO: implement this
        }

        */
    }
}

