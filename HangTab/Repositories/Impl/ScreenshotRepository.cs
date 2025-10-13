namespace HangTab.Repositories.Impl;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a Repository for the data layer and does not require unit tests.")]
public class ScreenshotRepository(IScreenshot screenshot) : IScreenshotRepository
{
    public async Task<IScreenshotResult?> TakeScreenshotAsync()
    {
        if (!screenshot.IsCaptureSupported)
        {
            return null;
        }

        return await screenshot.CaptureAsync();
    }
}
