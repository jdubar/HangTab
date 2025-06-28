using HangTab.Repositories;

namespace HangTab.Services.Impl;
public class DatabaseService(IDatabaseRepository databaseRepository) : IDatabaseService
{
    public Task<bool> DeleteAllData() => databaseRepository.DeleteAllData();
    public Task<bool> DeleteSeasonData() => databaseRepository.DeleteSeasonData();
}
