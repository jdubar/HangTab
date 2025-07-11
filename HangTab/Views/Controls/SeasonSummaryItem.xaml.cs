namespace HangTab.Views.Controls;

public partial class SeasonSummaryItem : ContentView
{
	public SeasonSummaryItem()
	{
		InitializeComponent();
	}

	public string Title
	{
		get => (string)GetValue(TitleProperty);
		set => SetValue(TitleProperty, value);
    }

	public string ImageSource
	{
		get => (string)GetValue(ImageSourceProperty);
		set => SetValue(ImageSourceProperty, value);
    }
	
	public int WeekNumber
	{
		get => (int)GetValue(WeekNumberProperty);
		set => SetValue(WeekNumberProperty, value);
    }
	
	public string ItemCount
	{
		get => (string)GetValue(ItemCountProperty);
		set => SetValue(ItemCountProperty, value);
    }

	public static readonly BindableProperty TitleProperty = 
		BindableProperty.Create(nameof(Title), typeof(string), typeof(SeasonSummaryItem), string.Empty);

	public static readonly BindableProperty ImageSourceProperty =
		BindableProperty.Create(nameof(ImageSource), typeof(string), typeof(SeasonSummaryItem), string.Empty);

	public static readonly BindableProperty WeekNumberProperty =
		BindableProperty.Create(nameof(WeekNumber), typeof(int), typeof(SeasonSummaryItem), 0);

	public static readonly BindableProperty ItemCountProperty =
		BindableProperty.Create(nameof(ItemCount), typeof(string), typeof(SeasonSummaryItem), string.Empty);
}