namespace HangTab.Services;
public interface IDatabaseService
{
    Task<bool> DropAllTables();
}
