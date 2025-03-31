namespace DatesAndStuff.Tests;

using FluentAssertions;
using Moq;

internal sealed class PaymentServiceTest
{
    [Test]
    public void TestPaymentService_ManualMock_SufficientBalance()
    {
        // Arrange
        var sut = new Person("Test Pista",
            new EmploymentInformation(
                54,
                new Employer("RO1234567", "Valami city valami hely", "Dagobert bacsi", new List<int> { 6201, 7210 })),
            new TestPaymentService(),
            new LocalTaxData("4367558"),
            new FoodPreferenceParams
            {
                CanEatChocolate = true, CanEatEgg = true, CanEatLactose = true, CanEatGluten = true
            }
        );

        // Act
        var result = sut.PerformSubsriptionPayment();

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void TestPaymentService_ManualMock_InsufficientBalance()
    {
        // Arrange
        var sut = new Person("Test Pista",
            new EmploymentInformation(
                54,
                new Employer("RO1234567", "Valami city valami hely", "Dagobert bacsi", new List<int> { 6201, 7210 })),
            new TestPaymentService(200),
            new LocalTaxData("4367558"),
            new FoodPreferenceParams
            {
                CanEatChocolate = true, CanEatEgg = true, CanEatLactose = true, CanEatGluten = true
            }
        );

        // Act
        var result = sut.PerformSubsriptionPayment();

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void TestPaymentService_Mock_SufficientBalance()
    {
        // Arrange
        var paymentSequence = new MockSequence();
        var paymentService = new Mock<IPaymentService>(MockBehavior.Strict);

        paymentService.Setup(p => p.Balance).Returns(Person.SubscriptionFee + 100);

        paymentService.InSequence(paymentSequence).Setup(m => m.StartPayment()).Verifiable();
        paymentService.InSequence(paymentSequence).Setup(m => m.SpecifyAmount(Person.SubscriptionFee)).Verifiable();
        paymentService.InSequence(paymentSequence).Setup(m => m.ConfirmPayment()).Verifiable();

        var paymentServiceMock = paymentService.Object;

        var sut = new Person("Test Pista",
            new EmploymentInformation(
                54,
                new Employer("RO1234567", "Valami city valami hely", "Dagobert bacsi", new List<int> { 6201, 7210 })),
            paymentServiceMock
            ,
            new LocalTaxData("4367558"),
            new FoodPreferenceParams
            {
                CanEatChocolate = true, CanEatEgg = true, CanEatLactose = true, CanEatGluten = true
            }
        );

        // Act
        var result = sut.PerformSubsriptionPayment();

        // Assert
        result.Should().BeTrue();
        paymentService.Verify(m => m.StartPayment(), Times.Once);
        paymentService.Verify(m => m.SpecifyAmount(Person.SubscriptionFee), Times.Once);
        paymentService.Verify(m => m.ConfirmPayment(), Times.Once);
    }

    [Test]
    public void TestPaymentService_Mock_InsufficientBalance()
    {
        // Arrange
        var paymentSequence = new MockSequence();
        var paymentService = new Mock<IPaymentService>(MockBehavior.Strict);

        paymentService.Setup(p => p.Balance).Returns(Person.SubscriptionFee - 100);

        paymentService.InSequence(paymentSequence).Setup(m => m.StartPayment()).Verifiable();
        paymentService.InSequence(paymentSequence).Setup(m => m.CancelPayment()).Verifiable();
        paymentService.InSequence(paymentSequence).Setup(m => m.SpecifyAmount(Person.SubscriptionFee)).Verifiable();
        paymentService.InSequence(paymentSequence).Setup(m => m.ConfirmPayment()).Verifiable();

        var paymentServiceMock = paymentService.Object;

        var sut = new Person("Test Pista",
            new EmploymentInformation(
                54,
                new Employer("RO1234567", "Valami city valami hely", "Dagobert bacsi", new List<int> { 6201, 7210 })),
            paymentServiceMock
            ,
            new LocalTaxData("4367558"),
            new FoodPreferenceParams
            {
                CanEatChocolate = true, CanEatEgg = true, CanEatLactose = true, CanEatGluten = true
            }
        );

        // Act
        var result = sut.PerformSubsriptionPayment();

        // Assert
        result.Should().BeFalse();
        paymentService.Verify(m => m.StartPayment(), Times.Once);
        paymentService.Verify(m => m.CancelPayment(), Times.Once);
        paymentService.Verify(m => m.SpecifyAmount(Person.SubscriptionFee), Times.Never);
        paymentService.Verify(m => m.ConfirmPayment(), Times.Never);
    }

    [Test]
    [CustomPersonCreationAutodataAttribute(true)]
    public void TestPaymentService_MockWithAutodata_SufficientBalance(Person sut, Mock<IPaymentService> paymentService)
    {
        // Arrange

        // Act
        var result = sut.PerformSubsriptionPayment();

        // Assert
        result.Should().BeTrue();
        paymentService.Verify(m => m.StartPayment(), Times.Once);
        paymentService.Verify(m => m.SpecifyAmount(Person.SubscriptionFee), Times.Once);
        paymentService.Verify(m => m.ConfirmPayment(), Times.Once);
    }

    [Test]
    [CustomPersonCreationAutodataAttribute(false)]
    public void TestPaymentService_MockWithAutodata_InsufficientBalance(Person sut,
        Mock<IPaymentService> paymentService)
    {
        // Arrange

        // Act
        var result = sut.PerformSubsriptionPayment();

        // Assert
        result.Should().BeFalse();
        paymentService.Verify(m => m.StartPayment(), Times.Once);
        paymentService.Verify(m => m.CancelPayment(), Times.Once);
        paymentService.Verify(m => m.SpecifyAmount(Person.SubscriptionFee), Times.Never);
        paymentService.Verify(m => m.ConfirmPayment(), Times.Never);
    }
}
