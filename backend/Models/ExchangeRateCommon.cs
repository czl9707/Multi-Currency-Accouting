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


    public async Task<CurrencyExchangeRate?> GetExchangeRateAsync(CurrencyExchangeRate rateSetup)
    {
        var rate = await this.GetExchangeRateFromDBAsync(rateSetup).ConfigureAwait(false);
        if (rate != null) return rate;
        
        try{
            rate = await GrabExchangeRateAsync(rateSetup).ConfigureAwait(false);
        }catch (NullReferenceException) {
            // log error
            return null;
        }

        await this.InsertExchangeRateIntoDBAsync(rate).ConfigureAwait(false);
        return rate;
    }

    private async Task<CurrencyExchangeRate?> GetExchangeRateFromDBAsync(CurrencyExchangeRate rateSetup)
    {
        using var connection = this.DbConnectionFactory.GetConnection();
        var rates = await this.DapperWrapperService.QueryAsync<CurrencyExchangeRate>(
            connection: connection,
            sql: sqlSelectSingleRate,
            param: new {
                vbase_cur = rateSetup.BaseCur,
                vtarget_cur = rateSetup.TargetCur,
                vexchange_utc = rateSetup.ExchangeUtc
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

    private static async Task<CurrencyExchangeRate> GrabExchangeRateAsync(CurrencyExchangeRate rateSetup)
    {
        var client = new HttpClient();
        // transfer to format 2020-10-21
        var dateString = rateSetup.ExchangeRate.ToString("yyyy-MM-dd");
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(
                $"https://currency-conversion-and-exchange-rates.p.rapidapi.com/{dateString}?base={rateSetup.BaseCur}&quotes={rateSetup.TargetCur}"
            ),
            Headers = {
                { "X-RapidAPI-Key", "89dfb95b7cmsh5a31dc13a7b4da0p1cd15cjsn94b25d25bea2" },
		        { "X-RapidAPI-Host", "currency-conversion-and-exchange-rates.p.rapidapi.com" },
            }
        };
        
        using var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        return GenerateExchangeRateFromJson(body, rateSetup);
    }

    private static CurrencyExchangeRate GenerateExchangeRateFromJson(string responseBody, CurrencyExchangeRate rateSetup)
    {
        var jobj = JObject.Parse(responseBody);
        JObject? rates = (JObject?)jobj["rates"];
        if (rates == null || !rates.ContainsKey(rateSetup.TargetCur))
            throw new NullReferenceException(
                $"Remote response do not contain rate from {rateSetup.BaseCur} to {rateSetup.TargetCur}."
            );

        float rate = -1;
        if (rates.HasValues && rates.ContainsKey(rateSetup.TargetCur)){            
            rate = (float)rates[rateSetup.TargetCur];
        }

        rateSetup.ExchangeRate = rate;
        return rateSetup;
    }
}