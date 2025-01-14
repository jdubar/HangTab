using HangTab.Data;
using HangTab.Models;

namespace HangTab.Repositories.Impl;
public class BowlerRepository(IDatabaseContext context) : IBowlerRepository
{
    public Task<Bowler> GetBowlerById(int id) => context.GetItemByIdAsync<Bowler>(id);
    public Task<IEnumerable<Bowler>> GetAllBowlers() => context.GetAllAsync<Bowler>();
    public Task<IEnumerable<Bowler>> GetRegularBowlers() => context.GetFilteredAsync<Bowler>(b => !b.IsSub);
    public Task<IEnumerable<Bowler>> GetSubstituteBowlers() => context.GetFilteredAsync<Bowler>(b => b.IsSub);
    public Task<bool> AddBowler(Bowler bowler) => context.AddItemAsync(bowler);
    public Task<bool> DeleteBowler(int id) => context.DeleteItemByIdAsync<Bowler>(id);
    public Task<bool> UpdateBowler(Bowler bowler) => context.UpdateItemAsync(bowler);
}
