namespace Accountant.Services.DB;

using System.Data;
using Dapper;

public class DapperWrapperService : IDapperWrapperService{
    public DapperWrapperService(){
        DefaultTypeMap.MatchNamesWithUnderscores = true;
    }

    public IEnumerable<T> Query<T>(IDbConnection connection, string sql, object? param = null, CommandType? commandType = CommandType.Text, IDbTransaction? trans = null, int? commandTimeout = null) where T: class
        => connection.Query<T>(sql: sql, param: param, transaction: trans, commandTimeout: commandTimeout, commandType: commandType);
    
    public async Task<IEnumerable<T>> QueryAsync<T>(IDbConnection connection, string sql, object? param = null, CommandType? commandType = CommandType.Text, IDbTransaction? trans = null, int? commandTimeout = null) where T: class
        => await connection.QueryAsync<T>(sql: sql, param: param, transaction: trans, commandTimeout: commandTimeout, commandType: commandType);
    
    public int Execute(IDbConnection connection, string sql, object? param = null, CommandType? commandType = null, IDbTransaction? trans = null, int? commandTimeout = null)
        => connection.Execute(sql: sql, param: param, transaction: trans, commandTimeout: commandTimeout, commandType: commandType);
    
    public async Task<int> ExecuteAsync(IDbConnection connection, string sql, object? param = null, CommandType? commandType = null, IDbTransaction? trans = null, int? commandTimeout = null)
        => await connection.ExecuteAsync(sql: sql, param: param, transaction: trans, commandTimeout: commandTimeout, commandType: commandType);
    
    public IDbTransaction BeginTransaction(IDbConnection connection) 
        => connection.BeginTransaction();
}
