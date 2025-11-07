using HangTab.Repositories;

namespace HangTab.Services.Impl;
public class DatabaseService(IDatabaseRepository databaseRepository) : IDatabaseService
{
    public Task<bool> DeleteAllDataAsync() => databaseRepository.DeleteAllDataAsync();
    public Task<bool> DeleteSeasonDataAsync() => databaseRepository.DeleteSeasonDataAsync();
    public Task InitializeDatabaseAsync() => databaseRepository.InitializeDatabaseAsync();
}
