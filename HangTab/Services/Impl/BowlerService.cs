using HangTab.Models;
using HangTab.Repositories;

namespace HangTab.Services.Impl;
public class BowlerService(IBowlerRepository bowlerRepository) : IBowlerService
{
    public Task<bool> AddBowler(Bowler bowler) => bowlerRepository.AddBowler(bowler);
    public Task<IEnumerable<Bowler>> GetAllBowlersByWeekId(int id) => bowlerRepository.GetAllBowlersByWeekId(id);
    public Task<IEnumerable<Bowler>> GetAllBowlers() => bowlerRepository.GetAllBowlers();
    public Task<Bowler> GetBowlerById(int id) => bowlerRepository.GetBowlerById(id);
    public Task<IEnumerable<Bowler>> GetBowlersByWeekId(int weekId) => bowlerRepository.GetBowlersByWeekId(weekId);
    public Task<bool> UpdateBowler(Bowler bowler) => bowlerRepository.UpdateBowler(bowler);
}
