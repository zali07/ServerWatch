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
        /// The unique identifier of the <see cref="Server"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int id;

        /// <summary>
        /// The corresponding GUID of the server.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string gUID;

        /// <summary>
        /// The corresponding publicKey for the server used for authentication.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string publicKey;

        /// <summary>
        /// The corresponding partner for the server.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string partner;

        /// <summary>
        /// The corresponding server name of the server.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string serverName;

        /// <summary>
        /// The corresponding windows version of the server.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string windows;

        /// <summary>
        /// If the server was approved for telemetric data gathering by an administrator.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool isApproved;

        /// <summary>
        /// The flags of the server.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int flag;

        /// <summary>
        /// Indicates whether the object is frozen.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool isFrozen;

        #endregion

        /// <summary>
        /// Gets or sets the unique identifier of the <see cref="Server"/>.
        /// </summary>
        [DataMember]
        public int Id
        {
            get => this.id;
            set => this.AssertNotFrozen().id = value;
        }

        /// <summary>
        /// Gets or sets the corresponding GUID of the server.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string GUID
        {
            get => this.gUID;
            set => this.AssertNotFrozen().gUID = value;
        }

        /// <summary>
        /// Gets or sets the corresponding publicKey for the server used for authentication.
        /// </summary>
        [DataMember]
        public string PublicKey
        {
            get => this.publicKey;
            set => this.AssertNotFrozen().publicKey = value;
        }

        /// <summary>
        /// Gets or sets the corresponding partner for the server.
        /// </summary>
        [DataMember]
        public string Partner
        {
            get => this.partner;
            set => this.AssertNotFrozen().partner = value;
        }

        /// <summary>
        /// Gets or sets the corresponding server name of the server.
        /// </summary>
        [DataMember]
        public string ServerName
        {
            get => this.serverName;
            set => this.AssertNotFrozen().serverName = value;
        }

        /// <summary>
        /// Gets or sets the corresponding windows version of the server.
        /// </summary>
        [DataMember]
        public string Windows
        {
            get => this.windows;
            set => this.AssertNotFrozen().windows = value;
        }

        /// <summary>
        /// Gets or sets if the server was approved for telemetric data gathering by an administrator.
        /// </summary>
        [DataMember]
        public bool IsApproved
        {
            get => this.isApproved;
            set => this.AssertNotFrozen().isApproved = value;
        }

        /// <summary>
        /// Gets or sets the flags of the server.
        /// </summary>
        [DataMember]
        public int Flag
        {
            get => this.flag;
            set => this.AssertNotFrozen().flag = value;
        }

        /// <summary>
        /// Gets a value indicating if the server was approved for telemetric data gathering by an administrator,
        /// this being one of the bits of the <see cref="Flag"/> property.
        /// </summary>
        [IgnoreDataMember]
        public bool isApproved1 => (this.flag & 1) != 0;

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

			return this.id == other.id
				&& this.gUID == other.gUID
				&& this.publicKey == other.publicKey
				&& this.partner == other.partner
				&& this.serverName == other.serverName
				&& this.windows == other.windows
				&& this.isApproved == other.isApproved
				&& this.flag == other.flag;
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

            if (this.id != null)
            {
                hashCode ^= this.id.GetHashCode();
            }

            if (this.gUID != null)
            {
                hashCode ^= this.gUID.GetHashCode();
            }

            if (this.publicKey != null)
            {
                hashCode ^= this.publicKey.GetHashCode();
            }

            if (this.partner != null)
            {
                hashCode ^= this.partner.GetHashCode();
            }

            if (this.serverName != null)
            {
                hashCode ^= this.serverName.GetHashCode();
            }

            if (this.windows != null)
            {
                hashCode ^= this.windows.GetHashCode();
            }

            if (this.isApproved != null)
            {
                hashCode ^= this.isApproved.GetHashCode();
            }

            if (this.flag != null)
            {
                hashCode ^= this.flag.GetHashCode();
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
            return this.gUID;
        }

        /// <summary>
        /// Gets the enumeration of the property names, which make up the key.
        /// </summary>
        /// <returns>The enumeration of the property names which make up the key.</returns>
        IEnumerable<string> IKeyedData<string>.GetKeyProperties()
        {
            return new SingleItemEnumerator<string>("GUID");
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

