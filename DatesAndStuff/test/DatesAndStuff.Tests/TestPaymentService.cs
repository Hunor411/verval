
namespace DatesAndStuff.Tests
{
    internal class TestPaymentService : IPaymentService
    {
        uint _startCallCount = 0;
        uint _specifyCallCount = 0;
        uint _confirmCallCount = 0;
        private double _balance;

        public double Balance => _balance;

        public TestPaymentService(double initialBalance = 1000)
        {
            _balance = initialBalance;
        }

        public void StartPayment()
        {
            if (_startCallCount != 0 || _specifyCallCount > 0 || _confirmCallCount > 0)
                throw new Exception("Invalid payment flow: StartPayment called in wrong state.");

            _startCallCount++;
        }

        public void SpecifyAmount(double amount)
        {
            if (_startCallCount != 1 || _specifyCallCount > 0 || _confirmCallCount > 0)
                throw new Exception("Invalid payment flow: SpecifyAmount called in wrong state.");

            if (amount > _balance)
                throw new Exception("Insufficient balance.");
            
            _specifyCallCount++;
            _balance -= amount;
        }

        public void ConfirmPayment()
        {
            if (_startCallCount != 1 || _specifyCallCount != 1 || _confirmCallCount > 0)
                throw new Exception("Invalid payment flow: ConfirmPayment called in wrong state.");

            _confirmCallCount++;
        }
        
        public void CancelPayment()
        {
            if (_startCallCount == 0)
                throw new Exception("No active payment to cancel.");

            _startCallCount = 0;
            _specifyCallCount = 0;
            _confirmCallCount = 0;
        }
    }
}
