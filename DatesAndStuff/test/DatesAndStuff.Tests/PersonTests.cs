namespace DatesAndStuff.Tests;

using AutoFixture;
using FluentAssertions;

[TestFixture]
public class PersonTests
{
    [SetUp]
    public void Setup()
    {
    }

    [TestFixture]
    public class MarrigeTests
    {
        [Test]
        public void GotMerriedFirstNameShouldChange()
        {
            // Arrange
            var sut = PersonFactory.CreateTestPerson();
            var newName = "Test-Eleso Pista";
            var beforeChanges = Person.Clone(sut);

            // Act
            sut.GotMarried(newName);

            // Assert
            sut.Name.Should().Be(newName);
            sut.Should().BeEquivalentTo(beforeChanges, o => o.Excluding(p => p.Name));
        }

        [Test]
        public void GotMerriedSecondShouldFail()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Customize<IPaymentService>(
                c => c.FromFactory(() => new TestPaymentService())
            );
            var freshPerson = fixture.Create<Person>();
            var newName = "Test-Eleso-Felallo Pista";
            freshPerson.GotMarried(newName);

            // Act
            var action = () => freshPerson.GotMarried("Valalmi uj nev");

            // Assert
            action.Should().Throw<Exception>().WithMessage("Poligamy not yet supported.");
        }
    }

    [TestFixture]
    public class SalaryTests
    {
        [Test]
        [TestCase(1)]
        [TestCase(5)]
        [TestCase(10)]
        [TestCase(25)]
        [TestCase(50)]
        public void IncreaseSalaryReasonableValueShouldModifySalary(double salaryIncreasePercentage)
        {
            // Arrange
            var sut = PersonFactory.CreateTestPerson();
            var initialSalary = sut.Salary;

            // Act
            sut.IncreaseSalary(salaryIncreasePercentage);

            // Assert
            sut.Salary.Should()
                .BeGreaterThan(initialSalary, "Salary should increase when given a positive percentage.");
        }

        [Test]
        [TestCase(0)]
        [TestCase(-5)]
        [TestCase(-9)]
        public void IncreaseSalaryInvalidValuesShouldNotIncreaseSalary(double salaryIncreasePercentage)
        {
            // Arrange
            var sut = PersonFactory.CreateTestPerson();
            var initialSalary = sut.Salary;

            // Act
            sut.IncreaseSalary(salaryIncreasePercentage);

            // Assert
            if (salaryIncreasePercentage == 0)
            {
                sut.Salary.Should().Be(initialSalary, "A 0% increase should not change the salary.");
            }
            else
            {
                sut.Salary.Should().BeLessThan(initialSalary, "A negative salary increase should decrease the salary.");
            }
        }

        [Test]
        [TestCase(-10)]
        [TestCase(-15)]
        [TestCase(-50)]
        public void IncreaseSalaryTooLargeNegativeValueShouldFail(double salaryIncreasePercentage)
        {
            // Arrange
            var sut = PersonFactory.CreateTestPerson();

            // Act
            var action = () => sut.IncreaseSalary(salaryIncreasePercentage);

            // Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }
    }


    [TestFixture]
    public class ConstrictorTests
    {
        [Test]
        public void ConstructorWithDefaultParamsShouldBeAbleToEatChocolate()
        {
            // Arrange

            // Act
            var sut = PersonFactory.CreateTestPerson();

            // Assert
            sut.CanEatChocolate.Should().BeTrue();
        }

        [Test]
        public void ConstructorDontLikeChocolateShouldNotBeAbleToEatChocolate()
        {
            // Arrange

            // Act
            var sut = PersonFactory.CreateTestPerson(
                fp => fp.CanEatChocolate = false
            );

            // Assert
            sut.CanEatChocolate.Should().BeFalse();
        }

        [Test]
        [CustomPersonCreationAutodata(true)]
        public void ConstructorYearlyTaxDataShouldBeZero(Person sut)
        {
            var yearlyTax = LocalTaxData.YearlyTax;

            yearlyTax.Should().Be(0);
        }
    }
}
