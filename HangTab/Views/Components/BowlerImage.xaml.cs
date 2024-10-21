namespace HangTab.Views.Components;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We won't test UI code-behind.")]
public partial class BowlerImage
{
    public static readonly BindableProperty ImageUrlProperty =
        BindableProperty.Create(nameof(ImageUrl), typeof(string), typeof(BowlerImage), string.Empty);
    public static readonly BindableProperty IsLowestHangsProperty =
        BindableProperty.Create(nameof(IsLowestHangs), typeof(bool), typeof(BowlerImage), false);
    public static readonly BindableProperty IsSubProperty =
        BindableProperty.Create(nameof(IsSub), typeof(bool), typeof(BowlerImage), false);
    public static readonly BindableProperty SizeProperty =
        BindableProperty.Create(nameof(Size), typeof(int), typeof(BowlerImage), 50);
    public static readonly BindableProperty PaddingValueProperty =
        BindableProperty.Create(nameof(PaddingValue), typeof(int), typeof(BowlerImage), -5);

    public string ImageUrl
    {
        get => (string)GetValue(ImageUrlProperty);
        set => SetValue(ImageUrlProperty, value);
    }

    public bool IsLowestHangs
    {
        get => (bool)GetValue(IsLowestHangsProperty);
        set => SetValue(IsLowestHangsProperty, value);
    }

    public bool IsSub
    {
        get => (bool)GetValue(IsSubProperty);
        set => SetValue(IsSubProperty, value);
    }

    public int Size
    {
        get => (int)GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    public int PaddingValue
    {
        get => (int)GetValue(PaddingValueProperty);
        set => SetValue(PaddingValueProperty, value);
    }

    public BowlerImage()
    {
        InitializeComponent();
    }
}