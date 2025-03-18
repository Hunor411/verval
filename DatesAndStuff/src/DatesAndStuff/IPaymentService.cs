
namespace DatesAndStuff
{
    public interface IPaymentService
    {
        public void StartPayment();

        public void SpecifyAmount(double amount);

        public void ConfirmPayment();
        
        public double Balance { get; }
        
        public void CancelPayment();
    }
}
