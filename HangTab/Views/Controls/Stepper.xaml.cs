namespace HangTab.Views.Controls;

public partial class Stepper : ContentView
{
    // TODO: Set these 2 static values earlier, like in App.xaml.cs - create a utility class perhaps
    private static Color PrimaryTextColor => Application.Current?.Resources["TextPrimaryColor"] as Color ?? Colors.Black;
    private static Color DisabledTextColor => Application.Current?.Resources["ControlDisabledTextColor"] as Color ?? Colors.Gray;

    public Stepper()
	{
		InitializeComponent();
        SetDecreaseButtonState(this, false);
    }

    public int Value
    {
        get => (int)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }
    
    public int Minimum
    {
        get => (int)GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }
    
    public int Maximum
    {
        get => (int)GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }
    
    public bool IsControlEnabled
    {
        get => (bool)GetValue(IsControlEnabledProperty);
        set => SetValue(IsControlEnabledProperty, value);
    }

    public static readonly BindableProperty ValueProperty = BindableProperty.Create(
        nameof(Value),
        typeof(int),
        typeof(Stepper),
        defaultValue: 0,
        BindingMode.TwoWay,
        propertyChanged: (bindableObject, oldValue, newValue) =>
        {
            var stepper = (Stepper)bindableObject;
            SetDecreaseButtonState(stepper, (int)newValue > stepper.Minimum);
            SetIncreaseButtonState(stepper, (int)newValue < stepper.Maximum);
        }
    );

    public static readonly BindableProperty MinimumProperty = BindableProperty.Create(
        nameof(Minimum),
        typeof(int),
        typeof(Stepper),
        defaultValue: 0);

    public static readonly BindableProperty MaximumProperty = BindableProperty.Create(
        nameof(Maximum),
        typeof(int),
        typeof(Stepper),
        defaultValue: 100);

    public static readonly BindableProperty IsControlEnabledProperty = BindableProperty.Create(
        nameof(IsControlEnabled),
        typeof(bool),
        typeof(Stepper),
        defaultValue: true,
        propertyChanged: (bindableObject, oldValue, newValue) =>
        {
            var stepper = (Stepper)bindableObject;
            SetDecreaseButtonState(stepper, stepper.Value > stepper.Minimum && (bool)newValue);
            SetIncreaseButtonState(stepper, stepper.Value < stepper.Maximum && (bool)newValue);
        });

    private void OnMinusButtonClicked(object sender, EventArgs e)
    {
        if (Value > Minimum)
        {
            Value--;
        }
    }

    private void OnPlusButtonClicked(object sender, EventArgs e)
    {
        if (Value >= Maximum)
        {
            return;
        }

        Value++;
    }

    private static void SetDecreaseButtonState(Stepper stepper, bool isEnabled)
    {
        stepper.DecreaseButton.IsEnabled = isEnabled;
        if (stepper.DecreaseButton.Source is FontImageSource buttonImage)
        {
            buttonImage.Color = isEnabled ? PrimaryTextColor : DisabledTextColor;
        }
    }

    private static void SetIncreaseButtonState(Stepper stepper, bool isEnabled)
    {
        stepper.IncreaseButton.IsEnabled = isEnabled;
        if (stepper.IncreaseButton.Source is FontImageSource buttonImage)
        {
            buttonImage.Color = isEnabled ? PrimaryTextColor : DisabledTextColor;
        }
    }
}