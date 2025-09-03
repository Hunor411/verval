namespace DatesAndStuff.BehaviourTests.StepDefinitions;

[Binding]
public sealed class CalculatorStepDefinitions
{
    private int operand1;
    private int operand2;

    private int result;
    // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

    [Given("the first number is (.*)")]
    public void GivenTheFirstNumberIs(int number) => this.operand1 = number;

    [Given("the second number is (.*)")]
    public void GivenTheSecondNumberIs(int number) => this.operand2 = number;

    [When("the two numbers are added")]
    public void WhenTheTwoNumbersAreAdded() => this.result = this.operand1 + this.operand2;

    [Then("the result should be (.*)")]
    public void ThenTheResultShouldBe(int expectedResult) => this.result.Should().Be(expectedResult);
}
