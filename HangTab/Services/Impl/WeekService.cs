using HangTab.Models;
using HangTab.Repositories;

namespace HangTab.Services.Impl;
public class WeekService(
    IBaseRepository<Week> weekRepo,
    IBaseRepository<Person> personRepo,
    IBaseRepository<Bowler> bowlerRepo) : IWeekService
{
    public async Task<Result<Week>> AddAsync(int weekNumber = 1)
    {
        var week = new Week
        {
            Number = weekNumber
        };
        await weekRepo.AddAsync(week);

        var regulars = await personRepo.GetFilteredAsync(p => !p.IsSub);
        if (!regulars.Any())
        {
            return Result.Ok(week);
        }

        foreach (var regular in regulars)
        {
            var bowler = new Bowler
            {
                PersonId = regular.Id,
                WeekId = week.Id
            };
            await bowlerRepo.AddAsync(bowler);
        }

        return Result.Ok(week);
    }

    public async Task<Result<IEnumerable<Week>>> GetAllAsync()
    {
        var weeks = await weekRepo.GetAllAsync();
        return weeks is null || !weeks.Any()
            ? Result.Fail<IEnumerable<Week>>("No weeks found.")
            : Result.Ok(weeks);
    }

    public async Task<Result<Week>> GetByIdAsync(int id)
    {
        if (id < 1)
        {
            return await AddAsync(); // Create the first week if no valid ID is provided
        }

        var week = await weekRepo.GetByIdAsync(id);
        if (week is null)
        {
            return Result.Ok(new Week());
        }

        var lineup = new List<Bowler>();
        var bowlers = await bowlerRepo.GetFilteredAsync(b => b.WeekId == id);
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
                Person = await personRepo.GetByIdAsync(bowler.SubId is not null ? (int)bowler.SubId : bowler.PersonId)
            });
        }

        week.Bowlers = lineup;
        return Result.Ok(week);
    }

    public async Task<Result> UpdateAsync(Week week)
    {
        if (week is null)
        {
            return Result.Fail("Week cannot be null.");
        }

        return await weekRepo.UpdateAsync(week)
            ? Result.Ok()
            : Result.Fail("Failed to update the week.");
    }
}
