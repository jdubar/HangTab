using Microsoft.Maui.Layouts;

namespace HangTab.Views.Components;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We won't test UI code-behind.")]
public class SlideControl : AbsoluteLayout
{
    private static readonly BindableProperty FillBarProperty =
        BindableProperty.Create(nameof(FillBar), typeof(View), typeof(SlideControl), defaultValue: default(View));

    private static readonly BindableProperty ThumbProperty =
        BindableProperty.Create(nameof(Thumb), typeof(View), typeof(SlideControl), defaultValue: default(View));

    private static readonly BindableProperty TrackBarProperty =
        BindableProperty.Create(nameof(TrackBar), typeof(View), typeof(SlideControl), defaultValue: default(View));

    public View FillBar
    {
        get => (View)GetValue(FillBarProperty);
        set => SetValue(FillBarProperty, value);
    }

    public View Thumb
    {
        get => (View)GetValue(ThumbProperty);
        set => SetValue(ThumbProperty, value);
    }

    public View TrackBar
    {
        get => (View)GetValue(TrackBarProperty);
        set => SetValue(TrackBarProperty, value);
    }

    private readonly PanGestureRecognizer _panGesture = new();
    private readonly View _gestureListener;

    public SlideControl()
    {
        _panGesture.PanUpdated += OnPanGestureUpdated;
        SizeChanged += OnSizeChanged;

        _gestureListener = new ContentView { BackgroundColor = Colors.White, Opacity = 0.05 };
        _gestureListener.GestureRecognizers.Add(_panGesture);
    }

    public event EventHandler SlideCompleted;

    private const double FadeEffect = 0.5;
    private const uint AnimLength = 50;

    private async void OnPanGestureUpdated(object sender, PanUpdatedEventArgs e)
    {
        if (Thumb is null || TrackBar is null || FillBar is null)
        {
            return;
        }

        switch (e.StatusType)
        {
            case GestureStatus.Started:
                await TrackBar.FadeTo(FadeEffect, AnimLength);
                break;

            case GestureStatus.Running:
                var x = Math.Max(0, e.TotalX);
                if (x > Width - Thumb.Width)
                {
                    x = Width - Thumb.Width;
                }

                Thumb.TranslationX = x;
                SetLayoutBounds((IView)FillBar, new Rect(0, 0, x + Thumb.Width / 2, Height));
                break;

            case GestureStatus.Completed:
                var posX = Thumb.TranslationX;
                SetLayoutBounds((IView)FillBar, new Rect(0, 0, 0, Height));

                var tasks = new Task[]
                {
                    TrackBar.FadeTo(1, AnimLength),
                    Thumb.TranslateTo(0, 0, AnimLength * 2, Easing.CubicIn),
                };

                await Task.WhenAll(tasks);

                if (posX >= Width - Thumb.Width - 10)/* keep some margin for error*/
                {
                    SlideCompleted?.Invoke(this, EventArgs.Empty);
                }

                break;
            case GestureStatus.Canceled:
            default:
                break;
        }
    }

    private void OnSizeChanged(object sender, EventArgs e)
    {
        if (Width <= 0 || Height <= 0)
        {
            return;
        }

        if (Thumb is null || TrackBar is null || FillBar is null)
        {
            return;
        }

        Children.Clear();

        SetLayoutFlags((IView)TrackBar, AbsoluteLayoutFlags.SizeProportional);
        SetLayoutBounds((IView)TrackBar, new Rect(0, 0, 1, 1));
        Children.Add(TrackBar);

        SetLayoutFlags((IView)FillBar, AbsoluteLayoutFlags.None);
        SetLayoutBounds((IView)FillBar, new Rect(0, 0, 0, Height));
        Children.Add(FillBar);

        SetLayoutFlags((IView)Thumb, AbsoluteLayoutFlags.None);
        SetLayoutBounds((IView)Thumb, new Rect(0, 0, Width / 5, Height));
        Children.Add(Thumb);

        SetLayoutFlags((IView)_gestureListener, AbsoluteLayoutFlags.SizeProportional);
        SetLayoutBounds((IView)_gestureListener, new Rect(0, 0, 1, 1));
        Children.Add(_gestureListener);
    }
}
