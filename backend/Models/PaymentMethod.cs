namespace Accountant.Models;

public class PaymentMethod{
    public PaymentMethod ()
    {
        this.MethodName = "";
        this.MethodId = -1;
    }

    public PaymentMethod(
        string methodName
    )
    {
        this.MethodName = methodName;
        this.MethodId = -1;
    }

    public long MethodId {get; set;}
    public string MethodName {get; set;}
}