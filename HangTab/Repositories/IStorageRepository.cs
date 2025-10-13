namespace HangTab.Repositories;
public interface IStorageRepository
{
    Task<string> SaveToDiskAsync(FileResult result);
}
