namespace DatesAndStuff;

public interface IPaymentService
{
    public double Balance { get; }
    public void StartPayment();

    public void SpecifyAmount(double amount);

    public void ConfirmPayment();

    public void CancelPayment();
}
