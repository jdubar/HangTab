namespace HangTab.Repositories;
public interface IDatabaseRepository
{
    Task<bool> DropAllTables();
    Task InitializeDatabase();
}
