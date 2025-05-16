namespace DatesAndStuff.BehaviourTests.StepDefinitions;

using AutoFixture;
using Moq;

[Binding]
public class SalaryIncreaseStepDefinitions
{
    private Person person;

    private bool salaryIncreaseSuccesfull;

    [Given(@"a person has a salary of (.*)")]
    public void GivenAPersonHasASalaryOf(double salary)
    {
        var fixture = new Fixture();
        fixture.Customize<double>(c => c.FromFactory(() => salary));
        fixture.Customize<IPaymentService>(c => c.FromFactory(
            () => new Mock<IPaymentService>().Object
        ));

        this.person = fixture.Build<Person>().Create();
    }

    [When(@"the salary is increased by (.*) percent")]
    public void WhenTheSalaryIsIncreasedByPercent(double percentage)
    {
        try
        {
            this.person.IncreaseSalary(percentage);
            this.salaryIncreaseSuccesfull = true;
        }
        catch
        {
            this.salaryIncreaseSuccesfull = false;
        }
    }

    [Then(@"the operation should be successful")]
    public void ThenTheOperationShouldBeSuccessful() => this.salaryIncreaseSuccesfull.Should().BeTrue();

    [Then(@"the operation should fail")]
    public void ThenTheOperationShouldFail() => this.salaryIncreaseSuccesfull.Should().BeFalse();


    [Then(@"the new salary should be (.*)")]
    public void ThenTheNewSalaryShouldBe(int expectedSalary) => this.person.Salary.Should().Be(expectedSalary);
}
