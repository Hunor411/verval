namespace DatesAndStuff.Mobile;

using Microsoft.Maui.Controls;

public partial class PersonPage : ContentPage
{
    public PersonPage()
    {
        this.InitializeComponent();

        this.BindingContext = new PersonViewModel();
    }
}
