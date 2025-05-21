namespace ServerWatchWS.Data
{
    using Microsoft.Data.SqlClient;
    using ServerWatchWS.Model;
    using System;
    using System.Collections.Generic;
    using System.Data;

    /// <summary>
    /// The specialized <see cref="DataLayer"/> for the Agent module of ServerWatchTower.
    /// </summary>
    public class DataLayer
    {
        private readonly string _connectionString;

        public DataLayer(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task InsertDriverEntriesAsync(List<DriverData> entries)
        {
            var table = TableTypeBuilder.CreateDriverEntryTable(entries);

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("spInsertDriverEntries", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            var parameter = command.Parameters.AddWithValue("@DriverEntries", table);
            parameter.SqlDbType = SqlDbType.Structured;
            parameter.TypeName = "dbo.DriverEntryTableType";

            await command.ExecuteNonQueryAsync();
        }

        public async Task InsertMirroringEntriesAsync(List<MirroringData> entries)
        {
            var table = TableTypeBuilder.CreateMirroringEntryTable(entries);

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("spInsertMirroringEntries", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            var parameter = command.Parameters.AddWithValue("@MirroringEntries", table);
            parameter.SqlDbType = SqlDbType.Structured;
            parameter.TypeName = "dbo.MirroringEntryTableType";

            await command.ExecuteNonQueryAsync();
        }

        public async Task<Servers?> GetServerByGUID(string guid)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand(
                "SELECT Id, GUID, PublicKey, Partner, Server, Windows, IsApproved " +
                "FROM Servers WHERE GUID = @GUID", connection);

            command.Parameters.AddWithValue("@GUID", guid);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Servers
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    GUID = reader.GetString(reader.GetOrdinal("GUID")),
                    PublicKey = reader.GetString(reader.GetOrdinal("PublicKey")),
                    Partner = reader.IsDBNull(reader.GetOrdinal("Partner")) ? null : reader.GetString(reader.GetOrdinal("Partner")),
                    Server = reader.IsDBNull(reader.GetOrdinal("Server")) ? null : reader.GetString(reader.GetOrdinal("Server")),
                    Windows = reader.IsDBNull(reader.GetOrdinal("Windows")) ? null : reader.GetString(reader.GetOrdinal("Windows")),
                    IsApproved = reader.GetBoolean(reader.GetOrdinal("IsApproved"))
                };
            }

            return null;
        }
    }
}
