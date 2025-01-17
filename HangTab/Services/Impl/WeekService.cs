using HangTab.Models;
using HangTab.Repositories;

namespace HangTab.Services.Impl;
public class WeekService(IWeekRepository weekRepository) : IWeekService
{
    public Task<Week> GetWeek(int id) => weekRepository.GetWeek(id);
    public Task<IEnumerable<Week>> GetAllWeeks() => weekRepository.GetAllWeeks();
    public Task<Week> GetWeekById(int weekNumber) => weekRepository.GetWeekById(weekNumber);
    public Task<Week> CreateWeek(int weekNumber) => weekRepository.CreateWeek(weekNumber);
    public Task UpdateWeek(Week week) => weekRepository.UpdateWeek(week);
}
