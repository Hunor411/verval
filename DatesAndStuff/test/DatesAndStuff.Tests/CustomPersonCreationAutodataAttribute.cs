namespace DatesAndStuff.Tests;

using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;
using Moq;

internal sealed class CustomPersonCreationAutodataAttribute : AutoDataAttribute
{
    public CustomPersonCreationAutodataAttribute(bool sufficientBalance)
        : base(() =>
        {
            var fixture = new Fixture();

            fixture.Customize(new AutoMoqCustomization());

            var paymentSequence = new MockSequence();
            var paymentService = new Mock<IPaymentService>(MockBehavior.Strict);

            if (sufficientBalance)
            {
                paymentService.Setup(p => p.Balance).Returns(Person.SubscriptionFee + 100);
                paymentService.InSequence(paymentSequence).Setup(m => m.StartPayment()).Verifiable();
                paymentService.InSequence(paymentSequence).Setup(m => m.SpecifyAmount(Person.SubscriptionFee))
                    .Verifiable();
                paymentService.InSequence(paymentSequence).Setup(m => m.ConfirmPayment()).Verifiable();
            }
            else
            {
                paymentService.Setup(p => p.Balance).Returns(Person.SubscriptionFee - 100);
                paymentService.InSequence(paymentSequence).Setup(m => m.StartPayment()).Verifiable();
                paymentService.InSequence(paymentSequence).Setup(m => m.CancelPayment()).Verifiable();
                paymentService.InSequence(paymentSequence).Setup(m => m.SpecifyAmount(Person.SubscriptionFee))
                    .Verifiable();
                paymentService.InSequence(paymentSequence).Setup(m => m.ConfirmPayment()).Verifiable();
            }

            fixture.Inject(paymentService);

            //fixture.Register<IPaymentService>(() => new TestPaymentService());

            double top = 20;
            double bottom = -11;
            fixture.Customize<double>(c => c.FromFactory(() => (new Random().NextDouble() * (top - bottom)) + bottom));
            return fixture;
        })
    {
    }
}
