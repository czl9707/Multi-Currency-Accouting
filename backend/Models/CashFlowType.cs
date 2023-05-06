namespace Accountant.Models;

public class CashFlowType<T>
where T : CashFlow
{
    public CashFlowType()
    {
        this.TypeName = "";
        this.TypeId = -1;
    }

    public CashFlowType (string typeName)
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