namespace Accountant.Models;

public class CashFlowRecord<T>
where T : CashFlow
{
    public CashFlowRecord(
        DateTime happenUtc = default,
        float amount = default,
        string currIso = "",
        string note = "",
        long typeId = -1,
        long methodId = -1
    ): this(){
        this.HappenUtc = happenUtc;
        this.Amount = amount;
        this.CurrIso = currIso;
        this.Note = note;
        this.TypeId = typeId;
        this.MethodId = methodId;

        this.CashFlowId = -1;
    }

    public CashFlowRecord(){
        this.Curr = new Currency();
        this.Type = new CashFlowType<T>();
        this.Method = new PaymentMethod();
        this.HappenUtc = default;
        this.Amount = default;
        this.Note = "";

        this.CashFlowId = -1;
    }

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
    ) : base(
        happenUtc,
        amount,
        currIso,
        note,
        typeId,
        methodId
    ){
        this.Type = new ExpenseType();
    }

    public ExpenseRecord():base(){
        this.Type = new ExpenseType();
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
    ) : base(
        happenUtc,
        amount,
        currIso,
        note,
        typeId,
        methodId
    ){
        this.Type = new IncomeType();
    }

    public IncomeRecord():base(){
        this.Type = new IncomeType();
    }
}