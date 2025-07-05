namespace HangTab.Views.Controls.ConfettiView;
public class ConfettiDrawable(List<ConfettiParticle> particles) : IDrawable
{
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        foreach (var p in particles.Where(p => p.Color is not null))
        {
            canvas.SaveState();
            // Move to center of the square
            canvas.Translate(p.X + p.Size / 2, p.Y + p.Size / 2);
            canvas.Rotate(p.Rotation);
            // Flip horizontally using scaleX (simulate flipping)
            canvas.Scale(p.Flip, 1);
            canvas.Translate(-p.Size / 2, -p.Size / 2);
            canvas.FillColor = p.Color;
            canvas.FillRectangle(0, 0, p.Size, p.Size);
            canvas.RestoreState();
        }
    }
}