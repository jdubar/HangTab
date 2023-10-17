namespace HangTab.Views.Components;

public partial class CardView : ContentView
{
    public static readonly BindableProperty BowlerNameProperty = BindableProperty.Create(nameof(BowlerName), typeof(string), typeof(CardView), string.Empty);
    private int count = 0;

    public string BowlerName
    {
        get => (string)GetValue(BowlerNameProperty);
        set => SetValue(BowlerNameProperty, value);
    }

    public CardView()
    {
        InitializeComponent();
    }

    private void OnHangBowlerClicked(object sender, EventArgs e)
    {
        count++;
        HangCount.Text = $"Hangings: {count}";
    }
}