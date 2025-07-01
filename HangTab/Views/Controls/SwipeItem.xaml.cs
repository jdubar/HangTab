using Microsoft.Maui.Controls.Shapes;

namespace HangTab.Views.Controls;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a Behavior for the UI and does not require unit tests.")]
public partial class SwipeItem : ContentView
{
	public SwipeItem()
	{
		InitializeComponent();
	}

	public Color Color
	{
		get => (Color)GetValue(ColorProperty);
		set => SetValue(ColorProperty, value);
    }

    public string IconSource
	{
		get => (string)GetValue(IconSourceProperty);
		set => SetValue(IconSourceProperty, value);
    }

	public bool RoundCorners
	{
		get => (bool)GetValue(RoundCornersProperty);
		set => SetValue(RoundCornersProperty, value);
    }

    public string Text
	{
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
    }

	public static readonly BindableProperty ColorProperty =
		BindableProperty.Create(nameof(Color), typeof(Color), typeof(SwipeItem), defaultValue: default(Color));

    public static readonly BindableProperty IconSourceProperty =
		BindableProperty.Create(nameof(IconSource), typeof(string), typeof(SwipeItem), defaultValue: string.Empty);

	public static readonly BindableProperty RoundCornersProperty =
		BindableProperty.Create(nameof(RoundCorners), typeof(bool), typeof(SwipeItem), defaultValue: false,
		propertyChanged:(bindableObject, oldValue, newValue) =>
        {
			var swipeItem = (SwipeItem)bindableObject;
			if ((bool)newValue)
			{
				swipeItem.ButtonBorder.StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(5, 0, 5, 0) };
			}
        });

    public static readonly BindableProperty TextProperty =
		BindableProperty.Create(nameof(Text), typeof(string), typeof(SwipeItem), defaultValue: string.Empty);
}
