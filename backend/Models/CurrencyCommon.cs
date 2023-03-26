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
    }

    private const string sqlSelect = "Select curr_iso, curr_name FROM tbl_currency;";
    private Dictionary<string, Currency>? Currencies;

    public async Task<Dictionary<string, Currency>> GetAllCurrenciesAsync()
    {
        if (this.Currencies is null)
        {
            using var connection = this.DBConnectionFactory.GetConnection();
            var currencies = await this.DapperWrapperService.QueryAsync<Currency>(connection, sqlSelect).ConfigureAwait(false);

            this.Currencies = currencies.ToDictionary(c => c.CurrIso, c => c);
        }

        return this.Currencies;
    }
}