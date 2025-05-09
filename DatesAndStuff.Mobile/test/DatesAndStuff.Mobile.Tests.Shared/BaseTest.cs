namespace DatesAndStuff.Mobile.Tests;

using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

public abstract class BaseTest
{
    protected static AppiumDriver App => AppiumSetup.App;

    // This could also be an extension method to AppiumDriver if you prefer
    protected static AppiumElement FindUIElement(string id)
    {
        if (App is WindowsDriver)
        {
            return App.FindElement(MobileBy.AccessibilityId(id));
        }

        return App.FindElement(MobileBy.Id(id));
    }
}
