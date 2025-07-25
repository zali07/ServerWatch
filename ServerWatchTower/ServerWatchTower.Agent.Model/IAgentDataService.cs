﻿namespace ServerWatchTower.Agent.Model
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// The interface of the core data loading/storing services.
    /// </summary>
    public interface IAgentDataService
    {
        #region Server

        /// <summary>
        /// Deletes the server asynchronously.
        /// </summary>
        /// <param name="serverGUID">The GUID of the server to be deleted.</param>
        Task DeleteServerAsync(string serverGUID);

        /// <summary>
        /// Starts a task of loading a specific <see cref="Server"/> from the database.
        /// </summary>
        /// <param name="serverGUID">The <see cref="Server.GUID"/> of the <see cref="Server"/> to be loaded.</param>
        /// <returns>A task with its result being the appropriate <see cref="Server"/> instance.</returns>
        Task<Server> GetServerAsync(string serverGUID);

        /// <summary>
        /// Starts a task of loading the first few <see cref="Server"/>s which match the specified criteria from the database.
        /// </summary>
        /// <param name="filter">The filtering criteria which indicates which <see cref="Server"/>s should be loaded.</param>
        /// <returns>A task with its result being the list of the appropriate <see cref="Server"/> instances and the tooltip for the state.</returns>
        /// <remarks>
        /// <para>The <see cref="Task{T}"/> will load only the first few (e.g. first 100) of the corresponding servers.</para>
        /// </remarks>
        /// <seealso cref="ServerCatalog"/>
        Task<(List<Server> Servers, string controlTipText)> GetServersAsync(ServerCatalogFilterArgs filter);

        /// <summary>
        /// Loads the data of a server in an editable <see cref="ServerE"/> instance asynchronously for displaying/editing.
        /// </summary>
        /// <param name="serverGUID">The GUID of the server.</param>
        /// <returns>A <see cref="Task{TResult}"/> with its <see cref="Task{TResult}.Result"/> being
        /// the data of the server in a new <see cref="ServerE"/> instance.</returns>
        Task<ServerE> LoadServerEAsync(string serverGUID);

        /// <summary>
        /// Saves the data of the server asynchronously.
        /// </summary>
        /// <param name="server">The data of the server to be saved in an editable <see cref="ServerE"/> instance.</param>
        Task<string> SaveServerAsync(ServerE server);

        #endregion Server

        #region Alert

        /// <summary>
        /// Starts a task of loading the list of the current alerts from the database.
        /// </summary>
        Task<List<Alert>> GetAlertsAsync();

        /// <summary>
        /// Starts a task of loading the list of the alerts history from the database.
        /// </summary>
        Task<List<Alert>> GetAlertsHistoryAsync();

        /// <summary>
        /// Acknowledges the alert with the specified ID asynchronously.
        /// </summary>
        /// <param name="alertId"></param>
        Task AcknowledgeAlertAsync(int alertId);

        #endregion Alert

        #region Report

        /// <summary>
        /// Fetches the statuses of all server components asynchronously.
        /// </summary>
        /// <returns>A task with a list of server component statuses.</returns>
        Task<List<ServerComponentStatus>> GetServerComponentStatusesAsync();

        #endregion Report

        #region Diagram

        Task<List<MirroringEntry>> GetMirroringEntriesAsync();

        Task<List<DriverEntry>> GetDriverEntriesAsync();

        Task<List<BackupEntry>> GetBackupEntriesAsync();

        Task<List<MirroringEntry>> GetMirroringEntriesAsync(string guid);

        Task<List<DriverEntry>> GetDriverEntriesAsync(string guid);

        Task<List<BackupEntry>> GetBackupEntriesAsync(string guid);

        Task<List<ChartDataPoint>> GetDiagramDataAsync(string guid, string type, DateTime startD, DateTime endD);

        #endregion Diagram
    }
}
