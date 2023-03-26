namespace Accountant.Models;

using Accountant.Services.DB;
using System.Data;

public abstract class CashFlowRecordCommon<T>
where T : CashFlowRecord
{
    private IDBConnectionFactory DBConnectionFactory;
    private IDapperWrapperService DapperWrapperService;

    public CashFlowRecordCommon(
        IDBConnectionFactory dBConnectionFactory,
        IDapperWrapperService dapperWrapperService
    )
    {
        this.DBConnectionFactory = dBConnectionFactory;
        this.DapperWrapperService = dapperWrapperService;
    }

    protected abstract string tableName {get;}
    protected string selectSqlById {
        get => @$"
            SELECT cashflow_id, happen_utc, last_modified_utc, amount, curr_iso, note, type_id, method_id
            FROM {tableName}
            WHERE cashflow_id = @vcashflow_id
        ";
    }
    protected string insertSql {
        get => @$"
            INSERT INTO {tableName}
                (happen_utc, last_modified_utc, amount, curr_iso, note, type_id, method_id)
            VALUES
                (@vhappen_utc, 'now', @vamount, @vcurr_iso, @vnote, @vtype_id, @vmethod_id )
        ";
    }
    protected string selectSqlByTime {
        get => @$"
            SELECT cashflow_id, happen_utc, last_modified_utc, amount, curr_iso, note, type_id, method_id
            FROM {tableName}
            WHERE happen_utc <= @vend_utc AND happen_utc >= @vstart_utc
        ";
    }

    protected string updateByID {
        get => @$"
            UPDATE {tableName}
            SET happen_utc = CASE WHEN @vhappen_utc is null THEN happen_utc ELSE @vhappen_utc END,
                last_modified_utc = 'now',
                amount = CASE WHEN @vamount is null THEN amount ELSE @vamount END,
                curr_iso = CASE WHEN @vcurr_iso is null THEN curr_iso ELSE @vcurr_iso END,
                note = CASE WHEN @vnote is null THEN note ELSE @vnote END,
                type_id = CASE WHEN @vtype_id is null THEN type_id ELSE @vtype_id END,
                method_id = CASE WHEN @vmethod_id is null THEN method_id ELSE @vmethod_id END
            WHERE cashflow_id = @vcashflow_id
        ";
    }

    protected string removeByID {
        get => @$"
            DELETE FROM {tableName}
            WHERE cashflow_id = @vcashflow_id
        ";
    }

    public async Task<T?> GetRecordByIDAsync (long cashflowID){
        using IDbConnection connection =  this.DBConnectionFactory.GetConnection();
        var records = await this.DapperWrapperService.QueryAsync<T>(
            connection: connection,
            sql: this.selectSqlById,
            param: new {vcashflow_id = cashflowID}
        );

        if (!records.Any())
        {
            // log something
            return null;
        }
        return records.First();
    }

    // startTime and endTime are inclusive. 
    public async Task<List<T>> GetRecordsForTimeSpanAsync (DateTime startTime, DateTime endTime){
        using IDbConnection connection =  this.DBConnectionFactory.GetConnection();
        var records = await this.DapperWrapperService.QueryAsync<T>(
            connection: connection,
            sql: this.selectSqlByTime,
            param: new {
                vstart_utc = startTime,
                vend_utc = endTime
            }
        );

        return records.ToList();
    }

    public async Task UpdateRecordByIDAsync (
        long recordID,
        DateTime? happenUtc = null,
        float? amount = null, 
        string? currIso = null, 
        string? note = null, 
        long? typeID = null, 
        long? methodID = null
    )
    {
        using IDbConnection connection =  this.DBConnectionFactory.GetConnection();
        await this.DapperWrapperService.ExecuteAsync(
            connection: connection,
            sql: this.updateByID,
            param: new {
                vcashflow_id = recordID,
                vhappen_utc = happenUtc,
                vamount = amount,
                vcurr_iso = currIso,
                vnote = note, 
                vtype_id = typeID, 
                vmethod_id = methodID
            }
        );
    }

    public async Task AddNewRecordAsync (
        DateTime happenUtc,
        float amount, 
        string currIso, 
        long typeID, 
        long methodID,
        string? note = "" 
    )
    {
        using IDbConnection connection =  this.DBConnectionFactory.GetConnection();
        await this.DapperWrapperService.ExecuteAsync(
            connection: connection,
            sql: this.insertSql,
            param: new {
                vhappen_utc = happenUtc,
                vamount = amount,
                vcurr_iso = currIso,
                vnote = note, 
                vtype_id = typeID, 
                vmethod_id = methodID
            }
        );
    }

    public async Task RemoveRecordByIDAsync (long recordID)
    {
        using IDbConnection connection = this.DBConnectionFactory.GetConnection();
        await this.DapperWrapperService.ExecuteAsync(
            connection: connection,
            sql: this.removeByID,
            param: new {
                vcashflow_id = recordID
            }
        );
    }
}

public class IncomeRecordCommon: CashFlowRecordCommon<IncomeRecord>
{
    public IncomeRecordCommon(
        IDBConnectionFactory dBConnectionFactory,
        IDapperWrapperService dapperWrapperService
    ): base(dBConnectionFactory, dapperWrapperService){}

    protected override string tableName 
    {
        get => "tbl_income";
    }
}

public class ExpenseRecordCommon: CashFlowRecordCommon<ExpenseRecord>
{
    public ExpenseRecordCommon(
        IDBConnectionFactory dBConnectionFactory,
        IDapperWrapperService dapperWrapperService
    ): base(dBConnectionFactory, dapperWrapperService){}
    
    protected override string tableName 
    {
        get => "tbl_expense";
    }
}