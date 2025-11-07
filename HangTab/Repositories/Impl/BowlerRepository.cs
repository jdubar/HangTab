using HangTab.Data;
using HangTab.Models;

namespace HangTab.Repositories.Impl;
public class BowlerRepository(IDatabaseContext context) : IBowlerRepository
{
    public Task<bool> AddBowlerAsync(Bowler bowler)
    {
        ArgumentNullException.ThrowIfNull(bowler);
        return context.AddItemAsync(bowler);
    }

    public Task<IEnumerable<Bowler>> GetAllBowlersByWeekIdAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
        return context.GetAllWithChildrenAsync<Bowler>(wl => wl.WeekId == id);
    }

    public Task<IEnumerable<Bowler>> GetAllBowlersAsync() => context.GetAllWithChildrenAsync<Bowler>();

    public Task<Bowler> GetBowlerByIdAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
        return context.GetItemByIdAsync<Bowler>(id);
    }

    public Task<bool> RemoveBowlerAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
        return context.DeleteItemByIdAsync<Bowler>(id);
    }

    public Task<bool> UpdateBowlerAsync(Bowler bowler)
    {
        ArgumentNullException.ThrowIfNull(bowler);
        return context.UpdateItemAsync(bowler);
    }
}
