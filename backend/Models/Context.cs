namespace Accountant.Models;

using Accountant.Services.DB;

public interface IContext
{
    public Task<Dictionary<string, Currency>> GetAllCurrenciesAsync();
    public Task<Dictionary<long, IncomeType>> GetAllIncomeTypesAsync();
    public Task<Dictionary<long, ExpenseType>> GetAllExpenseTypesAsync();
    public Task UpdateIncomeTypeAsync(long typeID, string typeName);
    public Task UpdateExpenseTypeAsync(long typeID, string typeName);
    public Task AddNewIncomeTypeAsync(string typeName);
    public Task AddNewExpenseTypeAsync(string typeName);
    public Task DeleteIncomeTypeAsync(long typeID);
    public Task DeleteExpenseTypeAsync(long typeID);
    public Task<CurrencyExchangeRate?> GetExchangeRateAsync(string base_cur, string target_cur, DateTime exchange_utc);
    public Task<Dictionary<long, PaymentMethod>> GetAllMethodsAsync();
    public Task UpdateMethodTypeAsync(long methodID, string methodName);
    public Task AddNewMethodTypeAsync(string methodName);
    public Task DeleteMethodTypeAsync(long methodID);
    public Task<ExpenseRecord?> GetExpenseRecordByIDAsync(long cashflowID);
    public Task<IncomeRecord?> GetIncomeRecordByIDAsync(long cashflowID);
    public Task<List<ExpenseRecord>> GetExpenseRecordsByTimeSpanAsync(DateTime startTime, DateTime endTime);
    public Task<List<IncomeRecord>> GetIncomeRecordsByTimeSpanAsync(DateTime startTime, DateTime endTime);
    public Task UpdateExpenseRecordByIDAsync(
        long recordID, DateTime? happenUtc = null, float? amount = null, string? currIso = null, string? note = null, long? typeID = null, long? methodID = null
    );
    public Task UpdateIncomeRecordByIDAsync(
        long recordID, DateTime? happenUtc = null, float? amount = null, string? currIso = null, string? note = null, long? typeID = null, long? methodID = null
    );
    public Task AddNewExpenseRecordAsync(
        DateTime happenUtc, float amount, string currIso, long typeID, long methodID, string note = ""
    );
    public Task AddNewIncomeRecordAsync(
        DateTime happenUtc, float amount, string currIso, long typeID, long methodID, string note = ""
    );
    public Task RemoveExpenseRecordByIDAsync(long cashflowID);
    public Task RemoveIncomeRecordByIDAsync(long cashflowID);

}   

public class Context: IContext
{
    public Context (
        IDBConnectionFactory dBConnectionFactory,
        IDapperWrapperService dapperWrapperService
    ){
        if (IsInitialized) return;
        IsInitialized = true;
        
        _CurrencyCommon = new CurrencyCommon(dBConnectionFactory, dapperWrapperService);
        _IncomeTypeCommon = new IncomeTypeCommon(dBConnectionFactory, dapperWrapperService);
        _ExpenseTypeCommon = new ExpenseTypeCommon(dBConnectionFactory, dapperWrapperService);
        _ExchangeRateCommon = new ExchangeRateCommon(dBConnectionFactory, dapperWrapperService);
        _PaymentMethodCommon = new PaymentMethodCommon(dBConnectionFactory, dapperWrapperService);
        _IncomeRecordCommon = new IncomeRecordCommon(dBConnectionFactory, dapperWrapperService);
        _ExpenseRecordCommon = new ExpenseRecordCommon(dBConnectionFactory, dapperWrapperService);
    }

    private static bool IsInitialized = false;
    private static CurrencyCommon? _CurrencyCommon {get; set;}
    private static IncomeTypeCommon? _IncomeTypeCommon {get; set;}
    private static ExpenseTypeCommon? _ExpenseTypeCommon {get; set;}
    private static ExchangeRateCommon? _ExchangeRateCommon {get; set;}
    private static PaymentMethodCommon? _PaymentMethodCommon {get; set;}
    private static IncomeRecordCommon? _IncomeRecordCommon {get; set;}
    private static ExpenseRecordCommon? _ExpenseRecordCommon {get; set;}

    public async Task<Dictionary<string, Currency>> GetAllCurrenciesAsync() 
        => _CurrencyCommon != null ? 
            await _CurrencyCommon.GetAllCurrenciesAsync().ConfigureAwait(false) : 
            throw new NullReferenceException("Context Not Initialized!");

    public async Task<Dictionary<long, IncomeType>> GetAllIncomeTypesAsync()
        => _IncomeTypeCommon != null ? 
            await _IncomeTypeCommon.GetAllTypesAsync().ConfigureAwait(false) : 
            throw new NullReferenceException("Context Not Initialized!");

