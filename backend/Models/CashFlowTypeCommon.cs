namespace Accountant.Models;

using System.Data;
using Accountant.Services.DB;

public abstract class CashFlowTypeCommon<T>
where T: CashFlowType
{
    private IDBConnectionFactory DBConnectionFactory;
    private IDapperWrapperService DapperWrapperService;

    public CashFlowTypeCommon(
        IDBConnectionFactory dBConnectionFactory,
        IDapperWrapperService dapperWrapperService
    )
    {
        this.DBConnectionFactory = dBConnectionFactory;
        this.DapperWrapperService = dapperWrapperService;
    }

    protected abstract string tableName {get;}
    protected string selectSql {
        get => $"SELECT type_id, type_name AS typeName FROM {tableName}";
    }

    protected string insertSql {
        get => @$"
            INSERT INTO {tableName} (type_name)
            VALUES (@vtype_name)
        ";
    }

    protected string updateSql {
        get => @$"
            UPDATE {tableName}
            SET type_name = @vtype_name
            WHERE type_id = @vtype_id
        ";
    }

    protected string deleteSql {
        get => @$"
            DELETE FROM {tableName}
            WHERE type_id = @vtype_id
        ";
    }

    private Dictionary<long, T>? Types {get; set;}

    public async Task<Dictionary<long, T>> GetAllTypesAsync(){
        if (this.Types is null){
            using IDbConnection connection =  this.DBConnectionFactory.GetConnection();
            var types = await this.DapperWrapperService.QueryAsync<T>(connection, this.selectSql);

            this.Types = types.ToDictionary(
                type => type.TypeId, type => type
            );
        }
            
        return this.Types;
    }

    public async Task UpdateTypeAsync(long typeID, string typeName)
    {
        using IDbConnection connection =  this.DBConnectionFactory.GetConnection();
        await this.DapperWrapperService.QueryAsync<T>(
            connection: connection,
            sql: this.updateSql,
            param: new {
                vtype_id = typeID,
                vtype_name = typeName 
            }
        );

        var types = await this.DapperWrapperService.QueryAsync<T>(connection, this.selectSql);
        this.Types = types.ToDictionary(
            type => type.TypeId, type => type
        );
    }

    public async Task AddNewTypeAsync(string typeName)
    {
        using IDbConnection connection =  this.DBConnectionFactory.GetConnection();
        await this.DapperWrapperService.QueryAsync<T>(
            connection: connection,
            sql: this.insertSql,
            param: new {
                vtype_name = typeName 
            }
        );

        var types = await this.DapperWrapperService.QueryAsync<T>(connection, this.selectSql);
        this.Types = types.ToDictionary(
            type => type.TypeId, type => type
        );
    }

    public async Task DeleteTypeAsync(long typeID)
    {
        using IDbConnection connection =  this.DBConnectionFactory.GetConnection();
        await this.DapperWrapperService.QueryAsync<T>(
            connection: connection,
            sql: this.deleteSql,
            param: new {
                vtype_id = typeID
            }
        );

        var types = await this.DapperWrapperService.QueryAsync<T>(connection, this.selectSql);
        this.Types = types.ToDictionary(
            type => type.TypeId, type => type
        );
    }
}

public class IncomeTypeCommon : CashFlowTypeCommon<IncomeType>
{
    public IncomeTypeCommon(
        IDBConnectionFactory dBConnectionFactory,
        IDapperWrapperService dapperWrapperService
        ): base(dBConnectionFactory, dapperWrapperService){}

    protected override string tableName 
    { 
        get => "tbl_income_type";
    }
}

public class ExpenseTypeCommon : CashFlowTypeCommon<ExpenseType>
{
    public ExpenseTypeCommon(
        IDBConnectionFactory dBConnectionFactory,
        IDapperWrapperService dapperWrapperService
        ): base(dBConnectionFactory, dapperWrapperService){}

    protected override string tableName 
    { 
        get => "tbl_expense_type";
    }
}