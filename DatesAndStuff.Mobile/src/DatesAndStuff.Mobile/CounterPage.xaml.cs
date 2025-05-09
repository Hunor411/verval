namespace DatesAndStuff.Mobile;

public partial class CounterPage : ContentPage
{
    private int count;

    public CounterPage() => this.InitializeComponent();

    private void OnCounterClicked(object sender, EventArgs e)
    {
        this.count++;

        this.CurrentCountLabel.Text = $"Current count: {this.count}";

        SemanticScreenReader.Announce(this.CurrentCountLabel.Text);
    }
}
