namespace DatesAndStuff.Mobile;

using System;
using Microsoft.Maui.Accessibility;
using Microsoft.Maui.Controls;

public partial class CounterPage : ContentPage
{
    private int count;

    public CounterPage()
    {
        this.InitializeComponent();
        this.CurrentCountLabel.Text = $"Current count: {this.count}";
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        this.count++;

        this.CurrentCountLabel.Text = $"Current count: {this.count}";

        SemanticScreenReader.Announce(this.CurrentCountLabel.Text);
    }
}
