namespace DatesAndStuff.Mobile;

public partial class SimulationTimePage : ContentPage
{
    public SimulationTimePage()
    {
        this.InitializeComponent();

        this.TimeLabel.Text = $"Time: {new SimulationTime(DateTime.Now)}";
    }
}
