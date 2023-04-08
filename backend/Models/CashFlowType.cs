namespace Accountant.Models;

public abstract class CashFlowType<T>
where T : CashFlow
{
    protected CashFlowType()
    {
        this.TypeName = "";
        this.TypeId = -1;
    }

    protected CashFlowType (string typeName)
    {
        this.TypeName = typeName;
        this.TypeId = -1;
    }

    public long TypeId {get; set;}
    public string TypeName {get; set;}
}

public class IncomeType : CashFlowType<Income>
{
    public IncomeType (): base(){}
    public IncomeType (string typeName): base(typeName){}
}

public class ExpenseType : CashFlowType<Expense>
{
    public ExpenseType (): base(){}
    public ExpenseType (string typeName): base(typeName){}

}