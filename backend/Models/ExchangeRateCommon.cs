namespace Accountant.Models;

using Accountant.Services.DB;
using System.Net.Http;
using Newtonsoft.Json.Linq;


public class ExchangeRateCommon {
    private IDBConnectionFactory DbConnectionFactory;
    private IDapperWrapperService DapperWrapperService;

    public ExchangeRateCommon(
        IDBConnectionFactory dBConnectionFactory,
        IDapperWrapperService dapperWrapperService
    )
    {
        this.DbConnectionFactory = dBConnectionFactory;
        this.DapperWrapperService = dapperWrapperService;
    }

    private const string sqlSelectSingleRate = 
        @"
        SELECT base_cur, target_cur, exchange_utc, exchange_rate
        FROM tbl_currency_exchange
        WHERE base_cur=@vbase_cur
            AND target_cur=@vtarget_cur
            AND exchange_utc=@vexchange_utc
        ";
    
    private const string sqlInsertSingleRate = 
        @"
        INSERT INTO tbl_currency_exchange
            (base_cur, target_cur, exchange_utc, exchange_rate)
        VALUES 
            (@vbase_cur, @vtarget_cur, @vexchange_utc, @vexchange_rate)
        ";


    public async Task<CurrencyExchangeRate?> GetExchangeRateAsync(string base_cur, string target_cur, DateTime exchange_utc)
    {
        var rate = await this.GetExchangeRateFromDBAsync(base_cur, target_cur, exchange_utc).ConfigureAwait(false);
        if (rate != null) return rate;
        
        try{
            rate = await GrabExchangeRateAsync(base_cur, target_cur, exchange_utc).ConfigureAwait(false);
        }catch (NullReferenceException) {
            // log error
            return null;
        }

        await this.InsertExchangeRateIntoDBAsync(rate).ConfigureAwait(false);
        Console.WriteLine("inserted");
        return rate;
    }

    private async Task<CurrencyExchangeRate?> GetExchangeRateFromDBAsync(string base_cur, string target_cur, DateTime exchange_utc)
    {
        using var connection = this.DbConnectionFactory.GetConnection();
        var rates = await this.DapperWrapperService.QueryAsync<CurrencyExchangeRate>(
            connection: connection,
            sql: sqlSelectSingleRate,
            param: new {
                vbase_cur = base_cur,
                vtarget_cur = target_cur,
                vexchange_utc = exchange_utc
            }
        ).ConfigureAwait(false);

        if (rates.Any()) return rates.First();
        return null;
    }

    private async Task InsertExchangeRateIntoDBAsync(CurrencyExchangeRate rate)
    {
        using var connection = this.DbConnectionFactory.GetConnection();
        await this.DapperWrapperService.ExecuteAsync(
            connection: connection,
            sql: sqlInsertSingleRate,
            param: new {
                vbase_cur = rate.BaseCur,
                vtarget_cur = rate.TargetCur,
                vexchange_utc = rate.ExchangeUtc,
                vexchange_rate = rate.ExchangeRate
            }
        );
    }

    private static async Task<CurrencyExchangeRate> GrabExchangeRateAsync(string base_cur, string target_cur, DateTime exchange_utc)
    {
        var client = new HttpClient();
        // transfer to format 2020-10-21
        var dateString = exchange_utc.ToString("yyyy-MM-dd");
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(
                $"https://currency-conversion-and-exchange-rates.p.rapidapi.com/{dateString}?base={base_cur}&quotes={target_cur}"            
            ),
            Headers = {
                { "X-RapidAPI-Key", "89dfb95b7cmsh5a31dc13a7b4da0p1cd15cjsn94b25d25bea2" },
		        { "X-RapidAPI-Host", "currency-conversion-and-exchange-rates.p.rapidapi.com" },
            }
        };
        
        using var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        return GenerateExchangeRateFromJson(body, base_cur, target_cur, exchange_utc);
    }

    private static CurrencyExchangeRate GenerateExchangeRateFromJson(string responseBody, string base_cur, string target_cur, DateTime exchange_utc)
    {
        var jobj = JObject.Parse(responseBody);
        JObject? rates = (JObject?)jobj["rates"];
        if (rates == null || !rates.ContainsKey(target_cur))
            throw new NullReferenceException(
                $"Remote response do not contain rate from {base_cur} to {target_cur}."
            );

        float rate = -1;
        if (rates != null && rates.ContainsKey(target_cur)){
            rate = (float)rates[target_cur];
        }

        return new CurrencyExchangeRate(base_cur, target_cur, exchange_utc, rate);
    }
}