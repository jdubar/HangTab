using HangTab.Data;
using HangTab.Models;

namespace HangTab.Repositories.Impl;
public class WeekRepository(IDatabaseContext context) : IWeekRepository
{
    public async Task<Week> GetWeek(int id)
    {
        var week = await context.GetItemByIdAsync<Week>(id);
        if (week is null)
        {
            return new Week();
        }

        var lineup = new List<Bowler>();
        var bowlers = await context.GetFilteredAsync<Bowler>(b => b.WeekId == id);
        foreach (var bowler in bowlers)
        {
            lineup.Add(new Bowler
            {
                Id = bowler.Id,
                Status = bowler.Status,
                HangCount = bowler.HangCount,
                WeekId = bowler.WeekId,
                PersonId = bowler.PersonId,
                SubId = bowler.SubId,
                Person = await context.GetItemByIdAsync<Person>(bowler.SubId is null ? bowler.Id : bowler.SubId)
            });
        }

        week.Bowlers = lineup;
        return week;
    }

    public Task<IEnumerable<Week>> GetAllWeeks() => context.GetAllWithChildrenAsync<Week>();
    public Task<Week> GetWeekById(int id) => context.GetItemByIdAsync<Week>(id);

    public async Task<Week> CreateWeek(int weekNumber)
    {
        var week = new Week
        {
            Number = weekNumber
        };

        // TODO: Is this still needed?
        await context.CreateTableIfNotExists<Bowler>();
        await context.CreateTableIfNotExists<Person>();
        await context.CreateTableIfNotExists<Week>();

        await context.AddItemAsync(week);
        return week;
    }

    public Task UpdateWeek(Week week) => context.UpdateWithChildrenAsync(week);
}
