namespace HangTab.Views.Controls;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a Behavior for the UI and does not require unit tests.")]
public partial class PageTitle : ContentView
{
	public PageTitle()
	{
		InitializeComponent();
	}

	public string Title
	{
		get => (string)GetValue(TitleProperty);
		set => SetValue(TitleProperty, value);
    }

    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(UserImage), defaultValue: string.Empty);
}