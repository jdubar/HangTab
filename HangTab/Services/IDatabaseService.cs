namespace HangTab.Services;
public interface IDatabaseService
{
    Task<bool> DeleteAllDataAsync();
    Task<bool> DeleteSeasonDataAsync();
    Task InitializeDatabaseAsync();
}
