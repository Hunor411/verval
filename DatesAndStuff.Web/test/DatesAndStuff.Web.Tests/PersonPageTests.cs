namespace DatesAndStuff.Web.Tests;

using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

[TestFixture]
public class PersonPageTests
{
    [OneTimeSetUp]
    public void StartBlazorServer()
    {
        var webProjectPath = Path.GetFullPath(Path.Combine(
            Assembly.GetExecutingAssembly().Location,
            "../../../../../../src/DatesAndStuff.Web/DatesAndStuff.Web.csproj"
        ));

        var webProjFolderPath = Path.GetDirectoryName(webProjectPath);

        var startInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            //Arguments = $"run --project \"{webProjectPath}\"",
            Arguments = "dotnet run --no-build",
            WorkingDirectory = webProjFolderPath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };

        this.blazorProcess = Process.Start(startInfo);

        // Wait for the app to become available
        var client = new HttpClient();
        var timeout = TimeSpan.FromSeconds(30);
        var start = DateTime.Now;

        while (DateTime.Now - start < timeout)
        {
            try
            {
                var result = client.GetAsync(BaseUrl).Result;
                if (result.IsSuccessStatusCode)
                {
                    break;
                }
            }
            catch (Exception e)
            {
                Thread.Sleep(1000);
            }
        }
    }

    [OneTimeTearDown]
    public void StopBlazorServer()
    {
        if (this.blazorProcess != null && !this.blazorProcess.HasExited)
        {
            this.blazorProcess.Kill(true);
            this.blazorProcess.Dispose();
        }
    }

    [SetUp]
    public void SetupTest()
    {
        this.driver = new ChromeDriver();
        this.verificationErrors = new StringBuilder();
    }

    [TearDown]
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

    private ChromeDriver driver;
    private StringBuilder verificationErrors;
    private const string BaseUrl = "http://localhost:5091";
    private Process? blazorProcess;

    [TestCase(5)]
    [TestCase(10)]
    [TestCase(15)]
    [TestCase(20)]
    [TestCase(25)]
    public void PersonSalaryIncreaseShouldIncrease(double percentage)
    {
        // Arrange
        this.driver.Navigate().GoToUrl(BaseUrl);

        var wait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(5));
        wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@data-test='PersonPageNavigation']"))).Click();

        wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@data-test='DisplayedSalary']")));
        var salaryBefore = double.Parse(
            this.driver.FindElement(By.XPath("//*[@data-test='DisplayedSalary']")).Text,
            CultureInfo.InvariantCulture
        );

        wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@data-test='SalaryIncreasePercentageInput']")));
        var input = this.driver.FindElement(By.XPath("//*[@data-test='SalaryIncreasePercentageInput']"));
        input.Clear();
        input.SendKeys(percentage.ToString(CultureInfo.CurrentCulture));

        // Act
        wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@data-test='SalaryIncreaseSubmitButton']")));
        var submitButton = this.driver.FindElement(By.XPath("//*[@data-test='SalaryIncreaseSubmitButton']"));
        submitButton.Click();

        // Assert
        wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@data-test='DisplayedSalary']")));
        var salaryAfter = double.Parse(
            this.driver.FindElement(By.XPath("//*[@data-test='DisplayedSalary']")).Text,
            CultureInfo.InvariantCulture
        );

        var expectedSalary = salaryBefore + (salaryBefore * (percentage / 100.0));
        salaryAfter.Should().BeApproximately(expectedSalary, 0.001);
    }
}
