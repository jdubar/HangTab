namespace HangTab.Repositories;
public interface IStorageRepository
{
    Task<string> SaveFileAsync(FileResult result);
    Task<string> SaveScreenshotAsync(IScreenshotResult result);
}
