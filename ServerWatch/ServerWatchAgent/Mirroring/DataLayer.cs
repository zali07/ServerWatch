using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ServerWatchAgent.Mirroring
{
    public class DataLayer : IDisposable
    {
        public string database;

        private SqlConnection defaultConnection = null;
        private string defaultConnectionString = null;

        public DataLayer()
        {
            defaultConnectionString = ConfigurationManager.ConnectionStrings["default"]?.ConnectionString;
        }

        private SqlConnection GetDefaultConnection()
        {
            if (string.IsNullOrWhiteSpace(defaultConnectionString))
            {
                return null;
            }

            if (defaultConnection == null || defaultConnection.State == ConnectionState.Closed
                || defaultConnection.State == ConnectionState.Broken)
            {
                if (defaultConnection != null)
                {
                    defaultConnection.Close();
                    defaultConnection.Dispose();
                }

                defaultConnection = new SqlConnection(defaultConnectionString);
                defaultConnection.Open();
            }

            return defaultConnection;
        }

        public string GetServer()
        {
            return defaultConnection.DataSource;
        }

        /// <summary>
        /// Gets the databases that are being mirrored on the server.
        /// </summary>
        public DataSet GetMirroredDatabases()
        {
            try
            {
                // this query is used to get the databases that are being mirrored on the server
                string getDatabases =
                    "SELECT d.name " +
                    "FROM sys.databases d " +
                        "JOIN sys.database_mirroring dm ON d.database_id = dm.database_id " +
                    "WHERE dm.mirroring_state IS NOT NULL;";

                DataSet ds = new DataSet();

                SqlConnection connection = GetDefaultConnection();
                if (connection == null)
                {
                    return null;
                }

                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = getDatabases;

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ds);
                    }
                }

                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting mirrored databases on server.\r\n{ex.Message}");
            }
        }

        public DataSet ExecuteMonitorResultsOnServer()
        {
            try
            {
                DataSet ds = new DataSet();

                SqlConnection connection = GetDefaultConnection();
                if (connection == null)
                {
                    return null;
                }

                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "MSDB.SYS.SP_DBMMONITORRESULTS";

                    // runs the SP_DBMMONITORUPDATE before computing results so we don't have to call that sp separately
                    cmd.Parameters.Add(new SqlParameter("@update_table", 1));
                    cmd.Parameters.Add(new SqlParameter("@database_name", database));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ds);
                    }
                }

                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error executing monitor results on server on {database} database: {ex.Message}", ex);
            }
        }

        public void Dispose()
        {
            if (defaultConnection != null)
            {
                defaultConnection.Close();
                defaultConnection.Dispose();
                defaultConnection = null;
            }
        }
    }
}
