namespace DatesAndStuff.Mobile.Tests;

using System.Globalization;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

[TestFixture]
public class PersonMobilePageTests : BaseTest
{
    [OneTimeSetUp]
    public void InitOnce() => AppiumSetup.RunBeforeAnyTests();

    [OneTimeTearDown]
    public void CleanupOnce() => AppiumSetup.RunBeforeAnyTests();

    private const string NavDrawerButtonXPath =
        "//android.widget.ImageButton[@content-desc=\"Open navigation drawer\"]";

    private const string PersonMenuItemXPath = "//android.widget.TextView[@text=\"Person\"]";
    private const string SalaryTextId = "PersonSalaryText";
    private const string SalaryInputId = "PersonSalaryInput";
    private const string SubmitButtonId = "PersonSalaryInputSubmit";
    private const string SalaryIncreaseErrorMessageId = "PersonSalaryValidationMessage";

    [TestCase(5)]
    [TestCase(10)]
    [TestCase(15)]
    [TestCase(20)]
    [TestCase(25)]
    public void PersonSalaryIncreaseShouldIncreaseSalary(double percentage)
    {
        App.FindElement(By.XPath(NavDrawerButtonXPath)).Click();
        App.FindElement(By.XPath(PersonMenuItemXPath)).Click();

        var wait = new WebDriverWait(App, TimeSpan.FromSeconds(5));
        var originalSalaryElement = wait.Until(_ => FindUiElement(SalaryTextId));
        var originalSalary = double.Parse(originalSalaryElement.Text, CultureInfo.InvariantCulture);

        var personSalaryInput = wait.Until(_ => FindUiElement(SalaryInputId));
        personSalaryInput.Clear();
        personSalaryInput.SendKeys(percentage.ToString(CultureInfo.InvariantCulture));

        FindUiElement(SubmitButtonId).Click();

        var newSalaryElement = wait.Until(_ => FindUiElement(SalaryTextId));
        var newSalary = double.Parse(newSalaryElement.Text, CultureInfo.InvariantCulture);

        var expectedSalary = originalSalary * (1 + (percentage / 100));
        newSalary.Should().BeApproximately(expectedSalary, 0.01);
    }

    [TestCase(-1)]
    [TestCase(-3)]
    [TestCase(-5)]
    [TestCase(-8)]
    [TestCase(-9)]
    public void PersonSalaryDecreaseShouldDecreaseSalary(double percentage)
    {
        App.FindElement(By.XPath(NavDrawerButtonXPath)).Click();
        App.FindElement(By.XPath(PersonMenuItemXPath)).Click();

        var wait = new WebDriverWait(App, TimeSpan.FromSeconds(5));
        var originalSalaryElement = wait.Until(_ => FindUiElement(SalaryTextId));
        var originalSalary = double.Parse(originalSalaryElement.Text, CultureInfo.InvariantCulture);

        var personSalaryInput = wait.Until(_ => FindUiElement(SalaryInputId));
        personSalaryInput.Clear();
        personSalaryInput.SendKeys(percentage.ToString(CultureInfo.InvariantCulture));

        FindUiElement(SubmitButtonId).Click();

        var newSalaryElement = wait.Until(_ => FindUiElement(SalaryTextId));
        var newSalary = double.Parse(newSalaryElement.Text, CultureInfo.InvariantCulture);

        var expectedSalary = originalSalary * (1 + (percentage / 100));
        newSalary.Should().BeApproximately(expectedSalary, 0.01);
    }

    [TestCase(-10)]
    [TestCase(-15)]
    [TestCase(-20)]
    [TestCase(-25)]
    [TestCase(-30)]
    public void PersonSalaryDecreaseWithTooLargeNegativeValueShouldShowValidationError(double percentage)
    {
        App.FindElement(By.XPath(NavDrawerButtonXPath)).Click();
        App.FindElement(By.XPath(PersonMenuItemXPath)).Click();

        var wait = new WebDriverWait(App, TimeSpan.FromSeconds(5));
        var originalSalaryElement = wait.Until(_ => FindUiElement(SalaryTextId));
        var originalSalary = double.Parse(originalSalaryElement.Text, CultureInfo.InvariantCulture);

        var personSalaryInput = wait.Until(_ => FindUiElement(SalaryInputId));
        personSalaryInput.Clear();
        personSalaryInput.SendKeys(percentage.ToString(CultureInfo.InvariantCulture));

        FindUiElement(SubmitButtonId).Click();
        var errorMessageElement = wait.Until(_ => FindUiElement(SalaryIncreaseErrorMessageId));
        errorMessageElement.Displayed.Should().BeTrue();

        var newSalaryElement = wait.Until(_ => FindUiElement(SalaryTextId));
        var newSalary = double.Parse(newSalaryElement.Text, CultureInfo.InvariantCulture);
        newSalary.Should().Be(originalSalary);
    }
}
