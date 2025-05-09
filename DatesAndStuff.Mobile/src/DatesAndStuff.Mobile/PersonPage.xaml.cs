namespace DatesAndStuff.Mobile;

public partial class PersonPage : ContentPage
{
    public PersonPage()
    {
        this.InitializeComponent();

        this.BindingContext = new PersonViewModel();
    }
}
