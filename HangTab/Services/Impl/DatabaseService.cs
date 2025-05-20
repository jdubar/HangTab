using HangTab.Repositories;

namespace HangTab.Services.Impl;
public class DatabaseService(IDatabaseRepository databaseRepository) : IDatabaseService
{
    public Task<bool> DeleteAllTableData() => databaseRepository.DeleteAllTableData();
    public Task InitializeDatabase() => databaseRepository.InitializeDatabase();
}
