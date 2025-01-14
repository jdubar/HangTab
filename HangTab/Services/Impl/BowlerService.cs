using HangTab.Models;
using HangTab.Repositories;

namespace HangTab.Services.Impl;
public class BowlerService(IBowlerRepository bowlerRepository) : IBowlerService
{
    public Task<Bowler> GetBowlerById(int id) => bowlerRepository.GetBowlerById(id);
    public Task<IEnumerable<Bowler>> GetAllBowlers() => bowlerRepository.GetAllBowlers();
    public Task<IEnumerable<Bowler>> GetRegularBowlers() => bowlerRepository.GetRegularBowlers();
    public Task<IEnumerable<Bowler>> GetSubstituteBowlers() => bowlerRepository.GetSubstituteBowlers();
    public Task<bool> AddBowler(Bowler bowler) => bowlerRepository.AddBowler(bowler);
    public Task<bool> DeleteBowler(int id) => bowlerRepository.DeleteBowler(id);
    public Task<bool> UpdateBowler(Bowler bowler) => bowlerRepository.UpdateBowler(bowler);
}
