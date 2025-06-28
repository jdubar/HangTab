using HangTab.Models;
using HangTab.Repositories;

namespace HangTab.Services.Impl;
public class WeekService(IWeekRepository weekRepository) : IWeekService
{
    public Task<Week> GetWeekById(int id) => weekRepository.GetWeekById(id);
    public Task<IEnumerable<Week>> GetAllWeeks() => weekRepository.GetAllWeeks();
    public Task<Week> CreateWeek(int weekNumber) => weekRepository.CreateWeek(weekNumber);
    public Task UpdateWeek(Week week) => weekRepository.UpdateWeek(week);
}
