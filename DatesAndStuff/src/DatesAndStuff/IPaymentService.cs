
namespace DatesAndStuff
{
    public interface IPaymentService
    {
        public void StartPayment();

        public void SpecifyAmount(double amount);

        public void ConfirmPayment();

        public bool SuccessFul();

        public double Balance { get; }
        
        public void CancelPayment();
    }
}
