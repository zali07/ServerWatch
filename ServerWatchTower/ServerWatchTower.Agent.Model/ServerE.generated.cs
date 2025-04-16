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
        /// Gets or sets the unique identifier of the <see cref="Server"/>. (CUI) It's coming from the digiDoc (XML)
        /// </summary>
        /// <remarks>
        /// <para>This property may only be set for a newly created item, as indicated by the <see cref="EditableObject.IsNew"/> property.</para>
        /// </remarks>
        /// <exception cref="InvalidOperationException">An attempt has been made to set this property
        /// for an existing (non-new) item.</exception>
        /// <exception cref="ArgumentException">The specified value is not valid for this property.</exception>
        [DataMember(IsRequired = true)]
        public string Cui
        {
            get => this.data.cui;
            set
            {
                if (!this.IsEditing)
                {
                    this.data.cui = value;
                    return;
                }

                if (this.data.cui != value)
                {
                    if (!this.IsNew)
                    {
                        throw ExceptionHelper.NewPropertySettableForNewException(nameof(this.Cui));
                    }

                    this.CuiBeforeUpdate(ref value);
                    this.ValidateProperty(nameof(this.Cui), ref value);

                    if (this.data.cui != value)
                    {
						var oldValue = this.data.cui;

                        this.data.cui = value;

                        this.CuiAfterUpdate(oldValue, value);
                        this.NotifyChangeOf(nameof(this.Cui));
                        this.IsDirty = true;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the corresponding name of the partner.
        /// </summary>
        /// <remarks>
        /// <para>This property may only be set for a newly created item, as indicated by the <see cref="EditableObject.IsNew"/> property.</para>
        /// </remarks>
        /// <exception cref="InvalidOperationException">An attempt has been made to set this property
        /// for an existing (non-new) item.</exception>
        /// <exception cref="ArgumentException">The specified value is not valid for this property.</exception>
        [DataMember]
        public string Name
        {
            get => this.data.name;
            set
            {
                if (!this.IsEditing)
                {
                    this.data.name = value;
                    return;
                }

                if (this.data.name != value)
                {
                    if (!this.IsNew)
                    {
                        throw ExceptionHelper.NewPropertySettableForNewException(nameof(this.Name));
                    }

                    this.NameBeforeUpdate(ref value);
                    this.ValidateProperty(nameof(this.Name), ref value);

                    if (this.data.name != value)
                    {
						var oldValue = this.data.name;

                        this.data.name = value;

                        this.NameAfterUpdate(oldValue, value);
                        this.Notify(ChangeOfProperty.Name);
                        this.IsDirty = true;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the corresponding code for the mapped partner. (CodFirma)
        /// </summary>
        /// <exception cref="ArgumentException">The specified value is not valid for this property.</exception>
        [DataMember]
        public string Code
        {
            get => this.data.code;
            set
            {
                if (!this.IsEditing)
                {
                    this.data.code = value;
                    return;
                }

                if (this.data.code != value)
                {
                    this.CodeBeforeUpdate(ref value);
                    this.ValidateProperty(nameof(this.Code), ref value);

                    if (this.data.code != value)
                    {
						var oldValue = this.data.code;

                        this.data.code = value;

                        this.CodeAfterUpdate(oldValue, value);
                        this.Notify(ChangeOfProperty.Code);
                        this.IsDirty = true;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the corresponding name for the mapped partner. (NumePartener)
        /// </summary>
        /// <exception cref="ArgumentException">The specified value is not valid for this property.</exception>
        [DataMember]
        public string PartnerName
        {
            get => this.data.partnerName;
            set
            {
                if (!this.IsEditing)
                {
                    this.data.partnerName = value;
                    return;
                }

                if (this.data.partnerName != value)
                {
                    this.PartnerNameBeforeUpdate(ref value);
                    this.ValidateProperty(nameof(this.PartnerName), ref value);

                    if (this.data.partnerName != value)
                    {
						var oldValue = this.data.partnerName;

                        this.data.partnerName = value;

                        this.PartnerNameAfterUpdate(oldValue, value);
                        this.NotifyChangeOf(nameof(this.PartnerName));
                        this.IsDirty = true;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the corresponding type of the mapped partner. (TipPartener)
        /// </summary>
        /// <exception cref="ArgumentException">The specified value is not valid for this property.</exception>
        [DataMember]
        public string PartnerType
        {
            get => this.data.partnerType;
            set
            {
                if (!this.IsEditing)
                {
                    this.data.partnerType = value;
                    return;
                }

                if (this.data.partnerType != value)
                {
                    this.PartnerTypeBeforeUpdate(ref value);
                    this.ValidateProperty(nameof(this.PartnerType), ref value);

                    if (this.data.partnerType != value)
                    {
						var oldValue = this.data.partnerType;

                        this.data.partnerType = value;

                        this.PartnerTypeAfterUpdate(oldValue, value);
                        this.NotifyChangeOf(nameof(this.PartnerType));
                        this.IsDirty = true;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets (CodGestDefault)
        /// </summary>
        /// <exception cref="ArgumentException">The specified value is not valid for this property.</exception>
        [DataMember]
        public string CodeGest
        {
            get => this.data.codeGest;
            set
            {
                if (!this.IsEditing)
                {
                    this.data.codeGest = value;
                    return;
                }

                if (this.data.codeGest != value)
                {
                    this.CodeGestBeforeUpdate(ref value);
                    this.ValidateProperty(nameof(this.CodeGest), ref value);

                    if (this.data.codeGest != value)
                    {
						var oldValue = this.data.codeGest;

                        this.data.codeGest = value;

                        this.CodeGestAfterUpdate(oldValue, value);
                        this.NotifyChangeOf(nameof(this.CodeGest));
                        this.IsDirty = true;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets (CodCenDefault)
        /// </summary>
        /// <exception cref="ArgumentException">The specified value is not valid for this property.</exception>
        [DataMember]
        public string CodeCen
        {
            get => this.data.codeCen;
            set
            {
                if (!this.IsEditing)
                {
                    this.data.codeCen = value;
                    return;
                }

                if (this.data.codeCen != value)
                {
                    this.CodeCenBeforeUpdate(ref value);
                    this.ValidateProperty(nameof(this.CodeCen), ref value);

                    if (this.data.codeCen != value)
                    {
						var oldValue = this.data.codeCen;

                        this.data.codeCen = value;

                        this.CodeCenAfterUpdate(oldValue, value);
                        this.NotifyChangeOf(nameof(this.CodeCen));
                        this.IsDirty = true;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the flags of the partner.
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
        /// Gets or sets the time stamp when the record was created.
        /// </summary>
        /// <exception cref="ArgumentException">The specified value is not valid for this property.</exception>
        [DataMember]
        public DateTime Ts
        {
            get => this.data.ts;
            set
            {
                if (!this.IsEditing)
                {
                    this.data.ts = value;
                    return;
                }

                if (this.data.ts != value)
                {
                    this.TsBeforeUpdate(ref value);
                    this.ValidateProperty(nameof(this.Ts), ref value);

                    if (this.data.ts != value)
                    {
						var oldValue = this.data.ts;

                        this.data.ts = value;

                        this.TsAfterUpdate(oldValue, value);
                        this.NotifyChangeOf(nameof(this.Ts));
                        this.IsDirty = true;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets (Utilizator)
        /// </summary>
        /// <exception cref="ArgumentException">The specified value is not valid for this property.</exception>
        [DataMember]
        public string User
        {
            get => this.data.user;
            set
            {
                if (!this.IsEditing)
                {
                    this.data.user = value;
                    return;
                }

                if (this.data.user != value)
                {
                    this.UserBeforeUpdate(ref value);
                    this.ValidateProperty(nameof(this.User), ref value);

                    if (this.data.user != value)
                    {
						var oldValue = this.data.user;

                        this.data.user = value;

                        this.UserAfterUpdate(oldValue, value);
                        this.NotifyChangeOf(nameof(this.User));
                        this.IsDirty = true;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating automatic reception (if there are large ones and it is possible),
        /// this being one of the bits of the <see cref="Flag"/> property.
        /// </summary>
        [IgnoreDataMember]
        public bool AutoReception
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

                if (this.AutoReception != value)
                {
                    this.AutoReceptionBeforeUpdate(ref value);
                    this.ValidateProperty(nameof(this.AutoReception), ref value);

					bool oldValue = this.AutoReception;

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

                        this.AutoReceptionAfterUpdate(oldValue, value);
                        this.NotifyChangeOf(nameof(this.AutoReception));
                        this.Notify(ChangeOfProperty.Flag);
                        this.IsDirty = true;
                    }
                }
            }
        }		

        /// <summary>
        /// Gets or sets a value indicating automatic takeover of delivery places,
        /// this being one of the bits of the <see cref="Flag"/> property.
        /// </summary>
        [IgnoreDataMember]
        public bool AutoDelivery
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

                if (this.AutoDelivery != value)
                {
                    this.AutoDeliveryBeforeUpdate(ref value);
                    this.ValidateProperty(nameof(this.AutoDelivery), ref value);

					bool oldValue = this.AutoDelivery;

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

                        this.AutoDeliveryAfterUpdate(oldValue, value);
                        this.NotifyChangeOf(nameof(this.AutoDelivery));
                        this.Notify(ChangeOfProperty.Flag);
                        this.IsDirty = true;
                    }
                }
            }
        }		

        /// <summary>
        /// Gets or sets a value indicating automatic retrieval of articles,
        /// this being one of the bits of the <see cref="Flag"/> property.
        /// </summary>
        [IgnoreDataMember]
        public bool AutoArticle
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

                if (this.AutoArticle != value)
                {
                    this.AutoArticleBeforeUpdate(ref value);
                    this.ValidateProperty(nameof(this.AutoArticle), ref value);

					bool oldValue = this.AutoArticle;

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

                        this.AutoArticleAfterUpdate(oldValue, value);
                        this.NotifyChangeOf(nameof(this.AutoArticle));
                        this.Notify(ChangeOfProperty.Flag);
                        this.IsDirty = true;
                    }
                }
            }
        }		

        /// <summary>
        /// Gets or sets a value indicating codArtClient with own CodArts,
        /// this being one of the bits of the <see cref="Flag"/> property.
        /// </summary>
        [IgnoreDataMember]
        public bool HasOwnCodArts
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

                if (this.HasOwnCodArts != value)
                {
                    this.HasOwnCodArtsBeforeUpdate(ref value);
                    this.ValidateProperty(nameof(this.HasOwnCodArts), ref value);

					bool oldValue = this.HasOwnCodArts;

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

                        this.HasOwnCodArtsAfterUpdate(oldValue, value);
                        this.NotifyChangeOf(nameof(this.HasOwnCodArts));
                        this.Notify(ChangeOfProperty.Flag);
                        this.IsDirty = true;
                    }
                }
            }
        }		

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void CuiBeforeUpdate(ref string value);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void CuiAfterUpdate(string oldValue, string newValue);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void NameBeforeUpdate(ref string value);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void NameAfterUpdate(string oldValue, string newValue);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void CodeBeforeUpdate(ref string value);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void CodeAfterUpdate(string oldValue, string newValue);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void PartnerNameBeforeUpdate(ref string value);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void PartnerNameAfterUpdate(string oldValue, string newValue);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void PartnerTypeBeforeUpdate(ref string value);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void PartnerTypeAfterUpdate(string oldValue, string newValue);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void CodeGestBeforeUpdate(ref string value);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void CodeGestAfterUpdate(string oldValue, string newValue);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void CodeCenBeforeUpdate(ref string value);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void CodeCenAfterUpdate(string oldValue, string newValue);

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
        partial void TsBeforeUpdate(ref DateTime value);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void TsAfterUpdate(DateTime oldValue, DateTime newValue);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void UserBeforeUpdate(ref string value);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void UserAfterUpdate(string oldValue, string newValue);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void AutoReceptionBeforeUpdate(ref bool value);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void AutoReceptionAfterUpdate(bool oldValue, bool newValue);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void AutoDeliveryBeforeUpdate(ref bool value);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void AutoDeliveryAfterUpdate(bool oldValue, bool newValue);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void AutoArticleBeforeUpdate(ref bool value);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void AutoArticleAfterUpdate(bool oldValue, bool newValue);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void HasOwnCodArtsBeforeUpdate(ref bool value);

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
        partial void HasOwnCodArtsAfterUpdate(bool oldValue, bool newValue);

        #region Data class

        /// <summary>
        /// Class for storing the property values of the <see cref="ServerE"/> instance.
        /// </summary>
        private partial class Data
        {
            /// <summary>
            /// The unique identifier of the <see cref="Server"/>. (CUI) It's coming from the digiDoc (XML)
            /// </summary>
            public string cui;

            /// <summary>
            /// The corresponding name of the partner.
            /// </summary>
            public string name;

            /// <summary>
            /// The corresponding code for the mapped partner. (CodFirma)
            /// </summary>
            public string code;

            /// <summary>
            /// The corresponding name for the mapped partner. (NumePartener)
            /// </summary>
            public string partnerName;

            /// <summary>
            /// The corresponding type of the mapped partner. (TipPartener)
            /// </summary>
            public string partnerType;

            /// <summary>
            /// (CodGestDefault)
            /// </summary>
            public string codeGest;

            /// <summary>
            /// (CodCenDefault)
            /// </summary>
            public string codeCen;

            /// <summary>
            /// The flags of the partner.
            /// </summary>
            public int flag;

            /// <summary>
            /// The time stamp when the record was created.
            /// </summary>
            public DateTime ts;

            /// <summary>
            /// (Utilizator)
            /// </summary>
            public string user;

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
                this.cui = data.cui;
                this.name = data.name;
                this.code = data.code;
                this.partnerName = data.partnerName;
                this.partnerType = data.partnerType;
                this.codeGest = data.codeGest;
                this.codeCen = data.codeCen;
                this.flag = data.flag;
                this.ts = data.ts;
                this.user = data.user;
            }
        }

        #endregion
    }
}

