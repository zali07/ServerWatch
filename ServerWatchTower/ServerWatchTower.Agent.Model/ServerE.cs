//-----------------------------------------------------------------------
// <copyright file="PartnerMappingE.cs" company="Cosys SRL.">
//     Copyright (c) 2012, 2024 Cosys SRL. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ServerWatchTower.Agent.Model
{
    using System;
    using Cosys.SilverLib.Model;
    using ServerWatchTower.Agent.Model.Properties;

    /// <summary>
    /// An editable <see cref="Server"/> instance with all the defined properties. (gtEFactRecMaparePartener)
    /// </summary>
    public partial class ServerE
    {
        #region Private fields

        /// <summary>
        /// Indicates whether the item events and validators have been registered for the member collections.
        /// </summary>
        private bool areItemEventsRegistered;

        /// <summary>
        /// Indicates whether the <see cref="RegisterContext"/> method has been called.
        /// </summary>
        private bool isContextSet;

        #endregion


        /// <summary>
        /// Sanitizes the data of the partner before it would be saved by removing the trailing spaces from
        /// some of the fields and by setting a default main contact person when it has not been set.
        /// </summary>
        public void Sanitize()
        {
            if (this.IsEditing && !this.IsDirty)
            {
                return;
            }

            if (!string.IsNullOrEmpty(this.Partner))
            {
                this.Partner = this.Partner.Trim();
            }

            if (!string.IsNullOrEmpty(this.ServerName))
            {
                this.ServerName = this.ServerName.Trim();
            }
            
            if (!string.IsNullOrEmpty(this.Windows))
            {
                this.Windows = this.Windows.Trim();
            }
        }

        /// <summary>
        /// Registers the context to be used by the object during editing for validation and initialization.
        /// </summary>
        /// <exception cref="InvalidOperationException">The method is called when the object is already in editing mode.</exception>
        public void RegisterContext()
        {
            if (this.IsEditing)
            {
                throw new InvalidOperationException();
            }

            this.isContextSet = true;
        }

        /// <summary>
        /// Method called when the object is being put into the editable state by the <see cref="EditableObject.StartEditing"/> method.
        /// </summary>
        protected override void OnStartEditing()
        {
            if (!this.isContextSet)
            {
                throw new InvalidOperationException(Resource.ExcServerContextNotSet);
            }

            if (!this.areItemEventsRegistered)
            {
                this.AddValidator(ValidatePartnerMapping);

                this.areItemEventsRegistered = true;
            }

            base.OnStartEditing();
        }

        /// <summary>
        /// Method called when the object is being taken out from the editable state by the <see cref="EditableObject.StopEditing"/> method.
        /// </summary>
        protected override void OnStopEditing()
        {
            base.OnStopEditing();
        }

        /// <summary>
        /// Validates a <see cref="ServerE"/> instance to see whether its properties have been properly set.
        /// </summary>
        /// <param name="sender">The sender of the validation event, which is the instance to validate.</param>
        /// <param name="args">The arguments of the event.</param>
        private static void ValidatePartnerMapping(object sender, ValidateEventArgs args)
        {
            if (!args.IsValid)
            {
                return;
            }

            var server = (ServerE)sender;

            //if (string.IsNullOrWhiteSpace(server.Name))
            //{
            //    args.ErrorMessage = Resource.ValServerNameRequired;
            //    args.MemberName = nameof(server.Name);
            //    args.IsValid = false;
            //    return;
            //}

            if (!args.SkipIgnorableValidations)
            {

            }
        }

        /// <summary>
        /// Sets the <see cref="EditableObject.IsDirty"/> property to <c>true</c> whenever any of the collections member becomes dirty.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The arguments of the event.</param>
        private void OnCollectionDirtyBitSet(object sender, EventArgs e)
        {
            this.IsDirty = true;
        }
    }
}
