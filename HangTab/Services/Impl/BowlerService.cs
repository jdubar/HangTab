using HangTab.Repositories;

namespace HangTab.Services.Impl;
public class BowlerService(IBowlerRepository bowlerRepository) : IBowlerService
{
    public Task<Bowler> GetBowler(int id) => bowlerRepository.GetBowler(id);
}
