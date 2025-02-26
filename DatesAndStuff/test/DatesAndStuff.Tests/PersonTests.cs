using FluentAssertions;

namespace DatesAndStuff.Tests;

[TestFixture]
public class PersonTests
{
    Person sut;

    [SetUp]
    public void Setup()
    {
        this.sut = new Person("Test Pista", 54);
    }
    
    [TestFixture]
    public class MarrigeTests
    {
        private Person sut;

        public MarrigeTests()
        {
            this.sut = new Person("Test Pista", 54);
        }

        [Test]
        public void GotMerried_First_NameShouldChange()
        {
            // Arrange
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
            var freshPerson = new Person("Teszt Pista", 54);
            string newName = "Test-Eleso-Felallo Pista";
            freshPerson.GotMarried("");

            // Act
            Action action = () => freshPerson.GotMarried("");

            // Assert
            action.Should().Throw<Exception>().WithMessage("Poligamy not yet supported.");
        }   
    }
    
    [TestFixture]
    public class SalaryTests
    {
        private Person sut;

        public SalaryTests()
        {
            this.sut = new Person("Test Pista", 54);
        }
        
        [Test]
        public void PositiveIncrease_ShouldIncreaseSalary()
        {
            // Arrange
            double initialSalary = sut.Salary;
            
            // Act
            sut.IncreaseSalary(10);
            
            // Assert
            sut.Salary.Should().BeGreaterThan(initialSalary, "Salary should increase when given a positive percentage.");
        }

        [Test]
        public void ZeroPercentIncrease_ShouldNotChangeSalary()
        {
            // Arrange
            double initialSalary = sut.Salary;
            
            // Act
            sut.IncreaseSalary(0);

            // Assert
            sut.Salary.Should().Be(initialSalary, "A 0% increase should not change the salary.");
        }

        [Test]
        public void NegativeIncrease_ShouldDecreaseSalary()
        {
            // Arrange
            double initialSalary = sut.Salary;
            
            // Act
            sut.IncreaseSalary(-5);
            
            // Assert
            sut.Salary.Should().BeLessThan(initialSalary, "A negative salary increase should decrease the salary.");
        }

        [Test]
        public void SmallerThanMinusTenPercent_ShouldFail()
        {
            // Arrange
            
            // Act
            Action action = () => sut.IncreaseSalary(-15);
            
            // Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}