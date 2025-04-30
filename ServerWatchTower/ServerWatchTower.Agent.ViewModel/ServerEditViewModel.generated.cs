//-----------------------------------------------------------------------
// <copyright file="ServerEditViewModel.generated.cs" company="Cosys SRL.">
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
    /// The view model class of the Server Edit.
    /// </summary>
    [Export("ServerWatchTower.Agent.ServerEditViewModel"), PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class ServerEditViewModel : ViewModelBase
    {
        #region Private fields

        /// <summary>
        /// The store of some of the boolean properties of the view model.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private BitVector32 bitValues;

        /// <summary>
        /// The background loader which will load the partners corresponding to the filtering criteria from the database into the <see cref="Content"/> collection.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private BackgroundLoader<ServerCatalogFilterArgs, SynchronizationResult> contentLoader;

        /// <summary>
        /// The field which stores the data of the <see cref="QuickFilter"/> property. Should not be accessed directly.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string quickFilterField = "";

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
        /// Gets or sets the <see cref="ServerWatchTower.Agent.Model.ServerCatalog"/> catalog to be used by the view model, imported from MEF.
        /// </summary>
        [Import]
        public ServerWatchTower.Agent.Model.ServerCatalog Catalog
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
        /// Gets the command of refreshing the content of the catalog.
        /// </summary>
        public DelegateCommand RefreshCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the command which starts editing the currently selected item from the catalog.
        /// </summary>
        public DelegateCommand OpenItemCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the command of selecting the current item and returning it to the caller ComboBox.
        /// </summary>
        public DelegateCommand SelectItemCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the default command on the current item, which is usually bound to the item double-click event.
        /// </summary>
        public DelegateCommand DefaultItemCommand
        {
            get;
            private set;
        }

        #endregion

        /// <summary>
        /// Gets or sets the background loader which will load the partners corresponding to the filtering criteria from the database into the <see cref="Content"/> collection.
        /// </summary>
        public IBackgroundLoader ContentLoader
        {
            get => this.contentLoader;
        }

        /// <summary>
        /// Gets or sets the quick filtering condition of the <see cref="Content"/> collection view, which will only include the items which contain the text of this property.
        /// </summary>
        public string QuickFilter
        {
            get => this.quickFilterField;
            set
            {
                this.OnQuickFilterChanging(ref value);

                if (this.quickFilterField != value)
                {
                    this.quickFilterField = value;

                    this.OnQuickFilterChanged(value);

                    this.Notify(ChangeOfProperty.QuickFilter);
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the catalog has been opened in selection mode.
        /// </summary>
        public bool IsInSelectionMode
        {
            get => this.bitValues[1];

            private set
            {
                if (this.bitValues[1] == value)
                {
                    return;
                }

                this.bitValues[1] = value;

                this.Notify(ChangeOfProperty.IsInSelectionMode);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the currently selected item of the catalog can be edited.
        /// </summary>
        public bool IsItemEditable
        {
            get => this.bitValues[2];

            private set
            {
                if (this.bitValues[2] == value)
                {
                    return;
                }

                this.bitValues[2] = value;

                this.Notify(ChangeOfProperty.IsItemEditable);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="SelectablesOnly"/> filtering property is available and can be set for the current instance of the catalog.
        /// </summary>
        public bool IsSelectablesOnlyFilterAvailable
        {
            get => this.bitValues[4];

            private set
            {
                if (this.bitValues[4] == value)
                {
                    return;
                }

                this.bitValues[4] = value;

                this.Notify(ChangeOfProperty.IsSelectablesOnlyFilterAvailable);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether only the items available for selection should be displayed in the catalog.
        /// </summary>
        public bool SelectablesOnly
        {
            get => this.bitValues[8];
            set
            {
                if (this.bitValues[8] == value)
                {
                    return;
                }

                this.bitValues[8] = value;

                this.Notify(ChangeOfProperty.SelectablesOnly);

                this.OnSelectablesOnlyChanged(value);
            }
        }

        /// <summary>
        /// Gets the arguments passed to the view for initialization.
        /// </summary>
        private ServerEditViewArgs OpenArgs
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes the view model when it is being created, but is not yet bound to the view.
        /// </summary>
        /// <param name="initArguments">The initialization arguments, as passed to
        /// the <see cref="ISilverApplication.CreateView(string,object)"/> method.</param>
        protected override void OnInitialize(object initArguments)
        {
            if (initArguments != null)
            {
                if (!(initArguments is ServerEditViewArgs))
                {
                    this.NormalizeOpenArgs(ref initArguments);

                    if (initArguments != null && !(initArguments is ServerEditViewArgs))
                    {
                        throw ExceptionHelper.NewInvalidViewArgsException("ServerEdit");
                    }
                }

                this.OpenArgs = (ServerEditViewArgs)initArguments;
            }

            this.PreInitialize();

            this.BuildCommands();

            this.contentLoader = new BackgroundLoader<ServerCatalogFilterArgs, SynchronizationResult>(this.ContentLoader_LoadAsync, this.ContentLoader_ProcessResult, 400);

            this.Initialize();
        }

        /// <summary>
        /// Initializes the command properties by creating the commands of the <see cref="ServerEditViewModel"/>.
        /// </summary>
        private void BuildCommands()
        {
            this.RefreshCommand = new DelegateCommand(this.ExecuteRefresh, this.CanExecuteRefresh);
            this.OpenItemCommand = new DelegateCommand(this.ExecuteOpenItem, this.CanExecuteOpenItem);
            this.SelectItemCommand = new DelegateCommand(this.ExecuteSelectItem, this.CanExecuteSelectItem);
            this.DefaultItemCommand = new DelegateCommand(this.ExecuteDefaultItem, this.CanExecuteDefaultItem);
        }

        /// <summary>
        /// Refreshes the availability of the commands of the view model.
        /// </summary>
        private void RefreshCommandState()
        {
            this.RefreshCommand.RaiseCanExecuteChanged();
            this.OpenItemCommand.RaiseCanExecuteChanged();
            this.SelectItemCommand.RaiseCanExecuteChanged();
            this.DefaultItemCommand.RaiseCanExecuteChanged();
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
            this.contentLoader?.Dispose();
        }

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void PreInitialize();

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void Initialize();

        /// <summary>
        /// Tries to convert the initialization arguments passed to the view to the appropriate type, when it is missing or is not of the expected type.
        /// </summary>
        /// <param name="args">A reference to the arguments passed to the view, which should be converted to the appropriate type when possible.</param>
        partial void NormalizeOpenArgs(ref object args);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void OnQuickFilterChanging(ref string value);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void OnQuickFilterChanged(string value);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void OnSelectablesOnlyChanged(bool value);

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

        /* Background loader stubs: ContentLoader

        /// <summary>
        /// The data loading method of the <see cref="contentLoader"/>, which starts the background loading task.
        /// </summary>
        /// <param name="args">The arguments which specify which data has to be loaded from the database.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> through which the loading operation might get canceled by the <see cref="BackgroundLoader{TArg, TData}"/>.</param>
        /// <returns>The data loading task, which has been started in the background by this method.</returns>
        private Task<SynchronizationResult> ContentLoader_LoadAsync(ServerCatalogFilterArgs args, CancellationToken cancellationToken)
        {
            //// TODO: start and return the data loading task
        }

        /// <summary>
        /// The result processing method of the <see cref="contentLoader"/>, which updates the view model with the newly loaded data.
        /// </summary>
        /// <param name="loadingTask">The <see cref="Task{TResult}"/> which has loaded the data from the database and is now completed.</param>
        /// <param name="args">The arguments with which the loading has been initiated.</param>
        /// <seealso cref="ContentLoader_LoadAsync"/>
        private void ContentLoader_ProcessResult(Task<SynchronizationResult> loadingTask, ServerCatalogFilterArgs args)
        {
            if (loadingTask.IsFaulted)
            {
                this.Application.HandleError(loadingTask);
                return;
            }

            //// TODO: process the loaded data
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

        /* Command stubs: OpenItemCommand

        /// <summary>
        /// Checks whether the <see cref="OpenItemCommand"/> can currently be executed.
        /// </summary>
        /// <returns>True when the command can be executed; false otherwise.</returns>
        private bool CanExecuteOpenItem()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Executes the <see cref="OpenItemCommand"/> by ...
        /// </summary>
        private void ExecuteOpenItem()
        {
            //// TODO: implement this
        }

        */

        /* Command stubs: SelectItemCommand

        /// <summary>
        /// Checks whether the <see cref="SelectItemCommand"/> can currently be executed.
        /// </summary>
        /// <returns>True when the command can be executed; false otherwise.</returns>
        private bool CanExecuteSelectItem()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Executes the <see cref="SelectItemCommand"/> by ...
        /// </summary>
        private void ExecuteSelectItem()
        {
            //// TODO: implement this
        }

        */

        /* Command stubs: DefaultItemCommand

        /// <summary>
        /// Checks whether the <see cref="DefaultItemCommand"/> can currently be executed.
        /// </summary>
        /// <returns>True when the command can be executed; false otherwise.</returns>
        private bool CanExecuteDefaultItem()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Executes the <see cref="DefaultItemCommand"/> by ...
        /// </summary>
        private void ExecuteDefaultItem()
        {
            //// TODO: implement this
        }

        */
    }
}

