namespace DatesAndStuff.Mobile;

using Microsoft.Maui.Controls;

public partial class App : Application
{
    public App()
    {
        this.InitializeComponent();

        this.MainPage = new AppShell();
    }
}
