namespace Accountant.Models;

public class PaymentMethod{
    public PaymentMethod ()
    {
        this.MethodName = "";
    }

    public long MethodId {get; set;}
    public string MethodName {get; set;}
}