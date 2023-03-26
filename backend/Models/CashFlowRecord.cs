namespace Accountant.Models;

public abstract class CashFlowRecord
{
    // For new record
    public CashFlowRecord(
        DateTime happenUtc,
        float amount,
        string currIso,
        string note,
        long typeId,
        long methodId
    )
    {
        this.HappenUtc = happenUtc;
        this.Amount = amount;
        this.CurrIso = currIso;
        this.Note = note;
        this.TypeId = typeId;
        this.MethodId = methodId;
    }

    // For record from DB
    public CashFlowRecord(){
        this.HappenUtc = DateTime.MinValue;
        this.Amount = 0;
        this.CurrIso = "";
        this.Note = "";
        this.TypeId = -1;
        this.MethodId = -1;
    }

    public long? IncomeId { get; private set; }
    public DateTime HappenUtc { get; set; }
    public DateTime? LastModifiedUtc { get; private set; }
    public float Amount { get; set; }
    public string CurrIso { get; set; }
    public string Note { get; set; }
    public long TypeId { get; set; }
    public long MethodId { get; set; }

    // below properties should be populated by context.
    public Currency? Curr { get; set; }
    public CashFlowType? Type { get; set; }
    public PaymentMethod? Method { get; set; }
}

public class ExpenseRecord : CashFlowRecord
{
    public ExpenseRecord(
        DateTime happenUtc,
        float amount,
        string currIso,
        string note,
        long typeId,
        long methodId
    ) : base(
        happenUtc,
        amount,
        currIso,
        note,
        typeId,
        methodId
    )
    { }

    public ExpenseRecord(): base(){}

    public new ExpenseType? Type { get; set; }
}

public class IncomeRecord : CashFlowRecord
{
    public IncomeRecord(
        DateTime happenUtc,
        float amount,
        string currIso,
        string note,
        long typeId,
        long methodId
    ) : base(
        happenUtc,
        amount,
        currIso,
        note,
        typeId,
        methodId
    )
    { }

    public IncomeRecord(): base(){}

    public new IncomeType? Type { get; set; }
}