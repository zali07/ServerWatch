﻿//-----------------------------------------------------------------------
// <copyright file="ServerE.generated.cs" company="Cosys SRL.">
//     Copyright (c) 2012, 2025 Cosys SRL. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ServerWatchTower.Agent.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.Serialization;
    using Cosys.SilverLib.Model;

    /// <summary>
    /// An editable <see cref="Server"/> instance with all the defined properties.
    /// </summary>
    [DataContract(Namespace = "http://software.cosys.ro/SilverERP/V1/Agent")]
    public partial class ServerE : EditableObject
    {
        #region Private fields

        /// <summary>
        /// The current values of the <see cref="ServerE"/> properties.
        /// </summary>
        private Data data = new Data();

        #endregion

        /// <summary>
        /// Gets or sets the corresponding GUID of the server.
        /// </summary>
        /// <exception cref="ArgumentException">The specified value is not valid for this property.</exception>
        [DataMember(IsRequired = true)]
        public string GUID
        {
            get => this.data.gUID;
            set
            {
                if (!this.IsEditing)
                {
                    this.data.gUID = value;
                    return;
                }

                if (this.data.gUID != value)
                {
                    this.GUIDBeforeUpdate(ref value);
                    this.ValidateProperty(nameof(this.GUID), ref value);

                    if (this.data.gUID != value)
                    {
						var oldValue = this.data.gUID;

                        this.data.gUID = value;

                        this.GUIDAfterUpdate(oldValue, value);
                        this.NotifyChangeOf(nameof(this.GUID));
                        this.IsDirty = true;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the corresponding publicKey for the server used for authentication.
        /// </summary>
        /// <exception cref="ArgumentException">The specified value is not valid for this property.</exception>
        [DataMember]
        public string PublicKey
        {
            get => this.data.publicKey;
            set
            {
                if (!this.IsEditing)
                {
                    this.data.publicKey = value;
                    return;
                }

                if (this.data.publicKey != value)
                {
                    this.PublicKeyBeforeUpdate(ref value);
                    this.ValidateProperty(nameof(this.PublicKey), ref value);

                    if (this.data.publicKey != value)
                    {
						var oldValue = this.data.publicKey;

                        this.data.publicKey = value;

                        this.PublicKeyAfterUpdate(oldValue, value);
                        this.NotifyChangeOf(nameof(this.PublicKey));
                        this.IsDirty = true;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the corresponding partner for the server.
        /// </summary>
        /// <exception cref="ArgumentException">The specified value is not valid for this property.</exception>
        [DataMember]
        public string Partner
        {
            get => this.data.partner;
            set
            {
                if (!this.IsEditing)
                {
                    this.data.partner = value;
                    return;
                }

                if (this.data.partner != value)
                {
                    this.PartnerBeforeUpdate(ref value);
                    this.ValidateProperty(nameof(this.Partner), ref value);

                    if (this.data.partner != value)
                    {
						var oldValue = this.data.partner;

                        this.data.partner = value;

                        this.PartnerAfterUpdate(oldValue, value);
                        this.NotifyChangeOf(nameof(this.Partner));
                        this.IsDirty = true;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the corresponding server name of the server.
        /// </summary>
        /// <exception cref="ArgumentException">The specified value is not valid for this property.</exception>
        [DataMember]
        public string ServerName
        {
            get => this.data.serverName;
            set
            {
                if (!this.IsEditing)
                {
                    this.data.serverName = value;
                    return;
                }

                if (this.data.serverName != value)
                {
                    this.ServerNameBeforeUpdate(ref value);
                    this.ValidateProperty(nameof(this.ServerName), ref value);

                    if (this.data.serverName != value)
                    {
						var oldValue = this.data.serverName;

                        this.data.serverName = value;

                        this.ServerNameAfterUpdate(oldValue, value);
                        this.NotifyChangeOf(nameof(this.ServerName));
                        this.IsDirty = true;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the corresponding windows version of the server.
        /// </summary>
        /// <exception cref="ArgumentException">The specified value is not valid for this property.</exception>
        [DataMember]
        public string Windows
        {
            get => this.data.windows;
            set
            {
                if (!this.IsEditing)
                {
                    this.data.windows = value;
                    return;
                }

                if (this.data.windows != value)
                {
                    this.WindowsBeforeUpdate(ref value);
                    this.ValidateProperty(nameof(this.Windows), ref value);

                    if (this.data.windows != value)
                    {
						var oldValue = this.data.windows;

                        this.data.windows = value;

                        this.WindowsAfterUpdate(oldValue, value);
                        this.NotifyChangeOf(nameof(this.Windows));
                        this.IsDirty = true;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the backup root on the server for the daily and weekly backups.
        /// </summary>
        /// <exception cref="ArgumentException">The specified value is not valid for this property.</exception>
        [DataMember]
        public string BackupRoot
        {
            get => this.data.backupRoot;
            set
            {
                if (!this.IsEditing)
                {
                    this.data.backupRoot = value;
                    return;
                }

                if (this.data.backupRoot != value)
                {
                    this.BackupRootBeforeUpdate(ref value);
                    this.ValidateProperty(nameof(this.BackupRoot), ref value);

                    if (this.data.backupRoot != value)
                    {
						var oldValue = this.data.backupRoot;

                        this.data.backupRoot = value;

                        this.BackupRootAfterUpdate(oldValue, value);
                        this.NotifyChangeOf(nameof(this.BackupRoot));
                        this.IsDirty = true;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the flags of the server.
        /// </summary>
        /// <exception cref="ArgumentException">The specified value is not valid for this property.</exception>
        [DataMember]
        public int Flag
        {
            get => this.data.flag;
            set
            {
                if (!this.IsEditing)
                {
                    this.data.flag = value;
                    return;
                }

                if (this.data.flag != value)
                {
                    this.FlagBeforeUpdate(ref value);
                    this.ValidateProperty(nameof(this.Flag), ref value);

                    if (this.data.flag != value)
                    {
						var oldValue = this.data.flag;

                        this.data.flag = value;

                        this.FlagAfterUpdate(oldValue, value);
                        this.Notify(ChangeOfProperty.Flag);
                        this.IsDirty = true;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating if the server was approved for telemetric data gathering by an administrator,
        /// this being one of the bits of the <see cref="Flag"/> property.
        /// </summary>
        [IgnoreDataMember]
        public bool IsApproved
        {
            get => (this.data.flag & 1) != 0;
            set
            {
                if (!this.IsEditing)
                {
                    if (value)
                    {
                        this.data.flag |= 1;
                    }
                    else
                    {
                        this.data.flag &= ~1;
                    }

                    return;
                }

                if (this.IsApproved != value)
                {
                    this.IsApprovedBeforeUpdate(ref value);
                    this.ValidateProperty(nameof(this.IsApproved), ref value);

					bool oldValue = this.IsApproved;

                    if (oldValue != value)
                    {
                        if (value)
                        {
                            this.data.flag |= 1;
                        }
                        else
                        {
                            this.data.flag &= ~1;
                        }

                        this.IsApprovedAfterUpdate(oldValue, value);
                        this.NotifyChangeOf(nameof(this.IsApproved));
                        this.Notify(ChangeOfProperty.Flag);
                        this.IsDirty = true;
                    }
                }
            }
        }		

        /// <summary>
        /// Gets or sets a value indicating if the server was removed from data gathering or is out of service,
        /// this being one of the bits of the <see cref="Flag"/> property.
        /// </summary>
        [IgnoreDataMember]
        public bool IsDeleted
        {
            get => (this.data.flag & 2) != 0;
            set
            {
                if (!this.IsEditing)
                {
                    if (value)
                    {
                        this.data.flag |= 2;
                    }
                    else
                    {
                        this.data.flag &= ~2;
                    }

                    return;
                }

                if (this.IsDeleted != value)
                {
                    this.IsDeletedBeforeUpdate(ref value);
                    this.ValidateProperty(nameof(this.IsDeleted), ref value);

					bool oldValue = this.IsDeleted;

                    if (oldValue != value)
                    {
                        if (value)
                        {
                            this.data.flag |= 2;
                        }
                        else
                        {
                            this.data.flag &= ~2;
                        }

                        this.IsDeletedAfterUpdate(oldValue, value);
                        this.Notify(ChangeOfProperty.IsDeleted);
                        this.Notify(ChangeOfProperty.Flag);
                        this.IsDirty = true;
                    }
                }
            }
        }		

        /// <summary>
        /// Gets or sets a value indicating if the server has enabled mirroring data gathering,
        /// this being one of the bits of the <see cref="Flag"/> property.
        /// </summary>
        [IgnoreDataMember]
        public bool HasMirroringDataGather
        {
            get => (this.data.flag & 4) != 0;
            set
            {
                if (!this.IsEditing)
                {
                    if (value)
                    {
                        this.data.flag |= 4;
                    }
                    else
                    {
                        this.data.flag &= ~4;
                    }

                    return;
                }

                if (this.HasMirroringDataGather != value)
                {
                    this.HasMirroringDataGatherBeforeUpdate(ref value);
                    this.ValidateProperty(nameof(this.HasMirroringDataGather), ref value);

					bool oldValue = this.HasMirroringDataGather;

                    if (oldValue != value)
                    {
                        if (value)
                        {
                            this.data.flag |= 4;
                        }
                        else
                        {
                            this.data.flag &= ~4;
                        }

                        this.HasMirroringDataGatherAfterUpdate(oldValue, value);
                        this.NotifyChangeOf(nameof(this.HasMirroringDataGather));
                        this.Notify(ChangeOfProperty.Flag);
                        this.IsDirty = true;
                    }
                }
            }
        }		

        /// <summary>
        /// Gets or sets a value indicating if the server has enabled drive data gathering,
        /// this being one of the bits of the <see cref="Flag"/> property.
        /// </summary>
        [IgnoreDataMember]
        public bool HasDriveDataGather
        {
            get => (this.data.flag & 8) != 0;
            set
            {
                if (!this.IsEditing)
                {
                    if (value)
                    {
                        this.data.flag |= 8;
                    }
                    else
                    {
                        this.data.flag &= ~8;
                    }

                    return;
                }

                if (this.HasDriveDataGather != value)
                {
                    this.HasDriveDataGatherBeforeUpdate(ref value);
                    this.ValidateProperty(nameof(this.HasDriveDataGather), ref value);

					bool oldValue = this.HasDriveDataGather;

                    if (oldValue != value)
                    {
                        if (value)
                        {
                            this.data.flag |= 8;
                        }
                        else
                        {
                            this.data.flag &= ~8;
                        }

                        this.HasDriveDataGatherAfterUpdate(oldValue, value);
                        this.NotifyChangeOf(nameof(this.HasDriveDataGather));
                        this.Notify(ChangeOfProperty.Flag);
                        this.IsDirty = true;
                    }
                }
            }
        }		

        /// <summary>
        /// Gets or sets a value indicating if the server has enabled backup data gathering,
        /// this being one of the bits of the <see cref="Flag"/> property.
        /// </summary>
        [IgnoreDataMember]
        public bool HasBackupDataGather
        {
            get => (this.data.flag & 16) != 0;
            set
            {
                if (!this.IsEditing)
                {
                    if (value)
                    {
                        this.data.flag |= 16;
                    }
                    else
                    {
                        this.data.flag &= ~16;
                    }

                    return;
                }

                if (this.HasBackupDataGather != value)
                {
                    this.HasBackupDataGatherBeforeUpdate(ref value);
                    this.ValidateProperty(nameof(this.HasBackupDataGather), ref value);

					bool oldValue = this.HasBackupDataGather;

                    if (oldValue != value)
                    {
                        if (value)
                        {
                            this.data.flag |= 16;
                        }
                        else
                        {
                            this.data.flag &= ~16;
                        }

                        this.HasBackupDataGatherAfterUpdate(oldValue, value);
                        this.NotifyChangeOf(nameof(this.HasBackupDataGather));
                        this.Notify(ChangeOfProperty.Flag);
                        this.IsDirty = true;
                    }
                }
            }
        }		

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void GUIDBeforeUpdate(ref string value);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void GUIDAfterUpdate(string oldValue, string newValue);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void PublicKeyBeforeUpdate(ref string value);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void PublicKeyAfterUpdate(string oldValue, string newValue);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void PartnerBeforeUpdate(ref string value);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void PartnerAfterUpdate(string oldValue, string newValue);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void ServerNameBeforeUpdate(ref string value);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void ServerNameAfterUpdate(string oldValue, string newValue);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void WindowsBeforeUpdate(ref string value);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void WindowsAfterUpdate(string oldValue, string newValue);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void BackupRootBeforeUpdate(ref string value);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void BackupRootAfterUpdate(string oldValue, string newValue);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void FlagBeforeUpdate(ref int value);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void FlagAfterUpdate(int oldValue, int newValue);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void IsApprovedBeforeUpdate(ref bool value);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void IsApprovedAfterUpdate(bool oldValue, bool newValue);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void IsDeletedBeforeUpdate(ref bool value);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void IsDeletedAfterUpdate(bool oldValue, bool newValue);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void HasMirroringDataGatherBeforeUpdate(ref bool value);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void HasMirroringDataGatherAfterUpdate(bool oldValue, bool newValue);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void HasDriveDataGatherBeforeUpdate(ref bool value);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void HasDriveDataGatherAfterUpdate(bool oldValue, bool newValue);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void HasBackupDataGatherBeforeUpdate(ref bool value);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void HasBackupDataGatherAfterUpdate(bool oldValue, bool newValue);

        #region Data class

        /// <summary>
        /// Class for storing the property values of the <see cref="ServerE"/> instance.
        /// </summary>
        private partial class Data
        {
            /// <summary>
            /// The corresponding GUID of the server.
            /// </summary>
            public string gUID;

            /// <summary>
            /// The corresponding publicKey for the server used for authentication.
            /// </summary>
            public string publicKey;

            /// <summary>
            /// The corresponding partner for the server.
            /// </summary>
            public string partner;

            /// <summary>
            /// The corresponding server name of the server.
            /// </summary>
            public string serverName;

            /// <summary>
            /// The corresponding windows version of the server.
            /// </summary>
            public string windows;

            /// <summary>
            /// The backup root on the server for the daily and weekly backups.
            /// </summary>
            public string backupRoot;

            /// <summary>
            /// The flags of the server.
            /// </summary>
            public int flag;

            /// <summary>
            /// Initializes a new instance of the <see cref="Data"/> class.
            /// </summary>
            public Data()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Data"/> class with the values of another instance.
            /// </summary>
            /// <param name="data">The other instance, which should be duplicated.</param>
            public Data(Data data)
            {
                this.gUID = data.gUID;
                this.publicKey = data.publicKey;
                this.partner = data.partner;
                this.serverName = data.serverName;
                this.windows = data.windows;
                this.backupRoot = data.backupRoot;
                this.flag = data.flag;
            }
        }

        #endregion
    }
}