    public async Task<Dictionary<long, ExpenseType>> GetAllExpenseTypesAsync()
        => _ExpenseTypeCommon != null ? 
            await _ExpenseTypeCommon.GetAllTypesAsync().ConfigureAwait(false) : 
            throw new NullReferenceException("Context Not Initialized!");

    public async Task UpdateIncomeTypeAsync(long typeID, string typeName)
    {
        if (_IncomeTypeCommon == null) 
            throw new NullReferenceException("Context Not Initialized!");
        await _IncomeTypeCommon.UpdateTypeAsync(typeID, typeName).ConfigureAwait(false);
    }

    public async Task UpdateExpenseTypeAsync(long typeID, string typeName)
    {
        if (_ExpenseTypeCommon == null) 
            throw new NullReferenceException("Context Not Initialized!");
        await _ExpenseTypeCommon.UpdateTypeAsync(typeID, typeName).ConfigureAwait(false);
    }

    public async Task AddNewIncomeTypeAsync(string typeName){
        if (_IncomeTypeCommon == null) 
            throw new NullReferenceException("Context Not Initialized!");
        await _IncomeTypeCommon.AddNewTypeAsync(typeName).ConfigureAwait(false);
    }

    public async Task AddNewExpenseTypeAsync(string typeName){
        if (_ExpenseTypeCommon == null) 
            throw new NullReferenceException("Context Not Initialized!");
        await _ExpenseTypeCommon.AddNewTypeAsync(typeName).ConfigureAwait(false);
    }

    public async Task DeleteIncomeTypeAsync(long typeID)
    {
        if (_IncomeTypeCommon == null)
            throw new NullReferenceException("Context Not Initialized!");
        await _IncomeTypeCommon.DeleteTypeAsync(typeID).ConfigureAwait(false);
    }
    public async Task DeleteExpenseTypeAsync(long typeID)
    {
        if (_ExpenseTypeCommon == null)
            throw new NullReferenceException("Context Not Initialized!");
        await _ExpenseTypeCommon.DeleteTypeAsync(typeID).ConfigureAwait(false);
    }

    public async Task<CurrencyExchangeRate?> GetExchangeRateAsync(string base_cur, string target_cur, DateTime exchange_utc)
        => _ExchangeRateCommon != null ? 
            await _ExchangeRateCommon.GetExchangeRateAsync(base_cur, target_cur, exchange_utc).ConfigureAwait(false) :
            throw new NullReferenceException("Context Not Initialized!");

    public async Task<Dictionary<long, PaymentMethod>> GetAllMethodsAsync()
        => _PaymentMethodCommon != null ?
            await _PaymentMethodCommon.GetAllMethodsAsync().ConfigureAwait(false) :
            throw new NullReferenceException("Context Not Initialized!");

    public async Task UpdateMethodTypeAsync(long methodID, string methodName)
    {
        if (_PaymentMethodCommon == null)
            throw new NullReferenceException("Context Not Initialized!");
        await _PaymentMethodCommon.UpdateMethodAsync(methodID, methodName).ConfigureAwait(false);
    }
    public async Task AddNewMethodTypeAsync(string methodName)
    {
        if (_PaymentMethodCommon == null)
            throw new NullReferenceException("Context Not Initialized!");
        await _PaymentMethodCommon.AddNewMethodAsync(methodName).ConfigureAwait(false);
    }
    public async Task DeleteMethodTypeAsync(long methodID)
    {
        if (_PaymentMethodCommon == null)
            throw new NullReferenceException("Context Not Initialized!");
        await _PaymentMethodCommon.DeleteMethodAsync(methodID).ConfigureAwait(false);
    }

    public async Task<ExpenseRecord?> GetExpenseRecordByIDAsync(long cashflowID)
    {
        if (_ExpenseRecordCommon == null)
            throw new NullReferenceException("Context Not Initialized!");

        var record = await _ExpenseRecordCommon.GetRecordByIDAsync(cashflowID).ConfigureAwait(false);
        await EnrichExpenseRecordAsync(record).ConfigureAwait(false);
        return record;
    }

    public async Task<IncomeRecord?> GetIncomeRecordByIDAsync(long cashflowID)
    {
        if (_IncomeRecordCommon == null)
            throw new NullReferenceException("Context Not Initialized!");

        var record = await _IncomeRecordCommon.GetRecordByIDAsync(cashflowID).ConfigureAwait(false);
        await EnrichIncomeRecordAsync(record).ConfigureAwait(false);
        return record;
    }
    
