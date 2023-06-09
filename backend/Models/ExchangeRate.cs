namespace Accountant.Models;

public class CurrencyExchangeRate {
    public CurrencyExchangeRate()
    {
        this.BaseCur = "";
        this.TargetCur = "";
    }
    
    public CurrencyExchangeRate(
        string baseCur,
        string targetCur,
        DateTime exchangeUtc,
        float exchangeRate = default
    ){
        this.BaseCur = baseCur;
        this.TargetCur = targetCur;
        this.ExchangeUtc = exchangeUtc;
        this.ExchangeRate = exchangeRate;
    }

    public string BaseCur { get; set; }
    public string TargetCur { get; set; }
    public DateTime ExchangeUtc { get; set; }
    public float ExchangeRate { get; set; }
}