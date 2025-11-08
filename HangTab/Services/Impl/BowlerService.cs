using HangTab.Models;
using HangTab.Repositories;

namespace HangTab.Services.Impl;
public class BowlerService(IBowlerRepository repo) : IBowlerService
{
    public Task<bool> AddAsync(Bowler bowler) => repo.AddAsync(bowler);
    public Task<IEnumerable<Bowler>> GetAllByWeekIdAsync(int id) => repo.GetAllByWeekIdAsync(id);
    public Task<IEnumerable<Bowler>> GetAllAsync() => repo.GetAllAsync();
    public Task<Bowler> GetByIdAsync(int id) => repo.GetByIdAsync(id);
    public Task<bool> RemoveAsync(int id) => repo.RemoveAsync(id);
    public Task<bool> UpdateAsync(Bowler bowler) => repo.UpdateAsync(bowler);
}
