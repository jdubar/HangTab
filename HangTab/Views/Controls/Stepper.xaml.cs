using HangTab.Utilities;

namespace HangTab.Views.Controls;

public partial class Stepper : ContentView
{
    public Stepper()
	{
		InitializeComponent();
        SetDecreaseButtonState(false);
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

    public static readonly BindableProperty ValueProperty =
        BindableProperty.Create(nameof(Value), typeof(int), typeof(Stepper), defaultValue: 0, BindingMode.TwoWay,
        propertyChanged: (bindableObject, oldValue, newValue) =>
        {
            var stepper = (Stepper)bindableObject;
            if (stepper.IsControlEnabled)
            {
                stepper.SetDecreaseButtonState((int)newValue > stepper.Minimum);
                stepper.SetIncreaseButtonState((int)newValue < stepper.Maximum);
            }
        });

    public static readonly BindableProperty MinimumProperty =
        BindableProperty.Create(nameof(Minimum), typeof(int), typeof(Stepper), defaultValue: 0);

    public static readonly BindableProperty MaximumProperty =
        BindableProperty.Create(nameof(Maximum), typeof(int), typeof(Stepper), defaultValue: 100);

    public static readonly BindableProperty IsControlEnabledProperty =
        BindableProperty.Create(nameof(IsControlEnabled), typeof(bool), typeof(Stepper), defaultValue: true,
        propertyChanged: (bindableObject, oldValue, newValue) =>
        {
            var stepper = (Stepper)bindableObject;
            if ((bool)newValue)
            {
                stepper.SetDecreaseButtonState(stepper.Value > stepper.Minimum);
                stepper.SetIncreaseButtonState(stepper.Value < stepper.Maximum);
            }
            else
            {
                stepper.SetDecreaseButtonState(false);
                stepper.SetIncreaseButtonState(false);
            }
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

    private void SetDecreaseButtonState(bool isEnabled)
    {
        DecreaseButton.IsEnabled = isEnabled;
        if (DecreaseButton.Source is FontImageSource buttonImage)
        {
            buttonImage.Color = isEnabled ? TextColor.PrimaryTextColor : TextColor.DisabledTextColor;
        }
    }

    private void SetIncreaseButtonState(bool isEnabled)
    {
        IncreaseButton.IsEnabled = isEnabled;
        if (IncreaseButton.Source is FontImageSource buttonImage)
        {
            buttonImage.Color = isEnabled ? TextColor.PrimaryTextColor : TextColor.DisabledTextColor;
        }
    }
}