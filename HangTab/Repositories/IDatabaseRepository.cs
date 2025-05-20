namespace HangTab.Repositories;
public interface IDatabaseRepository
{
    Task<bool> DeleteAllTableData();
    Task InitializeDatabase();
}
