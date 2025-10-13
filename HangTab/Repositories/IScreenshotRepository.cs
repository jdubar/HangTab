namespace HangTab.Repositories;
public interface IScreenshotRepository
{
    Task<ImageSource?> TakeScreenshotAsync();
}
