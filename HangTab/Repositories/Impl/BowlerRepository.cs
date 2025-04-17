using HangTab.Data;
using HangTab.Models;

namespace HangTab.Repositories.Impl;
public class BowlerRepository(IDatabaseContext context) : IBowlerRepository
{
    public Task<bool> AddBowler(Bowler bowler) => context.AddItemAsync(bowler);
    public Task<IEnumerable<Bowler>> GetAllBowlersByWeekId(int id) => context.GetAllWithChildrenAsync<Bowler>(wl => wl.WeekId == id);
    public Task<IEnumerable<Bowler>> GetAllBowlers() => context.GetAllAsync<Bowler>();
    public Task<Bowler> GetBowlerById(int id) => context.GetItemByIdAsync<Bowler>(id);
    public Task<IEnumerable<Bowler>> GetBowlersByWeekId(int weekId) => context.GetFilteredAsync<Bowler>(wl => wl.WeekId == weekId);// TODO: Possibly redundant
    public Task<bool> UpdateBowler(Bowler bowler) => context.UpdateItemAsync(bowler);
}
