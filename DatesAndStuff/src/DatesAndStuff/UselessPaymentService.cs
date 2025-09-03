namespace DatesAndStuff;

public class UselessPaymentService : IPaymentService
{
    public double Balance => double.PositiveInfinity;

    public void ConfirmPayment()
    {
    }

    public void SpecifyAmount(double amount)
    {
    }

    public void StartPayment()
    {
    }

    public void CancelPayment()
    {
    }

    public bool SuccessFul() => true;
}
