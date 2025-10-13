namespace HangTab.Services;
public interface IScreenshotService
{
    Task<string> TakeScreenshotAsync();
}
