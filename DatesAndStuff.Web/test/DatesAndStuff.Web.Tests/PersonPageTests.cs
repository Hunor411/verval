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

        this._blazorProcess = Process.Start(startInfo);

        // Wait for the app to become available
        var client = new HttpClient();
        var timeout = TimeSpan.FromSeconds(30);
        var start = DateTime.Now;

        while (DateTime.Now - start < timeout)
        {
            try
            {
                var result = client.GetAsync(BaseURL).Result;
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
        if (this._blazorProcess != null && !this._blazorProcess.HasExited)
        {
            this._blazorProcess.Kill(true);
            this._blazorProcess.Dispose();
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
    private const string BaseURL = "http://localhost:5091";
    private bool acceptNextAlert = true;

    private Process? _blazorProcess;

    [Test]
    public void PersonSalaryIncreaseShouldIncrease()
    {
        // Arrange
        this.driver.Navigate().GoToUrl(BaseURL);
        this.driver.FindElement(By.XPath("//*[@data-test='PersonPageNavigation']")).Click();

        var wait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(5));

        var input = wait.Until(
            ExpectedConditions.ElementExists(By.XPath("//*[@data-test='SalaryIncreasePercentageInput']")));
        input.Clear();
        input.SendKeys("5");

        // Act
        var submitButton =
            wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@data-test='SalaryIncreaseSubmitButton']")));
        submitButton.Click();


        // Assert
        var salaryLabel =
            wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@data-test='DisplayedSalary']")));
        var salaryAfterSubmission = double.Parse(salaryLabel.Text, CultureInfo.InvariantCulture);
        salaryAfterSubmission.Should().BeApproximately(5250, 0.001);
    }
}
