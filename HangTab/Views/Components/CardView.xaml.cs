namespace HangTab.Views.Components;
public partial class CardView
{
    public static readonly BindableProperty BowlerNameProperty =
        BindableProperty.Create(nameof(BowlerName), typeof(string), typeof(CardView), string.Empty);

    public static readonly BindableProperty ImageUrlProperty =
        BindableProperty.Create(nameof(ImageUrl), typeof(string), typeof(CardView), string.Empty);

    public static readonly BindableProperty IsHiddenProperty =
        BindableProperty.Create(nameof(IsHidden), typeof(bool), typeof(CardView), false);

    public static readonly BindableProperty IsSubProperty =
        BindableProperty.Create(nameof(IsSub), typeof(bool), typeof(CardView), false);

    public static readonly BindableProperty TotalHangingsProperty =
        BindableProperty.Create(nameof(TotalHangings), typeof(int), typeof(CardView), 0);

    public string BowlerName
    {
        get => (string)GetValue(BowlerNameProperty);
        set => SetValue(BowlerNameProperty, value);
    }

    public string ImageUrl
    {
        get => (string)GetValue(ImageUrlProperty);
        set => SetValue(ImageUrlProperty, value);
    }

    public bool IsHidden
    {
        get => (bool)GetValue(IsHiddenProperty);
        set => SetValue(IsHiddenProperty, value);
    }

    public bool IsSub
    {
        get => (bool)GetValue(IsSubProperty);
        set => SetValue(IsSubProperty, value);
    }

    public int TotalHangings
    {
        get => (int)GetValue(TotalHangingsProperty);
        set => SetValue(TotalHangingsProperty, value);
    }

    public CardView()
    {
        InitializeComponent();
    }
}