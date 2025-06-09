namespace HangTab.Views.Controls;

public partial class LoadingIndicator : ContentView
{
	public LoadingIndicator()
	{
		InitializeComponent();
    }

	public bool ShowLoadingIndicator
    {
        get => (bool)GetValue(ShowLoadingIndicatorProperty);
        set => SetValue(ShowLoadingIndicatorProperty, value);
    }

    public string LoadingText
    {
        get => (string)GetValue(LoadingTextProperty);
        set => SetValue(LoadingTextProperty, value);
    }

    public static readonly BindableProperty ShowLoadingIndicatorProperty =
        BindableProperty.Create(nameof(ShowLoadingIndicator), typeof(bool), typeof(LoadingIndicator), defaultValue: false, defaultBindingMode: BindingMode.TwoWay,
        propertyChanged: (bindableObject, oldValue, newValue) =>
        {
            var loadingIndicator = (LoadingIndicator)bindableObject;
            loadingIndicator.LoadingView.IsVisible = (bool)newValue;
        });

    public static readonly BindableProperty LoadingTextProperty =
        BindableProperty.Create(nameof(LoadingText), typeof(string), typeof(LoadingIndicator), defaultValue: "Loading...");
}