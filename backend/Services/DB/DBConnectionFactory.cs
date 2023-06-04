namespace Accountant.Services.DB;

using Microsoft.Data.Sqlite;

public class DBConnectionFactory : IDBConnectionFactory
{
    public DBConnectionFactory(IConfiguration configuration)
    {
        connectionString = configuration.GetValue<string>("Database:ConnectionString");
    }

    public string connectionString
    {
        get;
        private set;
    }

    public SqliteConnection GetConnection()
    {
        return new SqliteConnection(connectionString);
    }
}