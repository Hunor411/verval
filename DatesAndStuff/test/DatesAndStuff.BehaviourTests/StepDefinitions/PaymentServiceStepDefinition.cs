namespace DatesAndStuff.BehaviourTests.StepDefinitions;

using Moq;

[Binding]
public class PaymentServiceStepDefinition
{
    private Mock<IPaymentService>? paymentServiceMock;
    private Person? person;
    private bool paymentResult;

    [Given("the user starts the payment process with sufficient balance of (.*)")]
    [Given("the user starts the payment process with insufficient balance of (.*)")]
    public void GivenTheUserStartsThePaymentProcessWithBalance(double balance)
    {
        this.paymentServiceMock = new Mock<IPaymentService>(MockBehavior.Strict);
        this.paymentServiceMock.Setup(p => p.Balance).Returns(balance);
        this.paymentServiceMock.Setup(p => p.StartPayment()).Verifiable();

        if (balance >= Person.SubscriptionFee)
        {
            this.paymentServiceMock.Setup(p => p.SpecifyAmount(Person.SubscriptionFee)).Verifiable();
            this.paymentServiceMock.Setup(p => p.ConfirmPayment()).Verifiable();
            this.paymentServiceMock.Setup(p => p.SuccessFul()).Returns(true);
        }
        else
        {
            this.paymentServiceMock.Setup(p => p.CancelPayment()).Verifiable();
            this.paymentServiceMock.Setup(p => p.SuccessFul()).Returns(false);
        }

        this.person = new Person("Test Pista",
            new EmploymentInformation(
                54,
                new Employer("RO1234567", "Valami city valami hely", "Dagobert bacsi", new List<int> { 6201, 7210 })),
            this.paymentServiceMock.Object,
            new LocalTaxData("4367558"),
            new FoodPreferenceParams
            {
                CanEatChocolate = true, CanEatEgg = true, CanEatLactose = true, CanEatGluten = true
            }
        );
    }

    [When("the user attempts to pay the subscription fee")]
    public void WhenTheUserAttemptsToPayTheSubscriptionFee() =>
        this.paymentResult = this.person!.PerformSubsriptionPayment();

    [Then("the payment should be successful")]
    public void ThenThePaymentShouldBeSuccessful()
    {
        this.paymentResult.Should().BeTrue();
        this.paymentServiceMock!.Verify(p => p.StartPayment(), Times.Once);
        this.paymentServiceMock.Verify(p => p.SpecifyAmount(Person.SubscriptionFee), Times.Once);
        this.paymentServiceMock.Verify(p => p.ConfirmPayment(), Times.Once);
        this.paymentServiceMock.Verify(p => p.CancelPayment(), Times.Never);
    }

    [Then("the payment should be confirmed")]
    public void ThenThePaymentShouldBeConfirmed() =>
        this.paymentServiceMock!.Verify(p => p.ConfirmPayment(), Times.Once);

    [Then("the payment should fail due to insufficient balance")]
    public void ThenThePaymentShouldFailDueToInsufficientBalance()
    {
        this.paymentResult.Should().BeFalse();
        this.paymentServiceMock!.Verify(p => p.StartPayment(), Times.Once);
        this.paymentServiceMock.Verify(p => p.CancelPayment(), Times.Once);
        this.paymentServiceMock.Verify(p => p.SpecifyAmount(It.IsAny<double>()), Times.Never);
        this.paymentServiceMock.Verify(p => p.ConfirmPayment(), Times.Never);
    }

    [Then("the payment should be cancelled")]
    public void ThenThePaymentShouldBeCancelled() =>
        this.paymentServiceMock!.Verify(p => p.CancelPayment(), Times.Once);
}
