namespace HangTab.Repositories;
public interface IShareRepository
{
    Task ShareFileAsync(string filePath);
}
