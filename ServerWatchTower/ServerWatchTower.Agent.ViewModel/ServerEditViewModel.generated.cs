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
        /// The field which stores the data of the <see cref="Item"/> property. Should not be accessed directly.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ServerE itemField;

        /// <summary>
        /// The field which stores the data of the <see cref="ViewPanelIndex"/> property. Should not be accessed directly.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int viewPanelIndexField;

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

        /// <summary>
        /// Gets or sets the <see cref="ServerWatchTower.Agent.Model.ServerCatalog"/> catalog to be used by the view model, imported from MEF.
        /// </summary>
        [Import]
        public ServerWatchTower.Agent.Model.ServerCatalog Catalog
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

        #endregion

        /// <summary>
        /// Gets the data item which will be displayed and optionally edited on the view.
        /// </summary>
        public ServerE Item
        {
            get => this.itemField;

            private set
            {
                if (ReferenceEquals(this.itemField, value))
                {
                    return;
                }

                if (this.itemField != null)
                {
                    this.UnregisterItem(this.itemField);
                }

                this.itemField = value;

                if (this.itemField != null)
                {
                    this.RegisterItem(this.itemField);
                }

                this.Notify(ChangeOfProperty.Item);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Item"/> is read only, which happens when the user does not have the rights to update it, or it is being saved in the background.
        /// </summary>
        /// <seealso cref="IsEditable"/>
        public bool IsReadOnly
        {
            get => this.bitValues[1];

            private set
            {
                if (this.bitValues[1] == value)
                {
                    return;
                }

                this.bitValues[1] = value;

                this.Notify(ChangeOfProperty.IsReadOnly);
                this.Notify(ChangeOfProperty.IsEditable);

                this.OnIsReadOnlyChanged(value);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Item"/> can be edited.
        /// </summary>
        /// <remarks>
        /// <para>The value of this property is the negation of the <see cref="IsReadOnly"/> property.</para>
        /// </remarks>
        /// <seealso cref="IsReadOnly"/>
        public bool IsEditable
        {
            get => !this.IsReadOnly;
        }

        /// <summary>
        /// Gets or sets the index of the panel (tab page) being selected on the view.
        /// </summary>
        public int ViewPanelIndex
        {
            get => this.viewPanelIndexField;
            set
            {
                this.OnViewPanelIndexChanging(ref value);

                if (this.viewPanelIndexField != value)
                {
                    this.viewPanelIndexField = value;

                    this.OnViewPanelIndexChanged(value);

                    this.NotifyChangeOf(nameof(this.ViewPanelIndex));
                }
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
        /// Gets or sets a value indicating whether the closing of the View is permitted, without checking whether there are changes made to the data.
        /// </summary>
        private bool AllowClosing
        {
            get => this.bitValues[2];
            set => this.bitValues[2] = value;
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

            this.View.Closing += this.OnViewClosing;

            this.Initialize();
        }

        /// <summary>
        /// Initializes the command properties by creating the commands of the <see cref="ServerEditViewModel"/>.
        /// </summary>
        private void BuildCommands()
        {
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

        /// <summary>
        /// Checks whether the view can be closed, which depends on whether there are unsaved changes made by the user.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The arguments of the event.</param>
        private void OnViewClosing(object sender, CancelEventArgs e)
        {
            if (this.AllowClosing)
            {
                return;
            }

            this.CanCloseAndLooseChanges(e);
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
        partial void RegisterItem(ServerE item);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void UnregisterItem(ServerE item);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void OnIsReadOnlyChanged(bool value);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void OnViewPanelIndexChanging(ref int value);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void OnViewPanelIndexChanged(int value);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void CanCloseAndLooseChanges(CancelEventArgs e);

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

        /* Method stub: RegisterItem

        /// <summary>
        /// Registers the appropriate event handlers and creates the collection views when a new object has been set for the <see cref="Item"/> property.
        /// </summary>
        /// <param name="item">The new object being set for the <see cref="Item"/> property.</param>
        partial void RegisterItem(ServerE item)
        {
            //// TODO: create the view collections, attach the event handlers etc.
        }

        */

        /* Method stub: UnregisterItem

        /// <summary>
        /// Unregisters the event handlers and disposes the collection views used with the object previously set for the <see cref="Item"/> property.
        /// </summary>
        /// <param name="item">The object previously set for the <see cref="Item"/> property, which will no longer be used.</param>
        partial void UnregisterItem(ServerE item)
        {
            //// TODO: dispose the view collections, detach the event handlers etc.
        }

        */

        /* Method stub: CanCloseAndLooseChanges

        /// <summary>
        /// Checks whether the View can be closed and the changes made by the user lost by warning the user about this.
        /// </summary>
        /// <param name="e">The arguments through which the closing of the view can be canceled.</param>
        partial void CanCloseAndLooseChanges(CancelEventArgs e)
        {
            //// TODO: cancel closing when the user does not want to loose the changes
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

