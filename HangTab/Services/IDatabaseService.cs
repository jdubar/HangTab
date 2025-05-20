namespace HangTab.Services;
public interface IDatabaseService
{
    Task<bool> DeleteAllTableData();
    Task InitializeDatabase();
}
