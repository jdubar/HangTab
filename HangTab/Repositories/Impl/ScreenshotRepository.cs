namespace HangTab.Repositories.Impl;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a Repository for the data layer and does not require unit tests.")]
public class ScreenshotRepository(IScreenshot screenshot) : IScreenshotRepository
{
    public async Task<ImageSource?> TakeScreenshotAsync()
    {
        if (!screenshot.IsCaptureSupported)
        {
            return null;
        }

        var result = await screenshot.CaptureAsync();
        var stream = await result.OpenReadAsync();

        return ImageSource.FromStream(() => stream);
    }
}
