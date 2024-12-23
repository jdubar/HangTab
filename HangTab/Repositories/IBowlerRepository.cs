namespace HangTab.Repositories;
public interface IBowlerRepository
{
    Task<Bowler> GetBowler(int id);
}
