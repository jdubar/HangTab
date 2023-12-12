namespace HangTab.Views.Components;

public partial class BowlerImage : ContentView
{
    public static readonly BindableProperty ImageUrlProperty =
        BindableProperty.Create(nameof(ImageUrl), typeof(string), typeof(CardView), string.Empty);
    public static readonly BindableProperty IsSubProperty =
        BindableProperty.Create(nameof(IsSub), typeof(bool), typeof(CardView), false);

    public string ImageUrl
    {
        get => (string)GetValue(ImageUrlProperty);
        set => SetValue(ImageUrlProperty, value);
    }

    public bool IsSub
    {
        get => (bool)GetValue(IsSubProperty);
        set => SetValue(IsSubProperty, value);
    }

    public BowlerImage()
    {
        InitializeComponent();
    }
}