namespace Accountant.Models;

using Accountant.Services.DB;
using System.Collections.Generic;

public interface IContext
{
    public Task<List<Currency>> GetAllCurrenciesAsync();
    public Task<List<CashFlowType<T>>> GetAllCashFlowTypesAsync<T>() where T : CashFlow;
    public Task UpdateCashFlowTypeAsync<T>(CashFlowType<T> type) where T : CashFlow;
    public Task AddNewCashFlowTypeAsync<T>(CashFlowType<T> type) where T : CashFlow;
    public Task DeleteCashFlowTypeAsync<T>(long typeID) where T : CashFlow;
    public Task<CurrencyExchangeRate?> GetExchangeRateAsync(CurrencyExchangeRate rateSetup);
    public Task<List<PaymentMethod>> GetAllMethodsAsync();
    public Task UpdateMethodTypeAsync(PaymentMethod method);
    public Task AddNewMethodTypeAsync(PaymentMethod method);
    public Task DeleteMethodTypeAsync(long methodID);
    public Task<CashFlowRecord<T>?> GetCashFlowRecordByIDAsync<T>(long cashflowID) where T : CashFlow;
    public Task<List<CashFlowRecord<T>>> GetCashFlowRecordsByTimeSpanAsync<T>(DateTime startTime, DateTime endTime) where T : CashFlow;
    public Task UpdateCashFlowRecordAsync<T>(CashFlowRecord<T> record) where T : CashFlow;
    public Task AddNewCashFlowRecordAsync<T>(CashFlowRecord<T> record) where T : CashFlow;
    public Task DeleteCashFlowRecordAsync<T>(long cashflowID) where T : CashFlow;
}   

public class Context: IContext
{
    public Context (
        IDBConnectionFactory dBConnectionFactory,
        IDapperWrapperService dapperWrapperService
    ){        
        _CurrencyCommon = new CurrencyCommon(dBConnectionFactory, dapperWrapperService);
        _ExchangeRateCommon = new ExchangeRateCommon(dBConnectionFactory, dapperWrapperService);
        _PaymentMethodCommon = new PaymentMethodCommon(dBConnectionFactory, dapperWrapperService);
        
        _CashFlowTypeCommons = new Dictionary<Type, object>
        {
            {typeof(Income), new IncomeTypeCommon(dBConnectionFactory, dapperWrapperService)},
            {typeof(Expense), new ExpenseTypeCommon(dBConnectionFactory, dapperWrapperService)}
        };
        _CashFlowRecordCommons = new Dictionary<Type, object>
        {
            {typeof(Income), new IncomeRecordCommon(dBConnectionFactory, dapperWrapperService)},
            {typeof(Expense), new ExpenseRecordCommon(dBConnectionFactory, dapperWrapperService)}
        };
    }

    private CurrencyCommon _CurrencyCommon {get; set;}
    private Dictionary<Type, object> _CashFlowTypeCommons {get; set;}
    private Dictionary<Type, object> _CashFlowRecordCommons {get; set;}
    private ExchangeRateCommon _ExchangeRateCommon {get; set;}
    private PaymentMethodCommon _PaymentMethodCommon {get; set;}

    public async Task<List<Currency>> GetAllCurrenciesAsync() 
        => _CurrencyCommon != null ? 
            await _CurrencyCommon.GetAllCurrenciesAsync().ConfigureAwait(false) : 
            throw new NullReferenceException("Context Not Initialized!");

    public async Task<List<CashFlowType<T>>> GetAllCashFlowTypesAsync<T>()
    where T : CashFlow
    {
        var cashFlowTypeCommon = GetCashFlowTypeCommon<T>();
        return await cashFlowTypeCommon.GetAllTypesAsync().ConfigureAwait(false);
    }

    public async Task UpdateCashFlowTypeAsync<T>(CashFlowType<T> type)
    where T : CashFlow
    {
        var cashFlowTypeCommon = GetCashFlowTypeCommon<T>();
        await cashFlowTypeCommon.UpdateTypeAsync(type).ConfigureAwait(false);
    }

    public async Task AddNewCashFlowTypeAsync<T>(CashFlowType<T> type)
    where T : CashFlow
    {
        var cashFlowTypeCommon = GetCashFlowTypeCommon<T>();
        await cashFlowTypeCommon.AddNewTypeAsync(type).ConfigureAwait(false);
    }

    public async Task DeleteCashFlowTypeAsync<T>(long typeId)
    where T : CashFlow
    {
        var cashFlowTypeCommon = GetCashFlowTypeCommon<T>();
        await cashFlowTypeCommon.DeleteTypeAsync(typeId).ConfigureAwait(false);
    }

