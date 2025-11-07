using HangTab.Models;
using HangTab.Repositories;

namespace HangTab.Services.Impl;
public class BowlerService(IBowlerRepository bowlerRepository) : IBowlerService
{
    public Task<bool> AddBowlerAsync(Bowler bowler) => bowlerRepository.AddBowlerAsync(bowler);
    public Task<IEnumerable<Bowler>> GetAllBowlersByWeekIdAsync(int id) => bowlerRepository.GetAllBowlersByWeekIdAsync(id);
    public Task<IEnumerable<Bowler>> GetAllBowlersAsync() => bowlerRepository.GetAllBowlersAsync();
    public Task<Bowler> GetBowlerByIdAsync(int id) => bowlerRepository.GetBowlerByIdAsync(id);
    public Task<bool> RemoveBowlerAsync(int id) => bowlerRepository.RemoveBowlerAsync(id);
    public Task<bool> UpdateBowlerAsync(Bowler bowler) => bowlerRepository.UpdateBowlerAsync(bowler);
}
