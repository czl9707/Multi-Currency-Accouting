namespace Accountant.Services.DB;

using Microsoft.Data.Sqlite;

public interface IDBConnectionFactory
{
    public SqliteConnection GetConnection();
}