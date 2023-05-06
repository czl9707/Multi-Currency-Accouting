namespace Accountant.Models;

using Accountant.Services.DB;

public class CurrencyCommon
{
    private IDBConnectionFactory DBConnectionFactory;
    private IDapperWrapperService DapperWrapperService;

    public CurrencyCommon(
        IDBConnectionFactory dBConnectionFactory,
        IDapperWrapperService dapperWrapperService)
    {
        this.DBConnectionFactory = dBConnectionFactory;
        this.DapperWrapperService = dapperWrapperService;

        this.InitCurrencyAysnc().GetAwaiter().GetResult();
    }

    private const string sqlSelect = "Select curr_iso, curr_name FROM tbl_currency;";
    private IEnumerable<Currency> Currencies;

    public async Task<List<Currency>> GetAllCurrenciesAsync() => 
        this.Currencies.ToList();

    public async Task<Dictionary<string, Currency>> GetAllCurrenciesAsDictAsync() => 
        this.Currencies.ToDictionary(c => c.CurrIso, c => c);

    private async Task InitCurrencyAysnc()
    {
        using var connection = this.DBConnectionFactory.GetConnection();
        this.Currencies = await this.DapperWrapperService.QueryAsync<Currency>(connection, sqlSelect).ConfigureAwait(false);
    }
}