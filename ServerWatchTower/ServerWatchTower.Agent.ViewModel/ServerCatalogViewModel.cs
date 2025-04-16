namespace ServerWatchTower.Agent.ViewModel
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using ServerWatchTower.Agent.Model;
    using Cosys.SilverLib.Core;
    using Cosys.SilverLib.Model;

    /// <summary>
    /// The view model class of the Server Catalog.
    /// </summary>
    public partial class ServerCatalogViewModel
    {
        #region Internal fields

        /// <summary>
        /// The default filtering arguments for the partner catalog or partner selector.
        /// </summary>
        internal readonly static ServerCatalogFilterArgs DefaultFilterArgs
            = (new ServerCatalogFilterArgs()).StripAndLock();

        #endregion

        #region Private fields

        /// <summary>
        /// The base filtering conditions which will always get applied besides the quick filtering conditions on
        /// the loaded items.
        /// </summary>
        private ServerCatalogFilterArgs baseFilter;

        /// <summary>
        /// The value of <see cref="QuickFilter"/> without the <c>'%'</c> and <c>'^'</c> prefixes, updated
        /// together with <see cref="quickFilterField"/>.
        /// </summary>
        private string quickFilterCore = "";

        /// <summary>
        /// The <see cref="Server"/> collection under the <see cref="Content"/> <c>CollectionView</c>.
        /// </summary>
        private CatalogCollection<string, Server> serverCollection;

        #endregion

        /// <summary>
        /// Gets the collection view to the <see cref="Server"/> instances to be displayed on the view of the catalog.
        /// </summary>
        public CatalogCollectionView Content
        {
            get;
            private set;
        }

        /// <summary>
        /// Releases the unmanaged resources used by the ViewModel and removes the event handlers attached to long-lived objects.
        /// </summary>
        /// <param name="disposing">True when disposing is called explicitly; false when the method is called by the finalizer.</param>
        protected override void Dispose(bool disposing)
        {
            this.DisposeComponents();

            if (this.Content != null)
            {
                this.Content.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Checks the arguments passed to the view and the access rights of the user, before the actual initialization of the view model.
        /// </summary>
        partial void PreInitialize()
        {
            if (this.OpenArgs != null)
            {
                this.baseFilter = (this.OpenArgs.BaseFilter ?? DefaultFilterArgs).StripAndLock();

                if (this.OpenArgs.SelectionMode)
                {
                    //// // Note: The SelectablesOnly feature is not used at this time.
                    //// if (this.OpenArgs.SelectionFilter != null || this.OpenArgs.SelectionFilterFunction != null)
                    //// {
                    ////     // no refresh will be initiated at this point because IsInSelectionMode is still false
                    ////     this.SelectablesOnly = true;
                    ////     this.IsSelectablesOnlyFilterAvailable = true;
                    //// }

                    this.IsInSelectionMode = true;
                }
            }
            else
            {
                this.baseFilter = DefaultFilterArgs;
            }

            //this.IsItemEditable = this.Rights.CanEditServer;
        }

        /// <inheritdoc/>
        protected override async Task OnLoadAsync()
        {
            this.serverCollection = this.Catalog.CreateCollection();

            this.Content = new CatalogCollectionView(this.serverCollection);

            using (this.Content.DeferRefresh())
            {
                this.Content.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                this.Content.Filter = this.ContentFilter;
            }

            this.Content.CurrentChanged += this.CurrentItemChanged;

            await this.LoadCatalogsAsync();

            if (this.IsDisposed) { return; }

            if (this.OpenArgs != null)
            {
                // loading the partner to be selected initially
                if (this.OpenArgs.SelectedPartnerCode != null)
                {
                    var partner = await this.Catalog.LoadItemAsync(this.serverCollection, this.OpenArgs.SelectedPartnerCode);

                    if (this.IsDisposed) { return; }

                    if (this.ContentFilter(partner))
                    {
                        if (this.Content.MoveCurrentTo(partner))
                        {
                            this.BringCollectionItemIntoView(nameof(this.Content), partner);
                        }
                    }
                }
            }

            // display the view at this point
            await this.View.DisplayAsync();

            // loading the initial partners into the collection
            await this.Catalog.LoadDataAsync(this.serverCollection, this.baseFilter);
        }

        /// <summary>
        /// Checks and in certain cases updates the value being set for the <see cref="QuickFilter"/> property.
        /// </summary>
        /// <param name="value">The new value being set for the property, which optionally can be updated.</param>
        partial void OnQuickFilterChanging(ref string value)
        {
            if (value == null)
            {
                value = string.Empty;
            }
        }

        /// <summary>
        /// Starts loading the partners corresponding to the new filtering criteria when
        /// the <see cref="QuickFilter"/> property has changed.
        /// </summary>
        /// <param name="value">The new value being set for the <see cref="QuickFilter"/> property.</param>
        partial void OnQuickFilterChanged(string value)
        {
            if (value.Length < 1 || !(value[0] == '%' || value[0] == '^'))
            {
                this.quickFilterCore = value;
            }
            else
            {
                this.quickFilterCore = value.Substring(1);
            }

            this.SchedulePartnerLoading();
            this.Content?.RefreshWhenIdle();
        }

        /// <summary>
        /// Handles the case when the currently selected item has changed.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The arguments of the event.</param>
        private void CurrentItemChanged(object sender, EventArgs e)
        {
            this.RefreshCommandState();
        }

        /// <summary>
        /// Schedules the loading of the partners corresponding to the updated search criteria to be started in a few milliseconds.
        /// </summary>
        private void SchedulePartnerLoading()
        {
            var args = new ServerCatalogFilterArgs(this.baseFilter)
            {
                QuickFilter = this.quickFilterCore
            };

            if (!string.IsNullOrEmpty(this.QuickFilter))
            {
                if (this.QuickFilter[0] == '%')
                {
                    args.QuickSearchWithinName = args.QuickSearchInTaxCode = true;
                }
                else if (this.QuickFilter[0] == '^')
                {
                    args.QuickSearchWithinName = args.QuickSearchInTaxCode = false;
                }
            }

            args.Lock();

            _ = this.contentLoader.StartAsync(args);
        }

        /// <summary>
        /// The data loading method of the <see cref="contentLoader"/>, which starts the background loading task.
        /// </summary>
        /// <param name="args">The arguments which specify which data has to be loaded from the database.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> through which the loading operation might get canceled by the <see cref="BackgroundLoader{TArg, TData}"/>.</param>
        /// <returns>The data loading task, which has been started in the background by this method.</returns>
        private Task<SynchronizationResult> ContentLoader_LoadAsync(ServerCatalogFilterArgs args, CancellationToken cancellationToken)
        {
            return this.Catalog.LoadDataAsync(this.serverCollection, args);
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

            // no further processing is needed, as the Catalog.LoadDataAsync will also update the Content collection
        }

        /// <summary>
        /// Checks whether the <see cref="RefreshCommand"/> can currently be executed.
        /// </summary>
        /// <returns>True when the command can be executed; false otherwise.</returns>
        private bool CanExecuteRefresh()
        {
            return true;
        }

        /// <summary>
        /// Executes the <see cref="RefreshCommand"/> by ...
        /// </summary>
        private void ExecuteRefresh()
        {
            if (!this.CanExecuteRefresh())
            {
                return;
            }

            this.SchedulePartnerLoading();
        }

        /// <summary>
        /// Checks whether the <see cref="OpenItemCommand"/> can currently be executed.
        /// </summary>
        /// <returns>True when the command can be executed; false otherwise.</returns>
        private bool CanExecuteOpenItem()
        {
            return this.Content.CurrentItem != null;
        }

        /// <summary>
        /// Executes the <see cref="OpenItemCommand"/> by opening the editing view with the current item from the catalog.
        /// </summary>
        private void ExecuteOpenItem()
        {
            if (!this.CanExecuteOpenItem())
            {
                return;
            }

            //var partner = (Server)this.Content.CurrentItem;

            //string viewContract = "Cosys.SilverERP.Registry.ServerEditView";

            //var view = this.Application.FindView(viewContract, partner.Cui);

            //if (view != null)
            //{
            //    view.Activate();
            //}
            //else
            //{
            //    var args = new ServerEditViewArgs()
            //    {
            //        PartnerCui = partner.Cui
            //    };

            //    view = this.Application.CreateView(viewContract, args);

            //    view.Closed +=
            //        async (sender, e) =>
            //        {
            //            if (!this.IsDisposed && e.DialogResult == true)
            //            {
            //                var newPartner = await this.Catalog.ReloadItemAsync(this.serverCollection, (string)e.ViewResult);

            //                if (!this.IsDisposed && this.ContentFilter(newPartner))
            //                {
            //                    if (this.Content.MoveCurrentToKey(newPartner.Cui))
            //                    {
            //                        this.BringCollectionItemIntoView(nameof(this.Content), this.Content.CurrentItem);
            //                    }
            //                }
            //            }
            //        };

            //    if (this.View.DisplayMode == ViewDisplayMode.Normal)
            //    {
            //        view.Show(this.View);
            //    }
            //    else
            //    {
            //        view.ShowModal(this.View);
            //    }
            //}
        }

        /// <summary>
        /// Checks whether the <see cref="SelectItemCommand"/> can currently be executed.
        /// </summary>
        /// <returns>True when the command can be executed; false otherwise.</returns>
        private bool CanExecuteSelectItem()
        {
            return this.Content.CurrentItem is Server server
                && this.baseFilter.IsMatch(server);
        }

        /// <summary>
        /// Executes the <see cref="SelectItemCommand"/> by selecting the current item and closing the view.
        /// </summary>
        private void ExecuteSelectItem()
        {
            if (this.CanExecuteSelectItem())
            {
                this.View.DialogResult = true;
                this.View.ViewResult = this.Content.CurrentKey;
                this.View.Close();
            }
        }

        /// <summary>
        /// Checks whether the <see cref="DefaultItemCommand"/> can currently be executed.
        /// </summary>
        /// <returns>True when the command can be executed; false otherwise.</returns>
        private bool CanExecuteDefaultItem()
        {
            if (this.IsInSelectionMode)
            {
                return this.CanExecuteSelectItem();
            }
            else
            {
                return this.CanExecuteOpenItem();
            }
        }

        /// <summary>
        /// Executes the <see cref="DefaultItemCommand"/> by running either the <see cref="SelectItemCommand"/> 
        /// or the <see cref="OpenItemCommand"/>, depending on the current context.
        /// </summary>
        private void ExecuteDefaultItem()
        {
            if (this.IsInSelectionMode)
            {
                this.ExecuteSelectItem();
            }
            else
            {
                this.ExecuteOpenItem();
            }
        }

        /// <summary>
        /// Checks whether an item should be included into the <see cref="Content"/> collection view.
        /// </summary>
        /// <param name="item">The item to check whether to be included into the <see cref="Content"/> collection.</param>
        /// <returns>True when the item should be included, false otherwise.</returns>
        private bool ContentFilter(object item)
        {
            if (!(item is Server partner))
            {
                return false;
            }

            if (string.IsNullOrEmpty(this.quickFilterCore))
            {
                return true;
            }

            if (partner.Cui != null && partner.Cui.Contains(this.quickFilterCore, StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }

            return partner.Name.HasWordStartingWith(this.quickFilterCore, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
