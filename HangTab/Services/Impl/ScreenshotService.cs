using HangTab.Repositories;

namespace HangTab.Services.Impl;
public class ScreenshotService(
    IScreenshotRepository screenshot,
    IStorageRepository storage) : IScreenshotService
{
    public async Task<string> TakeScreenshotAsync()
    {
        var fileResult = await screenshot.TakeScreenshotAsync();
        if (fileResult is null)
        {
            return string.Empty;
        }

        var result = await storage.SaveScreenshotAsync(fileResult);
        return result.IsSuccess
            ? result.Value
            : string.Empty;
    }
}
