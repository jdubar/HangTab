using HangTab.Data;

namespace HangTab.Repositories.Impl;
public class BowlerRepository(IDatabaseContext context) : IBowlerRepository
{
    public Task<Bowler> GetBowler(int id) => context.GetItemByIdAsync<Bowler>(id);
}
