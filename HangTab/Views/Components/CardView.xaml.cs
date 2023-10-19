namespace HangTab.Views.Components;

public partial class CardView : ContentView
{
    public static readonly BindableProperty IsSubProperty = BindableProperty.Create(nameof(IsSub), typeof(bool), typeof(CardView), false);
    public static readonly BindableProperty BowlerNameProperty = BindableProperty.Create(nameof(BowlerName), typeof(string), typeof(CardView), string.Empty);
    private int count = 0;

    public bool IsSub
    {
        get => (bool)GetValue(IsSubProperty);
        set => SetValue(IsSubProperty, value);
    }

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