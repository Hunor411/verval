using AutoFixture;
using FluentAssertions;

namespace DatesAndStuff.Tests;

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
        public void GotMerried_First_NameShouldChange()
        {
            // Arrange
            var sut = PersonFactory.CreateTestPerson();
            string newName = "Test-Eleso Pista";
            var beforeChanges = Person.Clone(sut);

            // Act
            sut.GotMarried(newName);

            // Assert
            sut.Name.Should().Be(newName);
            sut.Should().BeEquivalentTo(beforeChanges, o => o.Excluding(p => p.Name));
        }

        [Test]
        public void GotMerried_Second_ShouldFail()
        {
            // Arrange
            var fixture = new AutoFixture.Fixture();
            fixture.Customize<IPaymentService>(
                c => c.FromFactory(() => new TestPaymentService())
                );
            var freshPerson = fixture.Create<Person>();
            string newName = "Test-Eleso-Felallo Pista";
            freshPerson.GotMarried(newName);

            // Act
            Action action = () => freshPerson.GotMarried("Valalmi uj nev");

            // Assert
            action.Should().Throw<Exception>().WithMessage("Poligamy not yet supported.");
        }   
    }
    
    [TestFixture]
    public class SalaryTests
    {
        [Test]
        [CustomPersonCreationAutodata]
        public void PositiveIncrease_ShouldIncreaseSalary(Person sut,double salaryIncreasePercentage )
        {
            // Arrange
            double initialSalary = sut.Salary;
            
            // Act
            sut.IncreaseSalary(10);
            
            // Assert
            sut.Salary.Should().BeGreaterThan(initialSalary, "Salary should increase when given a positive percentage.");
        }

        [Test]
        [CustomPersonCreationAutodata]
        public void ZeroPercentIncrease_ShouldNotChangeSalary(Person sut)
        {
            // Arrange
            double initialSalary = sut.Salary;
            
            // Act
            sut.IncreaseSalary(0);

            // Assert
            sut.Salary.Should().Be(initialSalary, "A 0% increase should not change the salary.");
        }

        [Test]
        [CustomPersonCreationAutodata]
        public void NegativeIncrease_ShouldDecreaseSalary(Person sut)
        {
            // Arrange
            double initialSalary = sut.Salary;
            
            // Act
            sut.IncreaseSalary(-5);
            
            // Assert
            sut.Salary.Should().BeLessThan(initialSalary, "A negative salary increase should decrease the salary.");
        }

        [Test]
        [CustomPersonCreationAutodata]
        public void SmallerThanMinusTenPercent_ShouldFail(Person sut)
        {
            // Arrange
            
            // Act
            Action action = () => sut.IncreaseSalary(-15);
            
            // Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }
    }

    [TestFixture]
    public class ConstrictorTests
    {
        [Test]
        public void Constructor_DefaultParams_ShouldBeAbleToEatChocolate()
        {
            // Arrange

            // Act
            Person sut = PersonFactory.CreateTestPerson();

            // Assert
            sut.CanEatChocolate.Should().BeTrue();
        }

        [Test]
        public void Constructor_DontLikeChocolate_ShouldNotBeAbleToEatChocolate()
        {
            // Arrange

            // Act
            Person sut = PersonFactory.CreateTestPerson(
                fp => fp.CanEatChocolate = false
                );

            // Assert
            sut.CanEatChocolate.Should().BeFalse();
        }
    }
}
