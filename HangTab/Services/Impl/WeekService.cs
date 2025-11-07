using HangTab.Models;
using HangTab.Repositories;

namespace HangTab.Services.Impl;
public class WeekService(IWeekRepository weekRepository) : IWeekService
{
    public Task<Week> GetWeekByIdAsync(int id) => weekRepository.GetWeekByIdAsync(id);
    public Task<IEnumerable<Week>> GetAllWeeksAsync() => weekRepository.GetAllWeeksAsync();
    public Task<Week> CreateWeekAsync(int weekNumber) => weekRepository.CreateWeekAsync(weekNumber);
    public Task UpdateWeekAsync(Week week) => weekRepository.UpdateWeekAsync(week);
}
