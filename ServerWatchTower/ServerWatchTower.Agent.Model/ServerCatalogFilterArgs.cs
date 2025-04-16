namespace ServerWatchTower.Agent.Model
{
    using System;
    using System.Runtime.Serialization;
    using Cosys.SilverLib.Model;

    /// <summary>
    /// Class defining the filtering arguments passed to the <see cref="ServerCatalog"/>.
    /// </summary>
    /// <remarks>
    /// <para>The instances of this class may not be updated after being initialized, or otherwise they will cause problems 
    /// during the catalog loading operations.</para>
    /// </remarks>
    [DataContract(Namespace = "http://software.cosys.ro/SilverERP/V1/Core")]
    public class ServerCatalogFilterArgs : CatalogFilterArgs<Server>
    {
        #region Private fields

        /// <summary>
        /// Indicates whether the <see cref="CatalogFilterArgs{TData}.QuickFilter"/> should be looked for anywhere within the name of the
        /// partner, and not just from its start.
        /// </summary>
        private bool? quickSearchWithinName;

        /// <summary>
        /// Indicates whether the <see cref="CatalogFilterArgs{TData}.QuickFilter"/> should be looked for
        /// in the tax code of the partner too.
        /// </summary>
        private bool? quickSearchInTaxCode;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerCatalogFilterArgs"/> class.
        /// </summary>
        public ServerCatalogFilterArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerCatalogFilterArgs"/> class with
        /// the specified base filtering arguments.
        /// </summary>
        /// <param name="baseFilter">A <see cref="ServerCatalogFilterArgs"/> instance from which the base
        /// filtering arguments will be taken over into the new instance.</param>
        /// <remarks>
        /// <para>The <see cref="CatalogFilterArgs{T}.QuickFilter"/> property is not copied to the new instance.</para>
        /// </remarks>
        public ServerCatalogFilterArgs(ServerCatalogFilterArgs baseFilter)
        {
            if (baseFilter != null)
            {
                this.quickSearchWithinName = baseFilter.quickSearchWithinName;
                this.quickSearchInTaxCode = baseFilter.quickSearchInTaxCode;
            }
        }

        #endregion

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="CatalogFilterArgs{TData}.QuickFilter"/> 
        /// should be looked for anywhere within the name of the partner, and not just from its start.
        /// </summary>
        [DataMember]
        public bool? QuickSearchWithinName
        {
            get => this.quickSearchWithinName;

            set
            {
                this.AssertNotLocked();
                this.quickSearchWithinName = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="CatalogFilterArgs{TData}.QuickFilter"/> should be looked for
        /// in the tax code of the partner too.
        /// </summary>
        [DataMember]
        public bool? QuickSearchInTaxCode
        {
            get => this.quickSearchInTaxCode;

            set
            {
                this.AssertNotLocked();
                this.quickSearchInTaxCode = value;
            }
        }

        /// <summary>
        /// Clones a new locked instance with the <see cref="CatalogFilterArgs{T}.QuickFilter"/> property cleared
        /// or returns the current one if it suffices these criteria.
        /// </summary>
        /// <returns>This or the newly created <see cref="ServerCatalogFilterArgs"/> instance cloned
        /// from this, which is locked as has its <see cref="CatalogFilterArgs{T}.QuickFilter"/> cleared.</returns>
        public ServerCatalogFilterArgs StripAndLock()
        {
            if (this.IsLocked && string.IsNullOrEmpty(this.QuickFilter))
            {
                return this;
            }
            else
            {
                var clone = new ServerCatalogFilterArgs(this);

                clone.Lock();

                return clone;
            }
        }

        /// <summary>
        /// Checks whether a <see cref="Server"/> corresponds to the filtering condition defined by this instance.
        /// </summary>
        /// <param name="server">The partner to check.</param>
        /// <returns>True when the <paramref name="server"/> corresponds to the filtering condition; false otherwise.</returns>
        public override bool IsMatch(Server server)
        {
            if (server == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(this.QuickFilter))
            {
                return true;
            }

            if (this.quickSearchInTaxCode.GetValueOrDefault(true))
            {
                if (server.Cui.Contains(this.QuickFilter, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }

            if (this.quickSearchWithinName.GetValueOrDefault(true))
            {
                return server.Name.HasWordStartingWith(this.QuickFilter, StringComparison.CurrentCultureIgnoreCase);
            }
            else
            {
                return server.Name.StartsWith(this.QuickFilter, StringComparison.CurrentCultureIgnoreCase);
            }
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.GetBaseHashCode()
                ^ (this.quickSearchWithinName.HasValue ? (this.quickSearchWithinName.GetValueOrDefault() ? 2 : 0) : 8)
                ^ (this.quickSearchInTaxCode.HasValue ? (this.quickSearchInTaxCode.GetValueOrDefault() ? 4 : 0) : 16);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is ServerCatalogFilterArgs filterArgs && this.Equals(filterArgs);
        }

        /// <summary>
        /// Determines whether the specified <see cref="ServerCatalogFilterArgs"/> is equal to this instance.
        /// </summary>
        /// <param name="filterArgs">The <see cref="ServerCatalogFilterArgs"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <paramref name="filterArgs"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(ServerCatalogFilterArgs filterArgs)
        {
            if (filterArgs == null)
            {
                return false;
            }

            return this.BaseEquals(filterArgs)
                && this.quickSearchWithinName == filterArgs.quickSearchWithinName
                && this.quickSearchInTaxCode == filterArgs.quickSearchInTaxCode;
        }
    }
}