    public async Task<CurrencyExchangeRate?> GetExchangeRateAsync(CurrencyExchangeRate rateSetup)
        => _ExchangeRateCommon != null ? 
            await _ExchangeRateCommon.GetExchangeRateAsync(rateSetup).ConfigureAwait(false) :
            throw new NullReferenceException("Context Not Initialized!");

    public async Task<List<PaymentMethod>> GetAllMethodsAsync()
        => _PaymentMethodCommon != null ?
            await _PaymentMethodCommon.GetAllMethodsAsync().ConfigureAwait(false) :
            throw new NullReferenceException("Context Not Initialized!");

    public async Task UpdateMethodTypeAsync(PaymentMethod method)
    {
        if (_PaymentMethodCommon == null)
            throw new NullReferenceException("Context Not Initialized!");
        await _PaymentMethodCommon.UpdateMethodAsync(method).ConfigureAwait(false);
    }
    public async Task AddNewMethodTypeAsync(PaymentMethod method)
    {
        if (_PaymentMethodCommon == null)
            throw new NullReferenceException("Context Not Initialized!");
        await _PaymentMethodCommon.AddNewMethodAsync(method).ConfigureAwait(false);
    }
    public async Task DeleteMethodTypeAsync(long methodId)
    {
        if (_PaymentMethodCommon == null)
            throw new NullReferenceException("Context Not Initialized!");
        await _PaymentMethodCommon.DeleteMethodAsync(methodId).ConfigureAwait(false);
    }

    public async Task<CashFlowRecord<T>?> GetCashFlowRecordByIDAsync<T>(long cashflowID) 
    where T : CashFlow
    {
        var cashFlowRecordCommon = GetCashFlowRecordCommon<T>();

        var record = await cashFlowRecordCommon.GetRecordByIDAsync(cashflowID).ConfigureAwait(false);
        await EnrichCashFlowRecordAsync<T>(record).ConfigureAwait(false);
        return record;
    }
    
    public async Task<List<CashFlowRecord<T>>> GetCashFlowRecordsByTimeSpanAsync<T>(DateTime startTime, DateTime endTime)
    where T : CashFlow
    {
        var cashFlowRecordCommon = GetCashFlowRecordCommon<T>();

        var records = await cashFlowRecordCommon.GetRecordsForTimeSpanAsync(startTime, endTime).ConfigureAwait(false);
        foreach (var record in records) await EnrichCashFlowRecordAsync<T>(record).ConfigureAwait(false);
        return records;
    }

    public async Task UpdateCashFlowRecordAsync<T>(CashFlowRecord<T> record)
    where T : CashFlow
    {
        var cashFlowRecordCommon = GetCashFlowRecordCommon<T>();
        await cashFlowRecordCommon.UpdateRecordAsync(record).ConfigureAwait(false);
    }

    public async Task AddNewCashFlowRecordAsync<T>(CashFlowRecord<T> record)
    where T : CashFlow
    {
        var cashFlowRecordCommon = GetCashFlowRecordCommon<T>();
        await cashFlowRecordCommon.AddNewRecordAsync(record).ConfigureAwait(false);
    }

    public async Task DeleteCashFlowRecordAsync<T>(long cashflowID)
    where T : CashFlow
    {
        var cashFlowRecordCommon = GetCashFlowRecordCommon<T>();
        await cashFlowRecordCommon.RemoveRecordAsync(cashflowID).ConfigureAwait(false);
    }

    private async Task<CashFlowRecord<T>?> EnrichCashFlowRecordAsync<T>(CashFlowRecord<T>? record)
    where T : CashFlow
    {
        if (record == null) return null;

        var currencies = await _CurrencyCommon.GetAllCurrenciesAsDictAsync().ConfigureAwait(false);
        var methods = await _PaymentMethodCommon.GetAllMethodsAsDictAsync().ConfigureAwait(false);
        var types = await this.GetCashFlowTypeCommon<T>().GetAllTypesAsDictAsync().ConfigureAwait(false);
        record.CurrName = currencies[record.CurrIso].CurrName;
        record.MethodName = methods[record.MethodId].MethodName;
        record.TypeName = types[record.TypeId].TypeName;

        return record;
    }

    private CashFlowRecordCommon<T> GetCashFlowRecordCommon<T>()
    where T : CashFlow
    {
        if (_CashFlowRecordCommons == null || ! _CashFlowRecordCommons.ContainsKey(typeof(T)))
            throw new NullReferenceException("Context Not Initialized!");
        return (CashFlowRecordCommon<T>) _CashFlowRecordCommons[typeof(T)];
    }

    private CashFlowTypeCommon<T> GetCashFlowTypeCommon<T>()
    where T : CashFlow
    {
        if (_CashFlowTypeCommons == null || ! _CashFlowTypeCommons.ContainsKey(typeof(T)))
            throw new NullReferenceException("Context Not Initialized!");
        return (CashFlowTypeCommon<T>) _CashFlowTypeCommons[typeof(T)];
    }
}