using HangTab.Data;
using HangTab.Models;

namespace HangTab.Repositories.Impl;
public class BowlerRepository(IDatabaseContext context) : IBowlerRepository
{
    public Task<IEnumerable<Bowler>> GetBowlers() => context.GetAllAsync<Bowler>();
    public Task<Bowler> GetBowler(int id) => context.GetItemByIdAsync<Bowler>(id);
    public Task<bool> AddBowler(Bowler bowler) => context.AddItemAsync(bowler);
    public Task<bool> DeleteBowler(int id) => context.DeleteItemByIdAsync<Bowler>(id);
    public Task<bool> UpdateBowler(Bowler bowler) => context.UpdateItemAsync(bowler);
}
