namespace Accountant.Models;

using Accountant.Services.DB;
using System.Data;

public abstract class CashFlowRecordCommon<T>
where T : CashFlow
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
                (@vhappen_utc, DATETIME('now'), @vamount, @vcurr_iso, @vnote, @vtype_id, @vmethod_id )
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
                last_modified_utc = DATETIME('now'),
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

    public async Task<CashFlowRecord<T>?> GetRecordByIDAsync (long cashflowID){
        using IDbConnection connection =  this.DBConnectionFactory.GetConnection();
        var records = await this.DapperWrapperService.QueryAsync<CashFlowRecord<T>>(
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
    public async Task<List<CashFlowRecord<T>>> GetRecordsForTimeSpanAsync (DateTime startTime, DateTime endTime){
        using IDbConnection connection =  this.DBConnectionFactory.GetConnection();
        var records = await this.DapperWrapperService.QueryAsync<CashFlowRecord<T>>(
            connection: connection,
            sql: this.selectSqlByTime,
            param: new {
                vstart_utc = startTime,
                vend_utc = endTime
            }
        );

        return records.ToList();
    }

    public async Task UpdateRecordAsync (CashFlowRecord<T> record)
    {
        using IDbConnection connection =  this.DBConnectionFactory.GetConnection();
        await this.DapperWrapperService.ExecuteAsync(
            connection: connection,
            sql: this.updateByID,
            param: new {
                vcashflow_id = record.CashFlowId,
                vhappen_utc = (DateTime?) (record.HappenUtc != DateTime.MinValue ? record.HappenUtc : null),
                vamount = (float?) (record.Amount > 0 ? record.Amount : null),
                vcurr_iso = (string?) (!String.IsNullOrEmpty(record.CurrIso) ? record.CurrIso : null),
                vnote = (string?) (!String.IsNullOrEmpty(record.Note) ? record.Note : null), 
                vtype_id = (long?) (record.TypeId >= 0 ? record.TypeId : null), 
                vmethod_id = (long?) (record.MethodId >= 0 ? record.MethodId : null)
            }
        );
    }

    public async Task AddNewRecordAsync (CashFlowRecord<T> record)
    {
        using IDbConnection connection =  this.DBConnectionFactory.GetConnection();
        await this.DapperWrapperService.ExecuteAsync(
            connection: connection,
            sql: this.insertSql,
            param: new {
                vhappen_utc = record.HappenUtc,
                vamount = record.Amount,
                vcurr_iso = record.CurrIso,
                vnote = record.Note, 
                vtype_id = record.TypeId, 
                vmethod_id = record.MethodId
            }
        );
    }

    public async Task RemoveRecordAsync (long cashflowID)
    {
        using IDbConnection connection = this.DBConnectionFactory.GetConnection();
        await this.DapperWrapperService.ExecuteAsync(
            connection: connection,
            sql: this.removeByID,
            param: new {
                vcashflow_id = cashflowID
            }
        );
    }
}

public class IncomeRecordCommon: CashFlowRecordCommon<Income>
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

public class ExpenseRecordCommon: CashFlowRecordCommon<Expense>
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