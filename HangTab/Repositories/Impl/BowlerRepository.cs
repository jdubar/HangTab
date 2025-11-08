using HangTab.Data;
using HangTab.Models;

namespace HangTab.Repositories.Impl;
public class BowlerRepository(IDatabaseContext context) : IBowlerRepository
{
    public Task<bool> AddAsync(Bowler bowler)
    {
        ArgumentNullException.ThrowIfNull(bowler);
        return context.AddItemAsync(bowler);
    }

    public Task<IEnumerable<Bowler>> GetAllByWeekIdAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
        return context.GetAllWithChildrenAsync<Bowler>(wl => wl.WeekId == id);
    }

    public Task<IEnumerable<Bowler>> GetAllAsync() => context.GetAllWithChildrenAsync<Bowler>();

    public Task<Bowler> GetByIdAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
        return context.GetItemByIdAsync<Bowler>(id);
    }

    public Task<bool> RemoveAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
        return context.DeleteItemByIdAsync<Bowler>(id);
    }

    public Task<bool> UpdateAsync(Bowler bowler)
    {
        ArgumentNullException.ThrowIfNull(bowler);
        return context.UpdateItemAsync(bowler);
    }
}
