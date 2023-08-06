namespace Accountant.Models;

public class PaymentMethod{
    public const long UNKNOWN = 0;
    public PaymentMethod ()
    {
        this.MethodName = "";
        this.MethodId = UNKNOWN;
    }

    public PaymentMethod(
        string methodName
    )
    {
        this.MethodName = methodName;
        this.MethodId = UNKNOWN;
    }

    public long MethodId {get; set;}
    public string MethodName {get; set;}
}