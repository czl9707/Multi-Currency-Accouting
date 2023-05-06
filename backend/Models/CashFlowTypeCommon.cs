namespace Accountant.Models;

using System.Data;
using Accountant.Services.DB;

public abstract class CashFlowTypeCommon<T>
where T: CashFlow
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

        this.InitTypeAsync().GetAwaiter().GetResult();
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

    private IEnumerable<CashFlowType<T>> Types {get; set;}

    public async Task<List<CashFlowType<T>>> GetAllTypesAsync() => 
        this.Types.ToList();

    public async Task<Dictionary<long, CashFlowType<T>>> GetAllTypesAsDictAsync() => 
        this.Types.ToDictionary(type => type.TypeId, type => type);

    public async Task UpdateTypeAsync(CashFlowType<T> type)
    {
        using IDbConnection connection =  this.DBConnectionFactory.GetConnection();
        await this.DapperWrapperService.QueryAsync<CashFlowType<T>>(
            connection: connection,
            sql: this.updateSql,
            param: new {
                vtype_id = type.TypeId,
                vtype_name = type.TypeName 
            }
        );

        await this.InitTypeAsync();
    }

    public async Task AddNewTypeAsync(CashFlowType<T> type)
    {
        using IDbConnection connection =  this.DBConnectionFactory.GetConnection();
        await this.DapperWrapperService.QueryAsync<CashFlowType<T>>(
            connection: connection,
            sql: this.insertSql,
            param: new {
                vtype_name = type.TypeName
            }
        );

        await this.InitTypeAsync();
    }

    public async Task DeleteTypeAsync(long typeId)
    {
        using IDbConnection connection =  this.DBConnectionFactory.GetConnection();
        await this.DapperWrapperService.QueryAsync<CashFlowType<T>>(
            connection: connection,
            sql: this.deleteSql,
            param: new {
                vtype_id = typeId
            }
        );

        await this.InitTypeAsync();
    }

    private async Task InitTypeAsync()
    {
        using IDbConnection connection =  this.DBConnectionFactory.GetConnection();
        this.Types = await this.DapperWrapperService.QueryAsync<CashFlowType<T>>(connection, this.selectSql);
    }
}

public class IncomeTypeCommon : CashFlowTypeCommon<Income>
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

public class ExpenseTypeCommon : CashFlowTypeCommon<Expense>
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