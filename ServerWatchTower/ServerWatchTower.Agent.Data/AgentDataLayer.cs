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
        /// Loads the data of a specific <see cref="PartnerMapping"/> from the database.
        /// </summary>
        /// <param name="serverGuid">The <see cref="PartnerMapping.Cui"/> of the <see cref="PartnerMapping"/> to be loaded.</param>
        /// <returns>The appropriate <see cref="Partner"/>, or <c>null</c> when the partner has not been found in the database.</returns>
        public Server GetServer(string serverGuid)
        {
            if (serverGuid == null)
            {
                throw new ArgumentNullException(nameof(serverGuid));
            }

            using (var cmd = this.CreateCommand("[dbo].[gsEFactRecMapareParteneriListare]"))
            {
                cmd.Parameters.Add("@FiltruRapid", SqlDbType.NVarChar, 64).Value = serverGuid;


                using (var r = cmd.ExecuteReader(CommandBehavior.SingleResult | CommandBehavior.SingleRow))
                {
                    if (!r.Read())
                    {
                        return null;
                    }

                    return new Server()
                    {
                        Cui = r.GetTrimmedString(r.GetOrdinal("CUI")),
                        Name = r.GetNString(r.GetOrdinal("NumeFirmaFact")),
                        Code = r.GetNTrimmedString(r.GetOrdinal("CodFirma")),
                        PartnerType = r.GetNTrimmedString(r.GetOrdinal("TipPartener")),
                        PartnerName = r.GetNString(r.GetOrdinal("NumeFirmaNom")),
                        CodeGest = r.GetNTrimmedString(r.GetOrdinal("CodGestDefault")),
                        CodeCen = r.GetNTrimmedString(r.GetOrdinal("CodCenDefault")),
                        Flag = r.GetInt32(r.GetOrdinal("Flag")),
                        Ts = r.GetDateTime(r.GetOrdinal("TS")),
                        User = r.GetNTrimmedString(r.GetOrdinal("Utilizator")),
                        State = r.GetNTrimmedString(r.GetOrdinal("StareMapare")),
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

            using (var cmd = this.CreateCommand("[dbo].[gsEFactRecMapareParteneriListare]"))
            {
                cmd.Parameters.Add("@FiltruRapid", SqlDbType.NVarChar, 64).Value = DbNull(filter.QuickFilter, false);

                var stateTip = cmd.Parameters.Add("@Stare_ControlTipText", SqlDbType.NVarChar, 255);
                stateTip.Direction = ParameterDirection.Output;

                using (var r = cmd.ExecuteReader(CommandBehavior.SingleResult))
                {
                    int cuiField = r.GetOrdinal("CUI");
                    int nameField = r.GetOrdinal("NumeFirmaFact");
                    int codeField = r.GetOrdinal("CodFirma");
                    int partnerTypeField = r.GetOrdinal("TipPartener");
                    int partnerNameField = r.GetOrdinal("NumeFirmaNom");
                    int codGestField = r.GetOrdinal("CodGestDefault");
                    int codCenField = r.GetOrdinal("CodCenDefault");
                    int flagField = r.GetOrdinal("Flag");
                    int tsField = r.GetOrdinal("TS");
                    int userField = r.GetOrdinal("Utilizator");
                    int state = r.GetOrdinal("StareMapare");

                    var nameTable = new NameTable();

                    while (r.Read())
                    {
                        result.Add(new Server()
                        {
                            Cui = r.GetTrimmedString(cuiField),
                            Name = r.GetNString(nameField, String.Empty),
                            Code = r.GetNTrimmedString(codeField, String.Empty),
                            PartnerType = r.GetNTrimmedString(partnerTypeField, nameTable),
                            PartnerName = r.GetNString(partnerNameField, String.Empty),
                            CodeGest = r.GetNTrimmedString(codGestField, nameTable),
                            CodeCen = r.GetNTrimmedString(codCenField, nameTable),
                            Flag = r.GetInt32(flagField),
                            Ts = r.GetDateTime(tsField),
                            User = r.GetNTrimmedString(userField, nameTable),
                            State = r.GetNTrimmedString(state, nameTable),
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

            using (var cmd = this.CreateCommand("[dbo].[dsAgPartnerMappingLoad]"))
            {
                cmd.Parameters.Add("@CUI", SqlDbType.NVarChar, 15).Value = serverGuid;

                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read())
                    {
                        throw new UserDatabaseException(Properties.Resource.ExcServerNotFound);
                    }

                    server = new ServerE()
                    {
                        Cui = r.GetTrimmedString(r.GetOrdinal("CUI")),
                        Name = r.GetNString(r.GetOrdinal("NumeFirmaFact")),
                        Code = r.GetNTrimmedString(r.GetOrdinal("CodFirma")),
                        PartnerType = r.GetNTrimmedString(r.GetOrdinal("TipPartener")),
                        PartnerName = r.GetNString(r.GetOrdinal("NumeFirma")),
                        CodeGest = r.GetNTrimmedString(r.GetOrdinal("CodGestDefault")),
                        CodeCen = r.GetNTrimmedString(r.GetOrdinal("CodCenDefault")),
                        Flag = r.GetInt32(r.GetOrdinal("Flag")),
                        Ts = r.GetDateTime(r.GetOrdinal("TS")),
                        User = r.GetNTrimmedString(r.GetOrdinal("Utilizator")),
                    };
                }
            }

            return server;
        }

        /// <summary>
        /// Saves the data of the partner and retrieves its unique code, useful in case
        /// of the new registrations for which the code is being generated automatically.
        /// </summary>
        /// <param name="server">The data of the partner to be saved in an editable <see cref="PartnerE"/> instance.</param>
        /// <returns>The unique code of the saved partner.</returns>
        public void SaveServer(ServerE server)
        {
            if (server == null)
            {
                throw new ArgumentNullException(nameof(server));
            }

            using (var cmd = this.CreateCommand("[dbo].[dsAgPartnerMappingSave]"))
            {
                cmd.Parameters.Add("@CodFirma", SqlDbType.NVarChar, 15).Value = server.Code;
                cmd.Parameters.Add("@TipPartener", SqlDbType.NVarChar, 25).Value = DbNull(server.PartnerType);
                cmd.Parameters.Add("@CodGestDefault", SqlDbType.NVarChar, 10).Value = DbNull(server.CodeGest, false);
                cmd.Parameters.Add("@CodCenDefault", SqlDbType.NVarChar, 15).Value = DbNull(server.CodeCen, false);
                cmd.Parameters.Add("@Flag", SqlDbType.Int).Value = server.Flag;

                this.ExecuteNonQuery(cmd);
            }
        }
    }
}
