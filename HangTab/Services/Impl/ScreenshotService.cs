using HangTab.Repositories;

namespace HangTab.Services.Impl;
public class ScreenshotService(
    IScreenshotRepository screenshot,
    IStorageRepository storage) : IScreenshotService
{
    public async Task<string> TakeScreenshotAsync()
    {
        var result = await screenshot.TakeScreenshotAsync();
        return result is null
            ? string.Empty
            : await storage.SaveScreenshotAsync(result);
    }
}
