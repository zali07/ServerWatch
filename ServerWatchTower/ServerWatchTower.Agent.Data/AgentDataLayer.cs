namespace ServerWatchTower.Agent.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading;
    using System.Xml;
    using ServerWatchTower.Agent.Model;
    using Cosys.SilverLib.Data;
    using Cosys.SilverLib.Model;
    using Microsoft.SqlServer.Server;

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
    }
}
