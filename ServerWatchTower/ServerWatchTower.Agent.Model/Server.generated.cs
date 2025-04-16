//-----------------------------------------------------------------------
// <copyright file="Server.generated.cs" company="Cosys SRL.">
//     Copyright (c) 2012, 2025 Cosys SRL. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ServerWatchTower.Agent.Model
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.Serialization;
    using Cosys.SilverLib.Model;

    /// <summary>
    /// Data class containing the main data of a server.
    /// </summary>
    [DataContract(Namespace = "http://software.cosys.ro/SilverERP/V1/Agent")]
    public partial class Server : IFreezableData, IEquatable<Server>, IKeyedData<string>
    {
        #region Private fields

        /// <summary>
        /// The unique identifier of the <see cref="Server"/>. (CUI) It's coming from the digiDoc (XML)
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string cui;

        /// <summary>
        /// The corresponding name of the partner.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string name;

        /// <summary>
        /// The corresponding code for the mapped partner. (CodFirma)
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string code;

        /// <summary>
        /// The corresponding name for the mapped partner. (NumePartener)
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string partnerName;

        /// <summary>
        /// The corresponding type of the mapped partner. (TipPartener)
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string partnerType;

        /// <summary>
        /// (CodGestDefault)
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string codeGest;

        /// <summary>
        /// (CodCenDefault)
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string codeCen;

        /// <summary>
        /// The flags of the partner.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int flag;

        /// <summary>
        /// The time stamp when the record was created or last modified.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime ts;

        /// <summary>
        /// (Utilizator)
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string user;

        /// <summary>
        /// The state of the mapped partner.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string state;

        /// <summary>
        /// Indicates whether the object is frozen.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool isFrozen;

        #endregion

        /// <summary>
        /// Gets or sets the unique identifier of the <see cref="Server"/>. (CUI) It's coming from the digiDoc (XML)
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Cui
        {
            get => this.cui;
            set => this.AssertNotFrozen().cui = value;
        }

        /// <summary>
        /// Gets or sets the corresponding name of the partner.
        /// </summary>
        [DataMember]
        public string Name
        {
            get => this.name;
            set => this.AssertNotFrozen().name = value;
        }

        /// <summary>
        /// Gets or sets the corresponding code for the mapped partner. (CodFirma)
        /// </summary>
        [DataMember]
        public string Code
        {
            get => this.code;
            set => this.AssertNotFrozen().code = value;
        }

        /// <summary>
        /// Gets or sets the corresponding name for the mapped partner. (NumePartener)
        /// </summary>
        [DataMember]
        public string PartnerName
        {
            get => this.partnerName;
            set => this.AssertNotFrozen().partnerName = value;
        }

        /// <summary>
        /// Gets or sets the corresponding type of the mapped partner. (TipPartener)
        /// </summary>
        [DataMember]
        public string PartnerType
        {
            get => this.partnerType;
            set => this.AssertNotFrozen().partnerType = value;
        }

        /// <summary>
        /// Gets or sets (CodGestDefault)
        /// </summary>
        [DataMember]
        public string CodeGest
        {
            get => this.codeGest;
            set => this.AssertNotFrozen().codeGest = value;
        }

        /// <summary>
        /// Gets or sets (CodCenDefault)
        /// </summary>
        [DataMember]
        public string CodeCen
        {
            get => this.codeCen;
            set => this.AssertNotFrozen().codeCen = value;
        }

        /// <summary>
        /// Gets or sets the flags of the partner.
        /// </summary>
        [DataMember]
        public int Flag
        {
            get => this.flag;
            set => this.AssertNotFrozen().flag = value;
        }

        /// <summary>
        /// Gets or sets the time stamp when the record was created or last modified.
        /// </summary>
        [DataMember]
        public DateTime Ts
        {
            get => this.ts;
            set => this.AssertNotFrozen().ts = value;
        }

        /// <summary>
        /// Gets or sets (Utilizator)
        /// </summary>
        [DataMember]
        public string User
        {
            get => this.user;
            set => this.AssertNotFrozen().user = value;
        }

        /// <summary>
        /// Gets or sets the state of the mapped partner.
        /// </summary>
        [DataMember]
        public string State
        {
            get => this.state;
            set => this.AssertNotFrozen().state = value;
        }

        /// <summary>
        /// Gets a value indicating automatic reception (if there are large ones and it is possible),
        /// this being one of the bits of the <see cref="Flag"/> property.
        /// </summary>
        [IgnoreDataMember]
        public bool AutoReception => (this.flag & 1) != 0;

        /// <summary>
        /// Gets a value indicating automatic takeover of delivery places,
        /// this being one of the bits of the <see cref="Flag"/> property.
        /// </summary>
        [IgnoreDataMember]
        public bool AutoDelivery => (this.flag & 2) != 0;

        /// <summary>
        /// Gets a value indicating automatic retrieval of articles,
        /// this being one of the bits of the <see cref="Flag"/> property.
        /// </summary>
        [IgnoreDataMember]
        public bool AutoArticle => (this.flag & 4) != 0;

        /// <summary>
        /// Gets a value indicating codArtClient with own CodArts,
        /// this being one of the bits of the <see cref="Flag"/> property.
        /// </summary>
        [IgnoreDataMember]
        public bool HasOwnCodArts => (this.flag & 8) != 0;

        /// <summary>
        /// Gets a value indicating whether the object is frozen.
        /// </summary>
        /// <seealso cref="Freeze"/>
        [IgnoreDataMember]
        public bool IsFrozen => this.isFrozen;

        /// <summary>
        /// Freezes the object, so it could no longer be updated.
        /// </summary>
        /// <remarks>
        /// <para>Once the object is initialized it should be frozen, so its content could no longer be updated.</para>
        /// </remarks>
        /// <returns>The object on which the method was called.</returns>
        /// <seealso cref="IsFrozen"/>
        public Server Freeze()
        {
            if (!this.isFrozen)
            {
                this.PrepareToFreeze();
                this.isFrozen = true;
            }

            return this;
        }

        void IFreezableData.Freeze()
        {
            if (!this.isFrozen)
            {
                this.PrepareToFreeze();
                this.isFrozen = true;
            }
        }

        /// <summary>
        /// Indicates whether the current object is equal to another <see cref="Server"/> instance, with the
        /// exception of the <see cref="IsFrozen"/> property.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter;
        /// otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// <para>The method compares the properties of the objects individually, with the exception
        /// of the <see cref="IsFrozen"/> property, to determine whether the objects are equal.</para>
        /// </remarks>
        public bool Equals(Server other)
        {
			if (other is null)
			{
				return false;
			}

			if (ReferenceEquals(this, other))
			{
				return true;
			}

			return this.cui == other.cui
				&& this.name == other.name
				&& this.code == other.code
				&& this.partnerName == other.partnerName
				&& this.partnerType == other.partnerType
				&& this.codeGest == other.codeGest
				&& this.codeCen == other.codeCen
				&& this.flag == other.flag
				&& this.ts == other.ts
				&& this.user == other.user
				&& this.state == other.state;
        }

        /// <summary>
        /// Checks whether two <see cref="Server"/> instances are equal, without considering
		/// their <see cref="IsFrozen"/> property.
        /// </summary>
        /// <param name="obj1">The first object.</param>
        /// <param name="obj2">The second object.</param>
        /// <returns><c>true</c> if the two objects are equal; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// <para>The method compares the properties of the objects individually, with the exception
        /// of the <see cref="IsFrozen"/> property, to determine whether the objects are equal.</para>
        /// </remarks>		
		public static bool operator ==(Server obj1, Server obj2)
		{
			if (obj1 is null)
			{
				return obj2 is null;
			}

			return obj1.Equals(obj2);        
		}

        /// <summary>
        /// Checks whether two <see cref="Server"/> instances are different, without considering
		/// their <see cref="IsFrozen"/> property.
        /// </summary>
        /// <param name="obj1">The first object.</param>
        /// <param name="obj2">The second object.</param>
        /// <returns><c>true</c> if the two objects are different; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// <para>The method compares the properties of the objects individually, with the exception
        /// of the <see cref="IsFrozen"/> property, to determine whether the objects are different.</para>
        /// </remarks>		
		public static bool operator !=(Server obj1, Server obj2)
		{
			if (obj1 is null)
			{
				return !(obj2 is null);
			}

			return !obj1.Equals(obj2);
		}

        /// <summary>
        /// Indicates whether the current object is equal to another one, with the
        /// exception of the <see cref="IsFrozen"/> property.
        /// </summary>
        /// <param name="obj">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="obj"/> parameter;
        /// otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// <para>The method compares the properties of the objects individually, with the exception
        /// of the <see cref="IsFrozen"/> property, to determine whether the objects are equal.</para>
        /// </remarks>
        public override bool Equals(object obj) => obj is Server server && this.Equals(server);
		
        /// <summary>
        /// Gets the hash code for the <see cref="Server"/> instance,
        /// which does not depend on the <see cref="isFrozen"/> property.
        /// </summary>
        /// <returns>The hash code of the object.</returns>
        public override int GetHashCode()
        {
            int hashCode = -842568011;
            
#pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'

            if (this.cui != null)
            {
                hashCode ^= this.cui.GetHashCode();
            }

            if (this.name != null)
            {
                hashCode ^= this.name.GetHashCode();
            }

            if (this.code != null)
            {
                hashCode ^= this.code.GetHashCode();
            }

            if (this.partnerName != null)
            {
                hashCode ^= this.partnerName.GetHashCode();
            }

            if (this.partnerType != null)
            {
                hashCode ^= this.partnerType.GetHashCode();
            }

            if (this.codeGest != null)
            {
                hashCode ^= this.codeGest.GetHashCode();
            }

            if (this.codeCen != null)
            {
                hashCode ^= this.codeCen.GetHashCode();
            }

            if (this.flag != null)
            {
                hashCode ^= this.flag.GetHashCode();
            }

            if (this.ts != null)
            {
                hashCode ^= this.ts.GetHashCode();
            }

            if (this.user != null)
            {
                hashCode ^= this.user.GetHashCode();
            }

            if (this.state != null)
            {
                hashCode ^= this.state.GetHashCode();
            }

#pragma warning restore CS0472

            return hashCode;
        }

        /// <summary>
        /// Gets the key of the data instance.
        /// </summary>
        /// <returns>The key of the data instance.</returns>
        string IKeyedData<string>.GetKey()
        {
            return this.cui;
        }

        /// <summary>
        /// Gets the enumeration of the property names, which make up the key.
        /// </summary>
        /// <returns>The enumeration of the property names which make up the key.</returns>
        IEnumerable<string> IKeyedData<string>.GetKeyProperties()
        {
            return new SingleItemEnumerator<string>("Cui");
        }

        /// <summary>
        /// Checks whether the object is frozen, and throws an <see cref="InvalidOperationException"/> when it is.
        /// </summary>
		/// <returns>The object on which the method was called.</returns>
        private Server AssertNotFrozen()
        {
            if (this.isFrozen)
            {
                throw ExceptionHelper.NewObjectFrozenException("Server");
            }

			return this;
        }

        /// <content>
        /// This method will be ignored unless it gets implemented in another place.
        /// </content>
		partial void PrepareToFreeze();
    }
}

