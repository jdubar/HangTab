using CommunityToolkit.Mvvm.Input;

namespace HangTab.Views.Components;

public partial class CardView : ContentView
{
    public static readonly BindableProperty BowlerNameProperty =
        BindableProperty.Create(nameof(BowlerName), typeof(string), typeof(CardView), string.Empty);
    public static readonly BindableProperty ImageUrlProperty =
        BindableProperty.Create(nameof(ImageUrl), typeof(string), typeof(CardView), string.Empty);
    public static readonly BindableProperty IsSubProperty =
        BindableProperty.Create(nameof(IsSub), typeof(bool), typeof(CardView), false);
    public static readonly BindableProperty TotalHangingsProperty =
        BindableProperty.Create(nameof(TotalHangings), typeof(int), typeof(CardView), 0);

    public static readonly BindableProperty HangBowlerCommandProperty =
        BindableProperty.Create(nameof(HangBowlerCommand), typeof(RelayCommand), typeof(CardView));

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

    public RelayCommand HangBowlerCommand
    {
        get => (RelayCommand)GetValue(HangBowlerCommandProperty);
        set => SetValue(HangBowlerCommandProperty, value);
    }

    public CardView()
    {
        InitializeComponent();
    }
}