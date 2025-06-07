namespace HangTab.Services;
public interface IDatabaseService
{
    Task<bool> DeleteAllData();
    Task<bool> DeleteSeasonData();
    Task InitializeDatabase();
}
