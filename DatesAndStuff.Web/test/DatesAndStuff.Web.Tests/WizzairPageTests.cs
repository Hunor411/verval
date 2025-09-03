namespace DatesAndStuff.Web.Tests;

using System.Diagnostics;
using System.Text;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

[TestFixture]
public class WizzairPageTests
{
    private const string WizzairUrl = "https://wizzair.com";
    private ChromeDriver driver;
    private StringBuilder verificationErrors;

    [SetUp]
    public void SetupTest()
    {
        var options = new ChromeOptions();
        options.AddArgument("--disable-geolocation");
        options.AddUserProfilePreference("profile.default_content_setting_values.geolocation", 2);
        this.driver = new ChromeDriver(options);
        this.verificationErrors = new StringBuilder();
    }

    [TearDown]
    [DebuggerStepThrough]
    public void TeardownTest()
    {
        try
        {
            this.driver.Quit();
            this.driver.Dispose();
        }
        catch (Exception)
        {
            // Ignore errors if unable to close the browser
        }

        Assert.That(this.verificationErrors.ToString(), Is.EqualTo(""));
    }

    private const string CookiePolicyButtonLocation = "//*[@id=\"onetrust-accept-btn-handler\"]";
    private const string OneWayInputLocation = "//*[@id=\"radio-button-id-5\"]";
    private const string OriginInputLocation = "//*[@id=\"wa-autocomplete-input-7\"]";
    private const string DestinationInputLocation = "//*[@id=\"wa-autocomplete-input-9\"]";
    private const string DateInputLocation =
        "//*[@id=\"app\"]/div/main/div/div/div[1]/div[1]/div[1]/div[2]/div/div[2]/div/div[1]/form/div/fieldset[2]/div/div[1]/div/input";

    [Test]
    public void ShouldHaveAtLeastTwoFlightsBetweenVasarhelyAndBudapestNextWeek()
    {
        this.driver.Navigate().GoToUrl(WizzairUrl);

        var wait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(5));
        wait.Until(ExpectedConditions.ElementExists(By.XPath(CookiePolicyButtonLocation))).Click();

        wait.Until(ExpectedConditions.ElementExists(By.XPath(OneWayInputLocation))).Click();

        var originInput = wait.Until(ExpectedConditions.ElementExists(By.XPath(OriginInputLocation)));
        originInput.Clear();
        originInput.SendKeys("TGM");
        wait.Until(ExpectedConditions.ElementExists(By.XPath("/html/body/div[3]/div/div/div/div/div/label/strong")));
        this.driver.FindElement(By.XPath("/html/body/div[3]/div/div/div/div/div/label/strong")).Click();

        var destinationInput = wait.Until(ExpectedConditions.ElementExists(By.XPath(DestinationInputLocation)));
        destinationInput.Clear();
        destinationInput.SendKeys("BUD");
        wait.Until(ExpectedConditions.ElementExists(By.XPath("/html/body/div[4]/div/div/div/div/div/label/strong")));
        this.driver.FindElement(By.XPath("/html/body/div[4]/div/div/div/div/div/label/strong")).Click();

        var dateInput = wait.Until(ExpectedConditions.ElementExists(By.XPath(DateInputLocation)));
        dateInput.Click();

        var today = DateTime.Today;
        var dayUntilNextMonday = (DayOfWeek.Monday - today.DayOfWeek + 7) % 7;
        var nextWeekStart = today.AddDays(dayUntilNextMonday);
        var nextWeekEnd = nextWeekStart.AddDays(6);

        var days = this.driver.FindElements(
            By.XPath("//span[contains(@class, 'vc-day-content') and not(contains(@class, 'is-disabled'))]"));

        var nextWeekDates = days
            .Where(e => int.TryParse(e.Text, out var day) && day >= nextWeekStart.Day && day <= nextWeekEnd.Day)
            .ToList();

        nextWeekDates.Count.Should().BeGreaterOrEqualTo(2,
            "there should be at least two available departure dates between TGM and BUD for next week");
    }
}
