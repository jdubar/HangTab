using HangTab.Models;
using HangTab.Repositories;

namespace HangTab.Services.Impl;
public class WeekService(IWeekRepository weekRepository) : IWeekService
{
    public Task<Week> GetWeek(int id) => weekRepository.GetWeek(id);
    public Task<IEnumerable<Week>> GetWeeks() => weekRepository.GetWeeks();
    public Task<Week> GetWeekByWeekNumber(int weekNumber) => weekRepository.GetWeekByWeekNumber(weekNumber);
    public Task<Week> CreateWeek(int weekNumber) => weekRepository.CreateWeek(weekNumber);
}
