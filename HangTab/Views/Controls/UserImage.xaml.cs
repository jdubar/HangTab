namespace HangTab.Views.Controls;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a Behavior for the UI and does not require unit tests.")]
public partial class UserImage : ContentView
{
	public UserImage()
	{
		InitializeComponent();
	}

    public bool ShowCrown
    {
        get => (bool)GetValue(ShowCrownProperty);
        set => SetValue(ShowCrownProperty, value);
    }

    public bool ShowBlind
    {
        get => (bool)GetValue(ShowBlindProperty);
        set => SetValue(ShowBlindProperty, value);
    }

    public bool ShowSub
    {
        get => (bool)GetValue(ShowSubProperty);
        set => SetValue(ShowSubProperty, value);
    }

    public string ImageUrl
    {
        get => (string)GetValue(ImageUrlProperty);
        set => SetValue(ImageUrlProperty, value);
    }

    public string Initials
    {
        get => (string)GetValue(InitialsProperty);
        set => SetValue(InitialsProperty, value);
    }

    public static readonly BindableProperty ShowCrownProperty =
        BindableProperty.Create(nameof(ShowCrown), typeof(bool), typeof(UserImage), defaultValue: false);

    public static readonly BindableProperty ShowBlindProperty =
        BindableProperty.Create(nameof(ShowBlind), typeof(bool), typeof(UserImage), defaultValue: false);

    public static readonly BindableProperty ShowSubProperty =
        BindableProperty.Create(nameof(ShowSub), typeof(bool), typeof(UserImage), defaultValue: false);

    public static readonly BindableProperty ImageUrlProperty =
        BindableProperty.Create(nameof(ImageUrl), typeof(string), typeof(UserImage), defaultValue: string.Empty);

    public static readonly BindableProperty InitialsProperty =
        BindableProperty.Create(nameof(Initials), typeof(string), typeof(UserImage), defaultValue: string.Empty);
}