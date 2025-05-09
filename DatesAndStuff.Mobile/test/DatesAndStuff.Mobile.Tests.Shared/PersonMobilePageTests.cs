namespace DatesAndStuff.Mobile.Tests;

using System.Globalization;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

[TestFixture]
public class PersonMobilePageTests : BaseTest
{
    private const string NavDrawerButtonXPath = "//android.widget.ImageButton[@content-desc=\"Open navigation drawer\"]";
    private const string PersonMenuItemXPath = "//android.widget.TextView[@text=\"Person\"]";
    private const string SalaryTextId = "PersonSalaryText";
    private const string SalaryInputId = "PersonSalaryInput";
    private const string SubmitButtonId = "PersonSalaryInputSubmit";

    [TestCase(5)]
    [TestCase(10)]
    [TestCase(15)]
    [TestCase(20)]
    [TestCase(25)]
    [Test]
    public void PersonSalaryIncreaseShouldIncrease(double percentage)
    {
        App.FindElement(By.XPath(NavDrawerButtonXPath)).Click();
        App.FindElement(By.XPath(PersonMenuItemXPath)).Click();

        var wait = new WebDriverWait(App, TimeSpan.FromSeconds(5));
        var originalSalaryElement = wait.Until(_ => FindUIElement(SalaryTextId));
        var originalSalary = double.Parse(originalSalaryElement.Text, CultureInfo.InvariantCulture);

        var personSalaryInput = wait.Until(_ => FindUIElement(SalaryInputId));
        personSalaryInput.Clear();
        personSalaryInput.SendKeys(percentage.ToString(CultureInfo.InvariantCulture));

        FindUIElement(SubmitButtonId).Click();

        var newSalaryElement = wait.Until(_ => FindUIElement(SalaryTextId));
        var newSalary = double.Parse(newSalaryElement.Text, CultureInfo.InvariantCulture);

        var expectedSalary = originalSalary * (1 + (percentage / 100));
        newSalary.Should().BeApproximately(expectedSalary, 0.01);
    }
}
