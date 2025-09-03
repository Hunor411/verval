namespace DatesAndStuff.Tests;

internal sealed class TestPaymentService : IPaymentService
{
    private uint confirmCallCount;
    private uint specifyCallCount;
    private uint startCallCount;

    public TestPaymentService(double initialBalance = 1000) => this.Balance = initialBalance;

    public double Balance { get; private set; }

    public void StartPayment()
    {
        if (this.startCallCount != 0 || this.specifyCallCount > 0 || this.confirmCallCount > 0)
        {
            throw new InvalidOperationException("StartPayment called in wrong state.");
        }

        this.startCallCount++;
    }

    public void SpecifyAmount(double amount)
    {
        if (this.startCallCount != 1 || this.specifyCallCount > 0 || this.confirmCallCount > 0)
        {
            throw new InvalidOperationException("SpecifyAmount called in wrong state.");
        }

        if (amount > this.Balance)
        {
            throw new InvalidOperationException("Insufficient balance.");
        }

        this.specifyCallCount++;
        this.Balance -= amount;
    }

    public void ConfirmPayment()
    {
        if (this.startCallCount != 1 || this.specifyCallCount != 1 || this.confirmCallCount > 0)
        {
            throw new InvalidOperationException("ConfirmPayment called in wrong state.");
        }

        this.confirmCallCount++;
    }

    public void CancelPayment()
    {
        if (this.startCallCount == 0)
        {
            throw new InvalidOperationException("No active payment to cancel.");
        }

        this.startCallCount = 0;
        this.specifyCallCount = 0;
        this.confirmCallCount = 0;
    }

    public bool SuccessFul() => this.startCallCount == 1 && this.specifyCallCount == 1 && this.confirmCallCount == 1;
}
