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
            Arguments = $"run --project \"{webProjectPath}\"",
            // Arguments = "dotnet run --no-build",
            WorkingDirectory = webProjFolderPath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };

        this.blazorProcess = Process.Start(startInfo)!;

        this.blazorProcess.OutputDataReceived += (s, e) => TestContext.Progress.WriteLine("[stdout] " + e.Data);
        this.blazorProcess.ErrorDataReceived += (s, e) => TestContext.Progress.WriteLine("[stderr] " + e.Data);
        this.blazorProcess.BeginOutputReadLine();
        this.blazorProcess.BeginErrorReadLine();

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
                    Console.WriteLine("Blazor server is up.");
                    return;
                }
            }
            catch
            {
                Thread.Sleep(1000);
            }
        }

        Assert.Fail($"Blazor server did not start at {BaseUrl} within {timeout.TotalSeconds} seconds.");
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
        var options = new ChromeOptions();
        options.AddArgument("--headless");
        this.driver = new ChromeDriver(options);
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
    private static string BaseUrl = "http://localhost:5091";
    private Process? blazorProcess;

    private const string PersonPageNavigationLocator = "//*[@data-test='PersonPageNavigation']";
    private const string DisplayedSalaryLocator = "//*[@data-test='DisplayedSalary']";
    private const string SalaryIncreasePercentageInputLocator = "//*[@data-test='SalaryIncreasePercentageInput']";
    private const string SalaryIncreaseSubmitButtonLocator = "//*[@data-test='SalaryIncreaseSubmitButton']";
    private const string SalaryIncreaseValidationMessageLocator = "//*[@data-test='SalaryIncreaseValidationMessage']";
    private const string SalaryIncreaseValidationMessage2Locator = "//*[@data-test='SalaryIncreaseValidationMessage2']";

    [TestCase(5)]
    [TestCase(10)]
    [TestCase(15)]
    [TestCase(20)]
    [TestCase(25)]
    public void PersonSalaryIncreaseShouldIncrease(double percentage)
    {
        this.driver.Navigate().GoToUrl(BaseUrl);

        var wait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(5));
        wait.Until(ExpectedConditions.ElementExists(By.XPath(PersonPageNavigationLocator))).Click();

        wait.Until(ExpectedConditions.ElementExists(By.XPath(DisplayedSalaryLocator)));
        var salaryBefore = double.Parse(
            this.driver.FindElement(By.XPath(DisplayedSalaryLocator)).Text,
            CultureInfo.InvariantCulture
        );

        wait.Until(ExpectedConditions.ElementExists(By.XPath(SalaryIncreasePercentageInputLocator)));
        var input = this.driver.FindElement(By.XPath(SalaryIncreasePercentageInputLocator));
        input.Clear();
        input.SendKeys(percentage.ToString(CultureInfo.CurrentCulture));

        wait.Until(ExpectedConditions.ElementExists(By.XPath(SalaryIncreaseSubmitButtonLocator)));
        this.driver.FindElement(By.XPath(SalaryIncreaseSubmitButtonLocator)).Click();

        wait.Until(ExpectedConditions.ElementExists(By.XPath(DisplayedSalaryLocator)));
        var salaryAfter = double.Parse(
            this.driver.FindElement(By.XPath(DisplayedSalaryLocator)).Text,
            CultureInfo.InvariantCulture
        );

        var expectedSalary = salaryBefore + (salaryBefore * (percentage / 100.0));
        salaryAfter.Should().BeApproximately(expectedSalary, 0.001);
    }

    [TestCase(-10)]
    [TestCase(-15)]
    [TestCase(-20)]
    [TestCase(-25)]
    [TestCase(-30)]
    public void PersonSalaryIncreaseSalaryTooLargeNegativeValueShouldFail(double percentage)
    {
        this.driver.Navigate().GoToUrl(BaseUrl);

        var wait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(5));
        wait.Until(ExpectedConditions.ElementExists(By.XPath(PersonPageNavigationLocator))).Click();

        wait.Until(ExpectedConditions.ElementExists(By.XPath(SalaryIncreasePercentageInputLocator)));
        var input = this.driver.FindElement(By.XPath(SalaryIncreasePercentageInputLocator));
        input.Clear();
        input.SendKeys(percentage.ToString(CultureInfo.CurrentCulture));

        wait.Until(ExpectedConditions.ElementExists(By.XPath(SalaryIncreaseSubmitButtonLocator)));
        this.driver.FindElement(By.XPath(SalaryIncreaseSubmitButtonLocator)).Click();

        wait.Until(ExpectedConditions.ElementExists(By.XPath(SalaryIncreaseValidationMessageLocator)));
        wait.Until(ExpectedConditions.ElementExists(By.XPath(SalaryIncreaseValidationMessage2Locator)));

        var salaryAfter = double.Parse(
            this.driver.FindElement(By.XPath(DisplayedSalaryLocator)).Text,
            CultureInfo.InvariantCulture
        );
        salaryAfter.Should().Be(5000);
    }
}
