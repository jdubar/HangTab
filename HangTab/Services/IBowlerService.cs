namespace HangTab.Services;

public interface IBowlerService
{
    Task<Bowler> GetBowler(int id);
}