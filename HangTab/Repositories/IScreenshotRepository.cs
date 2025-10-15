namespace HangTab.Repositories;
public interface IScreenshotRepository
{
    Task<IScreenshotResult?> TakeScreenshotAsync();
}