    public async Task<List<ExpenseRecord>> GetExpenseRecordsByTimeSpanAsync(DateTime startTime, DateTime endTime)
    {
        if (_ExpenseRecordCommon == null)
            throw new NullReferenceException("Context Not Initialized!");

        var records = await _ExpenseRecordCommon.GetRecordsForTimeSpanAsync(startTime, endTime).ConfigureAwait(false);
        foreach (var record in records) await EnrichExpenseRecordAsync(record).ConfigureAwait(false);
        return records;
    }

    public async Task<List<IncomeRecord>> GetIncomeRecordsByTimeSpanAsync(DateTime startTime, DateTime endTime)
    {
        if (_IncomeRecordCommon == null)
            throw new NullReferenceException("Context Not Initialized!");
        
        var records = await _IncomeRecordCommon.GetRecordsForTimeSpanAsync(startTime, endTime).ConfigureAwait(false);
        foreach (var record in records) await EnrichIncomeRecordAsync(record).ConfigureAwait(false);
        return records;
    }

    public async Task UpdateExpenseRecordByIDAsync(
        long recordID, DateTime? happenUtc = null, float? amount = null, string? currIso = null, string? note = null, long? typeID = null, long? methodID = null
    )
    {
        if (_ExpenseRecordCommon == null)
            throw new NullReferenceException("Context Not Initialized!");
        await _ExpenseRecordCommon.UpdateRecordByIDAsync(
            recordID, happenUtc, amount, currIso, note, typeID, methodID
        ).ConfigureAwait(false);
    }

    public async Task UpdateIncomeRecordByIDAsync(
        long recordID, DateTime? happenUtc = null, float? amount = null, string? currIso = null, string? note = null, long? typeID = null, long? methodID = null
    )
    {
        if (_IncomeRecordCommon == null)
            throw new NullReferenceException("Context Not Initialized!");
        await _IncomeRecordCommon.UpdateRecordByIDAsync(
            recordID, happenUtc, amount, currIso, note, typeID, methodID
        ).ConfigureAwait(false);
    }

    public async Task AddNewExpenseRecordAsync(
        DateTime happenUtc, float amount, string currIso, long typeID, long methodID, string note = ""
    )
    {
        if (_ExpenseRecordCommon == null)
            throw new NullReferenceException("Context Not Initialized!");
        await _ExpenseRecordCommon.AddNewRecordAsync(
            happenUtc, amount, currIso, typeID, methodID, note
        ).ConfigureAwait(false);
    }

    public async Task AddNewIncomeRecordAsync(
        DateTime happenUtc, float amount, string currIso, long typeID, long methodID, string note = ""
    )
    {
        if (_IncomeRecordCommon == null)
            throw new NullReferenceException("Context Not Initialized!");
        await _IncomeRecordCommon.AddNewRecordAsync(
            happenUtc, amount, currIso, typeID, methodID, note
        ).ConfigureAwait(false);
    }


    public async Task RemoveExpenseRecordByIDAsync(long cashflowID)
    {
        if (_ExpenseRecordCommon == null)
            throw new NullReferenceException("Context Not Initialized!");
        await _ExpenseRecordCommon.RemoveRecordByIDAsync(cashflowID).ConfigureAwait(false);
    }

    public async Task RemoveIncomeRecordByIDAsync(long cashflowID)
    {
        if (_IncomeRecordCommon == null)
            throw new NullReferenceException("Context Not Initialized!");
        await _IncomeRecordCommon.RemoveRecordByIDAsync(cashflowID).ConfigureAwait(false);
    }

    private async Task<ExpenseRecord?> EnrichExpenseRecordAsync(ExpenseRecord? record)
    {
        if (record == null) return null;

        var currencies = await this.GetAllCurrenciesAsync().ConfigureAwait(false);
        var methods = await this.GetAllMethodsAsync().ConfigureAwait(false);
        var types = await this.GetAllExpenseTypesAsync().ConfigureAwait(false);
        record.Curr = currencies[record.CurrIso];
        record.Method = methods[record.MethodId];
        record.Type = types[record.TypeId];

        return record;
    }
    
    private async Task<IncomeRecord?> EnrichIncomeRecordAsync(IncomeRecord? record)
    {
        if (record == null) return null;

        var currencies = await this.GetAllCurrenciesAsync().ConfigureAwait(false);
        var methods = await this.GetAllMethodsAsync().ConfigureAwait(false);
        var types = await this.GetAllIncomeTypesAsync().ConfigureAwait(false);
        record.Curr = currencies[record.CurrIso];
        record.Method = methods[record.MethodId];
        record.Type = types[record.TypeId];

        return record;
    }
}