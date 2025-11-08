using HangTab.Data;
using HangTab.Models;

namespace HangTab.Repositories.Impl;
public class WeekRepository(IDatabaseContext context) : IWeekRepository
{
    public async Task<Week> CreateAsync(int weekNumber = 1)
    {
        var week = new Week
        {
            Number = weekNumber
        };
        await context.AddItemAsync(week);

        var regulars = await context.GetFilteredAsync<Person>(p => !p.IsSub);
        if (!regulars.Any())
        {
            return week;
        }

        foreach (var regular in regulars)
        {
            var bowler = new Bowler
            {
                PersonId = regular.Id,
                WeekId = week.Id
            };
            await context.AddItemAsync(bowler);
        }

        return week;
    }

    public Task<IEnumerable<Week>> GetAllAsync() => context.GetAllWithChildrenAsync<Week>();

    public async Task<Week> GetByIdAsync(int id)
    {
        if (id < 1)
        {
            return await CreateAsync(); // Create the first week if no valid ID is provided
        }

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
                Person = await context.GetItemByIdAsync<Person>(bowler.SubId is null ? bowler.PersonId : bowler.SubId)
            });
        }

        week.Bowlers = lineup;
        return week;
    }

    public Task UpdateAsync(Week week) => context.UpdateWithChildrenAsync(week);
}
