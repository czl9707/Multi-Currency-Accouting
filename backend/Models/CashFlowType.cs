namespace Accountant.Models;

public abstract class CashFlowType
{
    protected CashFlowType(){
        this.TypeId = 0;
        this.TypeName = "";
    }

    protected CashFlowType (
        long typeId,
        string typeName
    ){
        this.TypeId = typeId;
        this.TypeName = typeName;
    }

    internal long TypeId {get; private set;}
    internal string TypeName {get; set;}
}

public class IncomeType : CashFlowType
{
    public IncomeType (): base(){}
    public IncomeType (
        long typeId,
        string typeName
    ): base(
        typeId,
        typeName
    ){}
}

public class ExpenseType : CashFlowType
{
    public ExpenseType (): base(){}

    public ExpenseType (
        long typeId,
        string typeName
    ): base(
        typeId,
        typeName
    ){}
}