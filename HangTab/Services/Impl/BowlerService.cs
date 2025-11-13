using HangTab.Models;
using HangTab.Repositories;

namespace HangTab.Services.Impl;
public class BowlerService(IBaseRepository<Bowler> repo) : IBowlerService
{
    public async Task<Result> AddAsync(Bowler bowler)
    {
        if (bowler is null)
        {
            return Result.Fail("Bowler cannot be null");
        }

        return await repo.AddAsync(bowler)
            ? Result.Ok()
            : Result.Fail("Failed to add the bowler");
    }

    public async Task<Result<IEnumerable<Bowler>>> GetAllAsync()
    {
        var bowlers = await repo.GetAllAsync();
        return bowlers is null || !bowlers.Any()
            ? Result.Fail("No bowlers found")
            : Result.Ok(bowlers);
    }

    public async Task<Result<IEnumerable<Bowler>>> GetAllByWeekIdAsync(int id)
    {
        if (id < 1)
        {
            return Result.Fail("Invalid week ID");
        }

        var bowlers = await repo.GetFilteredAsync(b => b.WeekId == id);
        return bowlers is null || !bowlers.Any()
            ? Result.Fail("No bowlers found for the specified week")
            : Result.Ok(bowlers);
    }

    public async Task<Result<Bowler>> GetByIdAsync(int id)
    {
        if (id < 1)
        {
            return Result.Fail("Invalid bowler ID");
        }

        var bowler = await repo.GetByIdAsync(id);
        return bowler is null
            ? Result.Fail("Bowler not found")
            : Result.Ok(bowler);
    }

    public async Task<Result> RemoveAsync(int id)
    {
        if (id < 1)
        {
            return Result.Fail("Invalid bowler ID");
        }

        return await repo.DeleteByIdAsync(id)
            ? Result.Ok()
            : Result.Fail("Failed to remove the bowler");
    }

    public async Task<Result> UpdateAsync(Bowler bowler)
    {
        if (bowler is null)
        {
            return Result.Fail("Bowler cannot be null");
        }

        return await repo.UpdateAsync(bowler)
            ? Result.Ok()
            : Result.Fail("Failed to update the bowler");
    }
}
