namespace DatesAndStuff.Mobile;

using System;
using Microsoft.Maui.Controls;

public partial class SimulationTimePage : ContentPage
{
    public SimulationTimePage()
    {
        this.InitializeComponent();

        this.TimeLabel.Text = $"Time: {new SimulationTime(DateTime.Now)}";
    }
}
