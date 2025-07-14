namespace HangTab.Views.Controls.ConfettiView;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We will not test the view code behind. There's no logic to test.")]
public partial class ConfettiView : GraphicsView
{
    private readonly Random _random = new();
    private readonly List<ConfettiParticle> _particles = [];
    private IDispatcherTimer? _timer;
    private bool _started = false;
    private const int ParticleCount = 60;

    public ConfettiView()
    {
        Drawable = new ConfettiDrawable(_particles);
        Loaded += OnLoaded;
    }

    private async void OnLoaded(object? sender, EventArgs e)
    {
        if (!_started)
        {
            _started = true;
            await Task.Delay(500);
            StartAnimation();
        }
    }

    private void StartAnimation()
    {
        _particles.Clear();
        for (var i = 0; i < ParticleCount; i++)
        {
            _particles.Add(CreateParticle());
        }

        _timer = Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(25);
        _timer.Tick += (s, e) =>
        {
            foreach (var p in _particles)
            {
                p.Y += p.SpeedY;
                p.X += p.SpeedX;
                p.Rotation += p.RotationSpeed;
                p.Flip += p.FlipSpeed;

                // Flip value oscillates between -1 and 1 for a flipping effect
                if (p.Flip > 1f) p.Flip = -1f;

                if (p.Y > Height)
                {
                    p.Y = -p.Size;
                    p.X = (float)_random.NextDouble() * (float)Width; // Restart from random X at top
                    p.Rotation = _random.NextSingle() * 360f;
                    p.Flip = _random.NextSingle() * 2f - 1f;
                }
            }
            Invalidate();
        };
        _timer.Start();
    }

    private ConfettiParticle CreateParticle() => new()
    {
        X = (float)_random.NextDouble() * (float)Width, // Start from random X at top
        Y = _random.NextSingle() * 20 - 20, // Slightly above the top
        SpeedY = 2 + _random.NextSingle() * 3,
        SpeedX = -1 + _random.NextSingle() * 2,
        Size = 3 + _random.NextSingle() * 3,
        Color = Color.FromRgb(_random.Next(256), _random.Next(256), _random.Next(256)),
        Rotation = _random.NextSingle() * 360f,
        RotationSpeed = -2f + _random.NextSingle() * 4f, // -2 to +2 degrees per frame
        Flip = _random.NextSingle() * 2f - 1f,
        FlipSpeed = 0.05f + _random.NextSingle() * 0.05f // Flip speed
    };
}