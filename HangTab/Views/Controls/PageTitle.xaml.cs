namespace HangTab.Views.Controls;

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