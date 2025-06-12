using HangTab.Data;
using HangTab.Models;

namespace HangTab.Repositories.Impl;
public class BowlerRepository(IDatabaseContext context) : IBowlerRepository
{
    public Task<bool> AddBowler(Bowler bowler)
    {
        ArgumentNullException.ThrowIfNull(bowler);
        return context.AddItemAsync(bowler);
    }

    public Task<IEnumerable<Bowler>> GetAllBowlersByWeekId(int id)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
        return context.GetAllWithChildrenAsync<Bowler>(wl => wl.WeekId == id);
    }

    public Task<IEnumerable<Bowler>> GetAllBowlers() => context.GetAllAsync<Bowler>();

    public Task<Bowler> GetBowlerById(int id)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
        return context.GetItemByIdAsync<Bowler>(id);
    }

    public Task<IEnumerable<Bowler>> GetBowlersByWeekId(int weekId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(weekId);
        return context.GetFilteredAsync<Bowler>(wl => wl.WeekId == weekId);// TODO: Possibly redundant
    }

    public Task<bool> UpdateBowler(Bowler bowler)
    {
        ArgumentNullException.ThrowIfNull(bowler);
        return context.UpdateItemAsync(bowler);
    }
}
