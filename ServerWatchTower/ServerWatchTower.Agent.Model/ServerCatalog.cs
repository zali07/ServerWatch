//-----------------------------------------------------------------------
// <copyright file="PartnerMappingCatalog.cs" company="Cosys SRL.">
//     Copyright (c) 2012, 2024 Cosys SRL. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ServerWatchTower.Agent.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Threading.Tasks;
    using Cosys.SilverLib.Model;

    /// <summary>
    /// The catalog of the <see cref="Server"/>s of the Registry module.
    /// </summary>
    [Export, Export("ServerWatchTower.Agent.ServerCatalog", typeof(ICatalog)), PartCreationPolicy(CreationPolicy.Shared)]
    public class ServerCatalog : LargeCatalog<string, Server, ServerCatalogFilterArgs>
    {
        /// <summary>
        /// Gets the <see cref="IEqualityComparer{T}"/> which can be used to check whether two keys
        /// identify the same item from the catalog.
        /// </summary>
        public static IEqualityComparer<string> KeyComparer => StringComparer.CurrentCultureIgnoreCase;

        #region Imports

        /// <summary>
        /// Gets or sets the <see cref="IAgentDataService"/> implementation to be used by the catalog.
        /// </summary>
        [Import]
        public IAgentDataService AgentDataService { get; set; }

        /// <summary>
        /// Gets or sets the configuration settings of the Core module.
        /// </summary>
        [Import]
        public AgentConfiguration Config { get; set; }

        #endregion

        /// <summary>
        /// Gets a string containing the indicators of the state of the partner mapping.
        /// </summary>
        public string StateIndicators { get; set; }

        /// <summary>
        /// Gets a value indicating whether the two specified item keys are equal, according to the comparison used be the catalog.
        /// </summary>
        /// <param name="key1">The first item key.</param>
        /// <param name="key2">The second item key.</param>
        /// <returns>True when both keys are <c>null</c>, or they would identify the same item from the catalog.</returns>
        /// <remarks>
        /// <para>This method uses the <see cref="KeyComparer"/> comparer of the catalog to compare the keys.</para>
        /// </remarks>
        public override bool AreKeysEqual(string key1, string key2)
        {
            return KeyComparer.Equals(key1, key2);
        }

        /// <summary>
        /// Creates a new catalog collection for storing the data of the catalog.
        /// </summary>
        /// <returns>The newly created <see cref="CatalogCollection{TKey,TItem}"/> instance for storing the data of the catalog.</returns>
        protected override CatalogCollection<string, Server> CreateContentsCollection()
        {
            return new CatalogCollection<string, Server>(KeyComparer);
        }

        /// <summary>
        /// Starts a task of loading a single item of the collection with the specified key from the database.
        /// </summary>
        /// <param name="dataKey">The key of the data item to load from the database.</param>
        /// <returns>A task with its result being the data item loaded from the database.</returns>
        protected override async Task<Server> StartLoadingData(string dataKey)
        {
            var partner = await this.AgentDataService.GetServerAsync(dataKey);

            return partner;
        }

        /// <summary>
        /// Starts a task of loading the first items which correspond to the specified filtering criteria from the database.
        /// </summary>
        /// <param name="dataFilter">The filtering criteria which specifies which items should be loaded.</param>
        /// <returns>A task with its result being the list of the appropriate data.</returns>
        /// <remarks>
        /// <para>The task will only load the first few (for example the first 100) items from the database. 
        /// When additional items will be needed, a different filtering criteria will be formulated to access those.</para>
        /// </remarks>
        protected override async Task<List<Server>> StartLoadingData(ServerCatalogFilterArgs dataFilter)
        {
            var result = await this.AgentDataService.GetServersAsync(dataFilter);

            this.StateIndicators = result.controlTipText;

            return result.Servers;
        }

        /// <summary>
        /// Loads the data of a partner into a new <see cref="ServerE"/> instance asynchronously.
        /// </summary>
        /// <param name="partnerCui">The unique code of the partner to load for editing.</param>
        /// <returns>A <see cref="Task{TResult}"/> with its <see cref="Task{TResult}.Result"/> being an
        /// editable <see cref="ServerE"/> instance with the data of the specified partner.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="partnerCui"/> is <c>null</c>.</exception>
        public async Task<ServerE> GetEditableItemAsync(string partnerCui)
        {
            if (partnerCui == null)
            {
                throw new ArgumentNullException(nameof(partnerCui));
            }

            var partner = await this.AgentDataService.LoadServerEAsync(partnerCui);

            partner.RegisterContext();

            return partner;
        }

        /// <summary>
        /// Saves the new or updated partner registration into the database and then refreshes it in the collection asynchronously.
        /// </summary>
        /// <param name="server">The editable <see cref="ServerE"/> instance containing the data to be saved.</param>
        /// <returns>A <see cref="Task{TResult}"/> with its <see cref="Task{TResult}.Result"/> being the
        /// code of the partner saved into the database.</returns>
        public async Task<string> SaveItemAsync(ServerE server)
        {
            if (server == null)
            {
                throw new ArgumentNullException(nameof(server));
            }

            return await this.AgentDataService.SaveServerAsync(server);
        }
    }
}
