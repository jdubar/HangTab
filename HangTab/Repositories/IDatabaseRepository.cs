namespace HangTab.Repositories;
public interface IDatabaseRepository
{
    Task<bool> DeleteAllData();
    Task<bool> DeleteSeasonData();
    Task InitializeDatabase();
}
