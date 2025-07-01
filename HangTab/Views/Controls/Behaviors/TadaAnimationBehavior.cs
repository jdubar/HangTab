using System.ComponentModel;

namespace HangTab.Views.Controls.Behaviors;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a Behavior for the UI and does not require unit tests.")]
public partial class TadaAnimationBehavior : Behavior<Image>
{
    protected override void OnAttachedTo(Image image)
    {
        image.PropertyChanged += OnPropertyChanged;
        base.OnAttachedTo(image);
    }

    protected override void OnDetachingFrom(Image image)
    {
        image.PropertyChanged -= OnPropertyChanged;
        base.OnDetachingFrom(image);
    }

    private static async void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Image.IsVisible) && sender is Image image)
        {
            await TadaAnimation(image);
        }
    }

    private static async Task TadaAnimation(View target)
    {
        const int duration = 400;
        await target.ScaleTo(0.9, duration / 8, Easing.CubicIn);
        await target.RotateTo(-3, duration / 8, Easing.CubicIn);
        await target.ScaleTo(1.1, duration / 4, Easing.CubicOut);
        await target.RotateTo(3, duration / 4, Easing.CubicOut);
        await target.ScaleTo(1, duration / 4, Easing.CubicIn);
        await target.RotateTo(0, duration / 4, Easing.CubicIn);
    }
}
