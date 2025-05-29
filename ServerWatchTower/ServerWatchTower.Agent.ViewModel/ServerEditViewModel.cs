namespace ServerWatchTower.Agent.ViewModel
{
    using Cosys.SilverLib.Model;
    using ServerWatchTower.Agent.Model;
    using ServerWatchTower.Agent.ViewModel.Properties;
    using System;
    using System.Threading.Tasks;
    using System.Windows;

    /// <summary>
    /// The view model class of the Server Edit.
    /// </summary>
    public partial class ServerEditViewModel
    {
        #region Internal fields

        /// <summary>
        /// The default filtering arguments for the partner catalog or partner selector.
        /// </summary>
        internal readonly static ServerCatalogFilterArgs DefaultFilterArgs
            = (new ServerCatalogFilterArgs()).StripAndLock();

        #endregion

        #region Private fields



        #endregion

        /// <summary>
        /// Releases the unmanaged resources used by the ViewModel and removes the event handlers attached to long-lived objects.
        /// </summary>
        /// <param name="disposing">True when disposing is called explicitly; false when the method is called by the finalizer.</param>
        protected override void Dispose(bool disposing)
        {
            this.DisposeComponents();



            base.Dispose(disposing);
        }

        /// <inheritdoc/>
        protected override async Task OnLoadAsync()
        {
            this.IsReadOnly = true;

            if (this.IsDisposed) { return; }

            if (this.OpenArgs != null)
            {
                await this.LoadItemAsync(this.OpenArgs.SelectedServerGUID);
            }
            else
            {
                throw new ArgumentNullException(nameof(this.OpenArgs));
            }

            this.IsReadOnly = false;
        }

        /// <summary>
        /// Loads the <see cref="Item"/> which will be edited on the view from the <see cref="Catalog"/> asynchronously.
        /// </summary>
        /// <param name="serverGuid">The <see cref="ServerE.GUID"/> of the server to edit.</param>
        /// <returns>The <see cref="Task"/> which will load the <see cref="Item"/> asynchronously.</returns>
        private async Task LoadItemAsync(string serverGuid)
        {
            ServerE editableItem;

            if (serverGuid != null)
            {
                editableItem = await this.Catalog.GetEditableItemAsync(serverGuid);
            }
            else
            {
                throw new ArgumentNullException(nameof(serverGuid));
            }

            if (this.IsDisposed) { return; }

            editableItem.StartEditing();

            this.Item = editableItem;
        }

        /// <summary>
        /// Checks whether the <see cref="SaveAndCloseCommand"/> can currently be executed.
        /// </summary>
        /// <returns>True when the command can be executed; false otherwise.</returns>
        private bool CanExecuteSaveAndClose() => true;

        /// <summary>
        /// Executes the <see cref="SaveAndCloseCommand"/> by ...
        /// </summary>
        private async void ExecuteSaveAndClose()
        {
            if (this.Item == null || this.IsReadOnly)
            {
                return;
            }

            this.View.CommitChanges();

            if (!this.View.Validate())
            {
                return;
            }

            if (this.Item.IsNew || this.Item.IsDirty)
            {
                if (await this.SaveServerAsync(reloadItemAfterSave: false))
                {
                    // closing the view (the DialogResult is already set)
                    this.AllowClosing = true;
                    this.View.Close();
                }
            }
            else
            {
                // no changes were made, closing the view
                this.View.DialogResult = false;
                this.AllowClosing = true;
                this.View.Close();
            }
        }

        /// <summary>
        /// Saves the data of the server asynchronously after validation, and optionally reloads it into the <see cref="Item"/> after save.
        /// </summary>
        /// <param name="reloadItemAfterSave">True when the data of the partner should be reloaded into the <see cref="Item"/> after being saved,
        /// which is the case when the editing will continue in the view.</param>
        /// <returns>A <see cref="Task{TResult}"/> with its <see cref="Task{TResult}.Result"/> being
        /// true when saving succeeded, or false otherwise.</returns>
        private async Task<bool> SaveServerAsync(bool reloadItemAfterSave)
        {
            bool retry;
            string serverGUID = null;

            this.Item.Sanitize();

            do
            {
                retry = false;

                try
                {
                    this.Item.Validate();

                    this.IsReadOnly = true;

                    serverGUID = await this.Catalog.SaveItemAsync(this.Item);

                    // we have to indicate to the parent form that changes have been made to the server
                    this.View.DialogResult = true;
                    this.View.ViewResult = serverGUID;
                }
                catch (IgnorableValidationException exc)
                {
                    this.IsReadOnly = false;

                    if (this.Item.SkipIgnorableValidations)
                    {
                        this.Item.SkipIgnorableValidations = false;

                        this.Application.ShowMessageBox(MessageBoxImage.Exclamation, exc.Message, Res.TitleSaving);

                        if (!string.IsNullOrEmpty(exc.MemberName))
                        {
                            this.FocusControlOfProperty(exc.MemberName);
                        }

                        return false;
                    }
                }
                catch (ValidationException exc)
                {
                    this.IsReadOnly = false;

                    this.Item.SkipIgnorableValidations = false;

                    this.Application.ShowMessageBox(MessageBoxImage.Exclamation, exc.Message, Res.TitleSaving);

                    if (!string.IsNullOrEmpty(exc.MemberName))
                    {
                        this.FocusControlOfProperty(exc.MemberName);
                    }

                    return false;
                }
                catch (UserDatabaseException exc)
                {
                    this.IsReadOnly = false;

                    this.Item.SkipIgnorableValidations = false;

                    this.Application.HandleError(exc, Res.TitleSaving);
                    return false;
                }
                catch (Exception exc)
                {
                    this.IsReadOnly = false;

                    this.Item.SkipIgnorableValidations = false;

                    this.Application.HandleError(exc, Res.TitleSaving);
                    return false;
                }
            }
            while (retry);

            if (reloadItemAfterSave && serverGUID != null)
            {
                await this.LoadItemAsync(serverGUID);
            }

            // saving succeeded
            return true;
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
        private async void ExecuteRefresh()
        {
            if (!this.CanExecuteRefresh())
            {
                return;
            }

            if (this.Item.IsDirty)
            {
                if (this.Application.ShowMessageBox(MessageBoxImage.Exclamation, Res.MsgChangesNotSavedRefresh, Res.CapServerEdit, MessageBoxButton.YesNo, MessageBoxResult.No) == MessageBoxResult.No)
                {
                    return;
                }
            }

            this.IsLoading = true;

            try
            {
                await this.LoadItemAsync(this.Item.GUID);
            }
            catch (Exception exc)
            {
                this.Application.HandleError(exc);
            }
            finally
            {
                this.RefreshCommandState();
                this.IsLoading = false;
            }
        }
    }
}
