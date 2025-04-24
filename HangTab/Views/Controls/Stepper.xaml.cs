namespace HangTab.Views.Controls;

public partial class Stepper : ContentView
{
	public Stepper()
	{
		InitializeComponent();
        DecreaseButton.IsEnabled = false;
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
            stepper.DecreaseButton.IsEnabled = (int)newValue > stepper.Minimum;
            stepper.IncreaseButton.IsEnabled = (int)newValue < stepper.Maximum;
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
            stepper.DecreaseButton.IsEnabled = stepper.Value > stepper.Minimum && (bool)newValue;
            stepper.IncreaseButton.IsEnabled = stepper.Value < stepper.Maximum && (bool)newValue;
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
}