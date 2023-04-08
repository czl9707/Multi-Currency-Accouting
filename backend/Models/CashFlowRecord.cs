namespace Accountant.Models;

public interface ICashFlowRecord {}

public abstract class CashFlowRecord<T> : ICashFlowRecord
where T : CashFlow
{
    // force override
    protected CashFlowRecord(
        DateTime happenUtc = default,
        float amount = default,
        string currIso = "",
        string note = "",
        long typeId = -1,
        long methodId = -1
    ){}

    // force override
    protected CashFlowRecord(){}

    public long CashFlowId { get; set; }
    public DateTime HappenUtc { get; set; }
    public DateTime LastModifiedUtc { get; set; }
    public float Amount { get; set; }
    public string Note { get; set; }
    public string CurrIso 
    {
        get => this.Curr.CurrIso; 
        set => this.Curr.CurrIso = value; 
    }
    public string CurrName 
    { 
        get => this.Curr.CurrName; 
        set => this.Curr.CurrName = value; 
    }
    public long TypeId
    {
        get => this.Type.TypeId;
        set => this.Type.TypeId = value;
    }
    public string TypeName
    {
        get => this.Type.TypeName;
        set => this.Type.TypeName = value;
    }
    public long MethodId
    {
        get => this.Method.MethodId;
        set => this.Method.MethodId = value;
    }
    public string MethodName
    {
        get => this.Method.MethodName;
        set => this.Method.MethodName = value;
    }
    protected Currency Curr { get; set; }
    protected CashFlowType<T> Type { get; set; }
    protected PaymentMethod Method { get; set; }
}

public class ExpenseRecord : CashFlowRecord<Expense>
{
    public ExpenseRecord(
        DateTime happenUtc = default,
        float amount = default,
        string currIso = "",
        string note = "",
        long typeId = -1,
        long methodId = -1
    ) : this() 
    {
        this.HappenUtc = happenUtc;
        this.Amount = amount;
        this.CurrIso = currIso;
        this.Note = note;
        this.TypeId = typeId;
        this.MethodId = methodId;

        this.CashFlowId = -1;
    }

    public ExpenseRecord()
    {
        this.Curr = new Currency();
        this.Type = new ExpenseType();
        this.Method = new PaymentMethod();
        this.HappenUtc = default;
        this.Amount = default;
        this.Note = "";

        this.CashFlowId = -1;
    }
}

public class IncomeRecord : CashFlowRecord<Income>
{
    public IncomeRecord(
        DateTime happenUtc = default,
        float amount = default,
        string currIso = "",
        string note = "",
        long typeId = -1,
        long methodId = -1
    ) : this() 
    {
        this.HappenUtc = happenUtc;
        this.Amount = amount;
        this.CurrIso = currIso;
        this.Note = note;
        this.TypeId = typeId;
        this.MethodId = methodId;

        this.CashFlowId = -1;
    }

    public IncomeRecord()
    {
        this.Curr = new Currency();
        this.Type = new IncomeType();
        this.Method = new PaymentMethod();
        this.HappenUtc = default;
        this.Amount = default;
        this.Note = "";

        this.CashFlowId = -1;
    }
}