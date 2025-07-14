namespace HangTab.Views.Controls.ConfettiView;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We will not test the model. There's no logic to test.")]
public class ConfettiParticle
{
    public float X { get; set; }
    public float Y { get; set; }
    public float SpeedY { get; set; }
    public float SpeedX { get; set; }
    public float Size { get; set; }
    public Color? Color { get; set; }
    public float Rotation { get; set; }
    public float RotationSpeed { get; set; }
    public float Flip { get; set; } // -1 to 1, for flipping effect
    public float FlipSpeed { get; set; }
}