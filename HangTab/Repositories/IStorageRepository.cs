namespace HangTab.Repositories;
public interface IStorageRepository
{
    Task<Result<Stream>> OpenAppPackageFileAsync(string fileName);
    Task<Result<string>> SaveFileAsync(FileResult result);
    Task<Result<string>> SaveScreenshotAsync(IScreenshotResult result);
}
