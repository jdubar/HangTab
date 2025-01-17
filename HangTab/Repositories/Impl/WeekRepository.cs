using HangTab.Data;
using HangTab.Models;

namespace HangTab.Repositories.Impl;
public class WeekRepository(IDatabaseContext context) : IWeekRepository
{
    public Task<Week> GetWeek(int id) => context.GetWithChildrenAsync<Week>(id);
    public Task<IEnumerable<Week>> GetAllWeeks() => context.GetAllWithChildrenAsync<Week>();
    public Task<Week> GetWeekById(int id) => context.GetItemByIdAsync<Week>(id);

    public async Task<Week> CreateWeek(int weekNumber)
    {
        var week = new Week
        {
            WeekNumber = weekNumber
        };

        await context.AddItemAsync(week);
        return week;
    }

    public Task UpdateWeek(Week week) => context.UpdateWithChildrenAsync(week);
}
