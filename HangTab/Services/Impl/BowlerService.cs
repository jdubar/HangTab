using HangTab.Models;
using HangTab.Repositories;

namespace HangTab.Services.Impl;
public class BowlerService(IBowlerRepository bowlerRepository) : IBowlerService
{
    public Task<bool> AddBowler(Bowler bowler) => bowlerRepository.AddBowler(bowler);
    public Task<Bowler> GetBowler(int id) => bowlerRepository.GetBowler(id);
    public Task<IEnumerable<Bowler>> GetBowlers() => bowlerRepository.GetBowlers();
    public Task<bool> DeleteBowler(int id) => bowlerRepository.DeleteBowler(id);
    public Task<bool> UpdateBowler(Bowler bowler) => bowlerRepository.UpdateBowler(bowler);
}
