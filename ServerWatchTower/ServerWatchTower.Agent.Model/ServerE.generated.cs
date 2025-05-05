//-----------------------------------------------------------------------
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
        /// Gets or sets the unique identifier of the <see cref="Server"/>.
        /// </summary>
        /// <exception cref="ArgumentException">The specified value is not valid for this property.</exception>
        [DataMember]
        public int Id
        {
            get => this.data.id;
            set
            {
                if (!this.IsEditing)
                {
                    this.data.id = value;
                    return;
                }

                if (this.data.id != value)
                {
                    this.IdBeforeUpdate(ref value);
                    this.ValidateProperty(nameof(this.Id), ref value);

                    if (this.data.id != value)
                    {
						var oldValue = this.data.id;

                        this.data.id = value;

                        this.IdAfterUpdate(oldValue, value);
                        this.Notify(ChangeOfProperty.Id);
                        this.IsDirty = true;
                    }
                }
            }
        }

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
        public bool isApproved
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

                if (this.isApproved != value)
                {
                    this.isApprovedBeforeUpdate(ref value);
                    this.ValidateProperty(nameof(this.isApproved), ref value);

					bool oldValue = this.isApproved;

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

                        this.isApprovedAfterUpdate(oldValue, value);
                        this.NotifyChangeOf(nameof(this.isApproved));
                        this.Notify(ChangeOfProperty.Flag);
                        this.IsDirty = true;
                    }
                }
            }
        }		

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void IdBeforeUpdate(ref int value);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void IdAfterUpdate(int oldValue, int newValue);

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
        partial void FlagBeforeUpdate(ref int value);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void FlagAfterUpdate(int oldValue, int newValue);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void isApprovedBeforeUpdate(ref bool value);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void isApprovedAfterUpdate(bool oldValue, bool newValue);

        #region Data class

        /// <summary>
        /// Class for storing the property values of the <see cref="ServerE"/> instance.
        /// </summary>
        private partial class Data
        {
            /// <summary>
            /// The unique identifier of the <see cref="Server"/>.
            /// </summary>
            public int id;

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
                this.id = data.id;
                this.gUID = data.gUID;
                this.publicKey = data.publicKey;
                this.partner = data.partner;
                this.serverName = data.serverName;
                this.windows = data.windows;
                this.flag = data.flag;
            }
        }

        #endregion
    }
}

