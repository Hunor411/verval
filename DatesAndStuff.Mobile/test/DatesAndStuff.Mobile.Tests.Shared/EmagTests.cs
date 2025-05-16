namespace DatesAndStuff.Mobile.Tests;

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Support.UI;

[TestFixture]
public class EmagTests
{
    private AppiumDriver emagApp;

    [OneTimeSetUp]
    public void SetUp()
    {
        var options = new AppiumOptions
        {
            PlatformName = "Android",
            AutomationName = "UIAutomator2"
        };

        options.AddAdditionalAppiumOption(MobileCapabilityType.NoReset, false);

        options.AddAdditionalAppiumOption(AndroidMobileCapabilityType.AppPackage, "ro.emag.android");
        options.AddAdditionalAppiumOption(AndroidMobileCapabilityType.AppActivity, "ro.emag.android.cleancode.app.ActivityStart");

        this.emagApp = new AndroidDriver(options);
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        this.emagApp.Quit();
        this.emagApp.Dispose();

        AppiumServerHelper.DisposeAppiumLocalServer();
    }

    private const string AcceptCookieButton =
        "//android.widget.TextView[@resource-id=\"ro.emag.android:id/btnRejectAll\"]";

    private const string AcceptYear = "//android.widget.Button[@resource-id=\"ro.emag.android:id/btn_dialog_over_16\"]";

    private const string FakeSearchInput =
        "//android.widget.TextView[@resource-id=\"ro.emag.android:id/tvCategoriesSearch\"]";

    private const string SearchInput = "//android.widget.EditText[@resource-id=\"ro.emag.android:id/etSearchQuery\"]";

    private const string FirstResult =
        "//android.widget.TextView[@resource-id=\"ro.emag.android:id/tvSearchSuggestion\" and @text=\"apple macbook pro\"]";

    private const string FirstItem = "(//android.widget.ImageView[@content-desc=\"Adaugă în coș\"])[1]";

    private const string CartNavButton =
        "(//android.widget.ImageView[@resource-id=\"ro.emag.android:id/navigation_bar_item_icon_view\"])[3]";

    private const string CartContinueBtn =
        "//android.widget.Button[@resource-id=\"ro.emag.android:id/btnCartContinue\"]";

    private const string SkipTutorial = "//android.view.View[@resource-id=\"ro.emag.android:id/clippingView\"]";

    private const string VideoFilePath = "/Users/deakhunor/Desktop/Huni/Egyetem/verval/labor/emag_test_video.mp4";

    private void TryClickIfExists(WebDriverWait wait, By by)
    {
        try
        {
            var element = wait.Until(_ => this.emagApp.FindElement(by));
            element.Click();
        }
        catch (WebDriverTimeoutException)
        {
            // Element not found within timeout — ignore
        }
    }

    [Test]
    public void ShouldAddProductToCartSuccessfully()
    {
        var wait = new WebDriverWait(this.emagApp, TimeSpan.FromSeconds(10));

        this.emagApp.StartRecordingScreen();

        try
        {
            this.TryClickIfExists(wait, By.XPath(SkipTutorial));
            this.TryClickIfExists(wait, By.XPath(AcceptCookieButton));
            this.TryClickIfExists(wait, By.XPath(AcceptYear));

            var fakeSearchInput = wait.Until(_ =>
                this.emagApp.FindElement(By.XPath(FakeSearchInput)));
            fakeSearchInput.Click();

            var searchInput = wait.Until(_ =>
                this.emagApp.FindElement(By.XPath(SearchInput)));
            searchInput.Clear();
            searchInput.SendKeys("Apple macbook pro");

            wait.Until(_ => this.emagApp.FindElement(By.XPath(FirstResult))).Click();
            wait.Until(_ => this.emagApp.FindElement(By.XPath(FirstItem))).Click();
            this.emagApp.FindElement(By.XPath(CartNavButton)).Click();
            wait.Until(_ => this.emagApp.FindElement(By.XPath(CartContinueBtn))).Click();
        }
        finally
        {
            var base64Video = this.emagApp.StopRecordingScreen();
            File.WriteAllBytes(VideoFilePath, Convert.FromBase64String(base64Video));
        }
    }
}
