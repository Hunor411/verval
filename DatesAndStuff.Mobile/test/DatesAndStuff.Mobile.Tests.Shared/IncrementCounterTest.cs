namespace DatesAndStuff.Mobile.Tests;

using FluentAssertions;
using OpenQA.Selenium;

internal sealed class IncrementCounterTest : BaseTest
{
    [OneTimeSetUp]
    public void InitOnce() => AppiumSetup.RunBeforeAnyTests();

    [OneTimeTearDown]
    public void CleanupOnce() => AppiumSetup.RunBeforeAnyTests();

    [Test]
    public void ClickCounterTest()
    {
        // Arrange

        // navigate to the counter page
        var drawer =
            App.FindElement(By.XPath("//android.widget.ImageButton[@content-desc=\"Open navigation drawer\"]"));
        drawer.Click();
        var counterMenu = App.FindElement(By.XPath("//android.widget.TextView[@text=\"Counter\"]"));
        counterMenu.Click();

        // check the current count
        var currentCountTextView = FindUiElement("CounterNumberLabel");
        var originalCount = 0;
        var currentCountValue = currentCountTextView.Text.Substring(currentCountTextView.Text.IndexOf(':') + 1);
        if (!int.TryParse(currentCountValue, out originalCount))
        {
            Assert.Fail($"Failed to parse current count value: '{currentCountValue}'");
        }

        var buttonToClick = FindUiElement("CounterIncreaseBtn");

        // Act
        buttonToClick.Click();
        //Task.Delay(500).Wait(); // Wait for the click to register and show up on the screenshot

        // Assert
        currentCountValue = currentCountTextView.Text.Substring(currentCountTextView.Text.IndexOf(':') + 1);
        var updatedCount = 0;
        if (!int.TryParse(currentCountValue, out updatedCount))
        {
            Assert.Fail($"Failed to parse current count value: '{updatedCount}'");
        }

        // Assert
        updatedCount.Should().Be(originalCount + 1);
    }
}
