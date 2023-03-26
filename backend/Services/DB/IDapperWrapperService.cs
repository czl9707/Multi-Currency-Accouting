namespace Accountant.Services.DB;

using System.Data;

public interface IDapperWrapperService{
    IEnumerable<T> Query<T>(IDbConnection connection, string sql, object? param = null, CommandType? commandType = CommandType.Text, IDbTransaction? trans = null, int? commandTimeout = null) where T: class;    
    Task<IEnumerable<T>> QueryAsync<T>(IDbConnection connection, string sql, object? param = null, CommandType? commandType = CommandType.Text, IDbTransaction? trans = null, int? commandTimeout = null) where T: class;    
    int Execute(IDbConnection connection, string sql, object? param = null, CommandType? commandType = null, IDbTransaction? trans = null, int? commandTimeout = null);    
    Task<int> ExecuteAsync(IDbConnection connection, string sql, object? param = null, CommandType? commandType = null, IDbTransaction? trans = null, int? commandTimeout = null);    
    IDbTransaction BeginTransaction(IDbConnection connection);
}
