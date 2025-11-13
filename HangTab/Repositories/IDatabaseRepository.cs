namespace HangTab.Repositories;
public interface IDatabaseRepository
{
    Task<bool> DeleteAllDataAsync();
    Task<bool> DeleteSeasonDataAsync();
    Task InitializeDatabaseAsync();
}
