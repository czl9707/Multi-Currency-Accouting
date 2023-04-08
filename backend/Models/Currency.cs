namespace Accountant.Models;

public class Currency 
{
    public Currency ()
    {
        this.CurrIso = "";
        this.CurrName = "";
    }

    public Currency(string currName): this()
    {
        this.CurrName = currName;
    }

    public string CurrIso {get; set;}
    public string CurrName {get; set;}
}