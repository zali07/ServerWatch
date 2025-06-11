namespace ServerWatchTower.Agent.Data
{
    using Cosys.SilverLib.Data;
    using Cosys.SilverLib.Model;
    using ServerWatchTower.Agent.Model;
    using Microsoft.SqlServer.Server;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading;
    using System.Xml;

    /// <summary>
    /// The specialized <see cref="DataLayer"/> for the Agent module of ServerWatchTower.
    /// </summary>
    internal class AgentDataLayer : DataLayer
    {

        #region Private fields

        /// <summary>
        /// The field behind the <see cref="ListOfIntSqlType"/> property.
        /// </summary>
        private static DataRecordEnumerator<int> _listOfIntSqlType;

        /// <summary>
        /// The field behind the <see cref="ServerSqlType"/> property.
        /// </summary>
        private static DataRecordEnumerator<ServerE> _serverSqlType;

        #endregion

        #region The constructors of the class

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentDataLayer"/> class with the specified connection string.
        /// </summary>
        /// <param name="connectionString">The Sql Server connection string to use by the data layer.</param>
        /// <remarks><para>The newly created <see cref="DataLayer"/> instance will not
        /// join any user session in the database.</para>
        /// <para>The <see cref="DataLayer.SqlConnection"/> will be closed, when the data layer
        /// gets disposed.</para></remarks>
        public AgentDataLayer(string connectionString)
            : base(connectionString, SessionTicket.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentDataLayer"/> class with the specified connection string settings.
        /// </summary>
        /// <param name="connectionProvider">The connection string settings to use
        /// for connecting to the database.</param>
        /// <remarks><para>The newly created <see cref="DataLayer"/> instance will not
        /// join any user session in the database.</para>
        /// <para>The <see cref="DataLayer.SqlConnection"/> will be closed, when the data layer
        /// gets disposed.</para></remarks>
        public AgentDataLayer(IConnectionStringProvider connectionProvider)
            : base(connectionProvider, SessionTicket.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentDataLayer"/> class by getting a connection string
        /// from the specified <see cref="IConnectionStringProvider"/>, and by optionally joining the appropriate session.
        /// </summary>
        /// <param name="connectionProvider">The <see cref="IConnectionStringProvider"/> implementation to be used
        /// to get the connection string for the data layer.</param>
        /// <param name="sessionProvider">The session provider which will specify the user session to be joined
        /// by the newly created data layer instance.</param>
        /// <remarks>
        /// <para>The constructor will check whether there is a <see cref="DataLayerConnectionAttribute"/> attribute
        /// on the data layer, and will use the connection with the name specified by the attribute, if it exists.</para>
        /// <para>When the <see cref="DataLayerConnectionAttribute"/> is not specified, the default connection string
        /// will be used by the newly created data layer, as returned by the <paramref name="connectionProvider"/>.</para>
        /// <para>The user session will be left, when the data layer gets disposed, and the underlying
        /// <see cref="System.Data.SqlClient.SqlConnection"/> will be closed.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="connectionProvider"/> is null.</exception>
        /// <exception cref="ArgumentException">The user session specified by the <paramref name="sessionProvider"/>
        /// does not contain a valid ticket, and cannot be used for direct database access.</exception>
        public AgentDataLayer(IConnectionStringProvider connectionProvider, ISessionProvider sessionProvider)
            : base(connectionProvider, sessionProvider)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentDataLayer"/> class
        /// by taking over the connection and session context from an other data layer instance.
        /// </summary>
        /// <param name="dataLayer">The instance from which the connection and the session context will be
        /// taken over.</param>
        /// <remarks>
        /// <para>Once the connection and the session context is taken over from the supplied <paramref name="dataLayer"/>,
        /// that data layer instance will get disposed, and can no longer be used. The database connection will be
        /// closed when the newly created data layer will be disposed.
        /// </para>
        /// </remarks>
        public AgentDataLayer(DataLayer dataLayer)
            : base(dataLayer)
        {
        }

        #endregion

        #region Sql table types

        /// <summary>
        /// Gets a <see cref="DataRecordEnumerator{TItem}"/> instance initialized for the <c>"dbo.dtListOfIntType"</c> Sql table type.
        /// </summary>
        private static DataRecordEnumerator<int> ListOfIntSqlType
        {
            get
            {
                if (_listOfIntSqlType == null)
                {
                    var s = new DataRecordEnumerator<int>(
                        "dbo.dtListOfIntType",
                        new[]
                        {
                            new SqlMetaData("Value", SqlDbType.Int),
                        },
                        (record, item) =>
                        {
                            record.SetInt32(0, item);
                        }
                    );

                    Interlocked.CompareExchange(ref _listOfIntSqlType, s, null);
                }

                return _listOfIntSqlType;
            }
        }

        /// <summary>
        /// Gets a <see cref="DataRecordEnumerator{TItem}"/> instance initialized for the <c>"dbo.dtServerType"</c> Sql table type.
        /// </summary>
        private static DataRecordEnumerator<ServerE> ServerSqlType
        {
            get
            {
                if (_serverSqlType == null)
                {
                    var s = new DataRecordEnumerator<ServerE>(
                        "dbo.ServerType",
                        new[]
                        {
                            new SqlMetaData("GUID", SqlDbType.NVarChar, 36),
                            new SqlMetaData("PublicKey", SqlDbType.NVarChar, -1),
                            new SqlMetaData("Partner", SqlDbType.NVarChar, -1),
                            new SqlMetaData("Server", SqlDbType.NVarChar, -1),
                            new SqlMetaData("Windows", SqlDbType.NVarChar, -1),
                            new SqlMetaData("BackupRoot", SqlDbType.NVarChar, 255),
                            new SqlMetaData("Flag", SqlDbType.Int),
                        },
                        (record, item, index) =>
                        {
                            record.SetValue(0, item.GUID);
                            record.SetValue(1, DbNull(item.PublicKey, false));
                            record.SetValue(2, DbNull(item.Partner, false));
                            record.SetValue(3, DbNull(item.ServerName, false));
                            record.SetValue(4, DbNull(item.Windows, false));
                            record.SetValue(5, DbNull(item.BackupRoot, false));
                            record.SetValue(6, item.Flag);
                        }
                    );

                    Interlocked.CompareExchange(ref _serverSqlType, s, null);
                }

                return _serverSqlType;
            }
        }

        #endregion

        /// <summary>
        /// Recognizes the type of a user defined Sql error, and creates an exception corresponding to it.
        /// </summary>
        /// <param name="errorNumber">The user defined Sql error number to recognize.</param>
        /// <param name="errorMessage">The error message which might have been received from the database.</param>
        /// <param name="sqlException">The <see cref="SqlException"/> received from the Sql server.</param>
        /// <returns>An exception derived from <see cref="UserDatabaseException"/>, which corresponds to
        /// the Sql error number.</returns>
        protected override SilverException ProcessSqlUserException(int errorNumber, string errorMessage, SqlException sqlException)
        {
            switch (errorNumber)
            {
                case 5:
                    return new AuthorizationException();
                case 6:
                    return new DatabaseException(errorNumber, DbRes.GetString(errorNumber), sqlException);

                default:
                    // return a new UserDatabaseException by default
                    return base.ProcessSqlUserException(errorNumber, DbRes.GetString(errorNumber) ?? errorMessage, sqlException);
            }
        }

        #region Server

        /// <summary>
        /// Deletes the server for the specified GUID.
        /// </summary>
        /// <param name="serverGuid"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void DeleteServer(string serverGuid)
        {
            if (string.IsNullOrEmpty(serverGuid))
            {
                throw new ArgumentNullException(nameof(serverGuid));
            }

            using (var cmd = this.CreateCommand("[dbo].[dsAgProductMappingDelete]"))
            {
                cmd.Parameters.Add("@GUID", SqlDbType.NVarChar, 36).Value = serverGuid;

                this.ExecuteNonQuery(cmd);
            }
        }

        /// <summary>
        /// Loads the data of a specific <see cref="Server"/> from the database.
        /// </summary>
        /// <param name="serverGuid">The <see cref="Server.GUID"/> of the <see cref="Server"/> to be loaded.</param>
        /// <returns>The appropriate <see cref="Server"/>, or <c>null</c> when the server has not been found in the database.</returns>
        public Server GetServer(string serverGuid)
        {
            if (serverGuid == null)
            {
                throw new ArgumentNullException(nameof(serverGuid));
            }

            using (var cmd = this.CreateCommand("[dbo].[spGetServer]"))
            {
                cmd.Parameters.Add("@GUID", SqlDbType.NVarChar, 36).Value = serverGuid;

                using (var r = cmd.ExecuteReader(CommandBehavior.SingleResult | CommandBehavior.SingleRow))
                {
                    if (!r.Read())
                    {
                        return null;
                    }

                    return new Server()
                    {
                        GUID = r.GetNString(r.GetOrdinal("GUID")),
                        PublicKey= r.GetNTrimmedString(r.GetOrdinal("PublicKey")),
                        Partner = r.GetNTrimmedString(r.GetOrdinal("Partner")),
                        ServerName = r.GetNString(r.GetOrdinal("Server")),
                        Windows = r.GetNTrimmedString(r.GetOrdinal("Windows")),
                        BackupRoot = r.GetNTrimmedString(r.GetOrdinal("BackupRoot")),
                        Flag = r.GetInt32(r.GetOrdinal("Flag")),
                        State = r.GetNTrimmedString(r.GetOrdinal("State")),
                    };
                }
            }
        }

        /// <summary>
        /// Loads the <see cref="PartnerMapping"/>s from the database.
        /// </summary>
        /// <returns>The list of the appropriate <see cref="PartnerMapping"/> instances.</returns>
        public (List<Server> Servers, string controlTipText) GetServers(ServerCatalogFilterArgs filter)
        {
            var result = new List<Server>();
            string controlTipText = string.Empty;

            using (var cmd = this.CreateCommand("[dbo].[spListServers]"))
            {
                cmd.Parameters.Add("@Filter", SqlDbType.NVarChar, 100).Value = DbNull(filter.QuickFilter, false);

                var stateTip = cmd.Parameters.Add("@States", SqlDbType.NVarChar, 255);
                stateTip.Direction = ParameterDirection.Output;

                using (var r = cmd.ExecuteReader(CommandBehavior.SingleResult))
                {
                    int guidField = r.GetOrdinal("GUID");
                    int publicKeyField = r.GetOrdinal("PublicKey");
                    int partnerField = r.GetOrdinal("Partner");
                    int serverNameField = r.GetOrdinal("Server");
                    int windowsField = r.GetOrdinal("Windows");
                    int backupRootField = r.GetOrdinal("BackupRoot");
                    int flagField = r.GetOrdinal("Flag");
                    int stateField = r.GetOrdinal("State");

                    var nameTable = new NameTable();

                    while (r.Read())
                    {
                        result.Add(new Server()
                        {
                            GUID = r.GetNTrimmedString(guidField),
                            PublicKey = r.GetNTrimmedString(publicKeyField),
                            Partner = r.GetNTrimmedString(partnerField),
                            ServerName = r.GetNString(serverNameField),
                            Windows = r.GetNTrimmedString(windowsField),
                            BackupRoot = r.GetNTrimmedString(backupRootField),
                            Flag = r.GetInt32(flagField),
                            State = r.GetNTrimmedString(stateField),
                        });
                    }
                }

                controlTipText = stateTip.Value as string ?? string.Empty;
            }

            return (result, controlTipText);
        }

        /// <summary>
        /// Loads the data of a partner in an editable <see cref="PartnerMappingE"/> instance for displaying/editing.
        /// </summary>
        /// <param name="serverGuid">The unique code of the partner.</param>
        /// <returns>The data of the partner in a new <see cref="PartnerMappingE"/> instance.</returns>
        /// <exception cref="UserDatabaseException">The partner with the specified code does not exist
        /// in the database..</exception>
        /// <exception cref="UnexpectedDataException">The data of the partner has not been received in the
        /// expected form from the database.</exception>
        public ServerE GetServerE(string serverGuid)
        {
            if (string.IsNullOrEmpty(serverGuid))
            {
                throw new ArgumentNullException(nameof(serverGuid));
            }

            ServerE server;

            using (var cmd = this.CreateCommand("[dbo].[spGetServer]"))
            {
                cmd.Parameters.Add("@GUID", SqlDbType.NVarChar, 36).Value = serverGuid;

                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read())
                    {
                        throw new UserDatabaseException(Properties.Resource.ExcServerNotFound);
                    }

                    server = new ServerE()
                    {
                        GUID = r.GetNString(r.GetOrdinal("GUID")),
                        PublicKey = r.GetNTrimmedString(r.GetOrdinal("PublicKey")),
                        Partner = r.GetNTrimmedString(r.GetOrdinal("Partner")),
                        ServerName = r.GetNString(r.GetOrdinal("Server")),
                        Windows = r.GetNTrimmedString(r.GetOrdinal("Windows")),
                        BackupRoot = r.GetNTrimmedString(r.GetOrdinal("BackupRoot")),
                        Flag = r.GetInt32(r.GetOrdinal("Flag")),
                    };
                }
            }

            return server;
        }

        /// <summary>
        /// Saves the data of the partner and retrieves its unique code, useful in case
        /// of the new registrations for which the code is being generated automatically.
        /// </summary>
        /// <param name="server">The data of the partner to be saved in an editable <see cref="ServerE"/> instance.</param>
        /// <returns>The unique code of the saved partner.</returns>
        public string SaveServer(ServerE server)
        {
            if (server == null)
            {
                throw new ArgumentNullException(nameof(server));
            }

            using (var cmd = this.CreateCommand("[dbo].[spSaveServer]"))
            {
                var serverGUIDParam = cmd.Parameters.Add("@GUID", SqlDbType.NVarChar, 36);
                serverGUIDParam.Direction = ParameterDirection.Output;

                var serverList = new List<ServerE> { server };
                cmd.Parameters.Add(ServerSqlType.CreateParameter("@server", serverList));

                this.ExecuteNonQuery(cmd);

                return (serverGUIDParam.Value != DBNull.Value) ? Convert.ToString(serverGUIDParam.Value) : string.Empty;
            }
        }

        #endregion Server

        #region Alert

        /// <summary>
        /// Loads the <see cref="Alert"/>s from the database.
        /// </summary>
        /// <returns>The list of the appropriate <see cref="Alert"/> instances.</returns>
        public List<Alert> GetAlerts()
        {
            var result = new List<Alert>();

            using (var cmd = this.CreateCommand("[dbo].[spListAlerts]"))
            using (var r = cmd.ExecuteReader(CommandBehavior.SingleResult))
            {
                this.ReadAlerts(r, result);
            }

            return result;
        }

        /// <summary>
        /// Loads the <see cref="Alert"/>s history from the database.
        /// </summary>
        /// <returns>The list of the appropriate <see cref="Alert"/> instances.</returns>
        public List<Alert> GetAlertsHistory()
        {
            var result = new List<Alert>();

            using (var cmd = this.CreateCommand("[dbo].[spListAlertsHistory]"))
            using (var r = cmd.ExecuteReader(CommandBehavior.SingleResult))
            {
                this.ReadAlerts(r, result);
            }

            return result;
        }

        private void ReadAlerts(SqlDataReader r, List<Alert> result)
        {

            if (!r.HasRows)
            {
                return;
            }

            int idField = r.GetOrdinal("Id");
            int titleField = r.GetOrdinal("Title");
            int messageField = r.GetOrdinal("Message");
            int dateField = r.GetOrdinal("Date");
            int expirationDateField = r.GetOrdinal("ExpirationDate");
            int typeField = r.GetOrdinal("Type");
            int rightField = r.GetOrdinal("AccessRight");
            int ackDateField = r.GetOrdinal("AckDate");

            while (r.Read())
            {
                result.Add(new Alert()
                {
                    Id = r.GetInt32(idField),
                    Title = r.GetTrimmedString(titleField),
                    Message = r.GetNString(messageField),
                    Date = r.GetDateTime(dateField),
                    ExpirationDate = r.GetNDateTime(expirationDateField),
                    AcknowledgedOn = r.GetNDateTime(ackDateField),
                    Type = (AlertType)r.GetString(typeField)[0],
                    AccessRights = r.GetNString(rightField)?.Split(',').TrimElements(),
                });
            }
        }

        /// <summary>
        /// Saves that the user has acknowledged an alert from the alert view.
        /// </summary>
        /// <param name="alertId">The identifier of the note being acknowledged.</param>
        public void AcknowledgeAlert(int alertId)
        {
            using (var cmd = this.CreateCommand("[dbo].[spAcknowledgeAlert]"))
            {
                cmd.Parameters.AddWithValue("@alertId", alertId);

                this.ExecuteNonQuery(cmd);
            }
        }

        #endregion Alert

        #region Report

        /// <summary>
        /// Fetches the statuses of all server components from the database.
        /// </summary>
        /// <returns>A list of server component statuses.</returns>
        public List<ServerComponentStatus> GetServerComponentStatuses()
        {
            var result = new List<ServerComponentStatus>();

            using (var cmd = this.CreateCommand("[dbo].[spGetServerComponentStatuses]"))
            using (var r = cmd.ExecuteReader())
            {

                while (r.Read())
                {
                    result.Add(new ServerComponentStatus
                    {
                        ServerGuid = r.GetNString(r.GetOrdinal("GUID")),
                        ServerName = r.GetNString(r.GetOrdinal("ServerName")),
                        ComponentName = r.GetNString(r.GetOrdinal("Component")),
                        Status = r.GetNString(r.GetOrdinal("Status"))
                    });
                }
            }

            return result;
        }

        #endregion Report
    }
}
