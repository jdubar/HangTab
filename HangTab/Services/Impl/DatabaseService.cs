using HangTab.Repositories;

namespace HangTab.Services.Impl;
public class DatabaseService(IDatabaseRepository databaseRepository) : IDatabaseService
{
    public Task<bool> DropAllTables() => databaseRepository.DropAllTables();
    public Task InitializeDatabase() => databaseRepository.InitializeDatabase();
}
