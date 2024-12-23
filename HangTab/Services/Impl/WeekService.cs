using HangTab.Repositories;

namespace HangTab.Services.Impl;
public class WeekService(IWeekRepository weekRepository) : IWeekService
{
    public Task<Week> GetWeek(int weekNumber) => weekRepository.GetWeek(weekNumber);
}
