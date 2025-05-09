namespace DatesAndStuff.Mobile;

using System.Diagnostics.CodeAnalysis;
using Foundation;

[Register("AppDelegate")]
[SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix",
    Justification = "AppDelegate is required for MAUI iOS app entry point.")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
