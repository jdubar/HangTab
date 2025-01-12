using HangTab.Data;
using HangTab.Models;

namespace HangTab.Repositories.Impl;
public class WeekRepository(IDatabaseContext context) : IWeekRepository
{
    public Task<Week> GetWeek(int id) => context.GetItemByIdAsync<Week>(id);
    public Task<IEnumerable<Week>> GetWeeks() => context.GetAllAsync<Week>();
    public async Task<Week> GetWeekByWeekNumber(int weekNumber)
    {
        var weeks = await context.GetFilteredAsync<Week>(w => w.WeekNumber == weekNumber);
        return weeks.Any()
            ? weeks.First()
            : await CreateWeek(weekNumber);
    }
    public async Task<Week> CreateWeek(int weekNumber)
    {
        var week = new Week
        {
            WeekNumber = weekNumber,
            BusRides = 0,
            Bowlers = []
        };

        var regulars = await context.GetFilteredAsync<Bowler>(b => !b.IsSub);
        if (!regulars.Any())
        {
            return week;
        }

        var lineup = regulars.Select((b, index) => new WeeklyLineup
        {
            Position = index + 1,
            Status = Enums.BowlerStatus.Active,
            HangCount = 0,
            WeekId = week.Id,
            BowlerId = b.Id,
            Bowler = b
        }).ToList();

        week.Bowlers = lineup;

        if (await context.AddItemAsync(week))
        {
            return week;
        }
        else
        {
            return new Week(); // TODO: Handle this better
        }
    }
}
