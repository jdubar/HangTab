namespace HangTab.Services;
public interface IBowlerService
{
    Task<bool> Add(Bowler bowler);
    Task<bool> Delete(int id);
    Task<bool> Exists(Bowler bowler);
    Task<IReadOnlyCollection<Bowler>> GetAll();
    Task<IReadOnlyCollection<Bowler>> GetActive();
    Task<IReadOnlyCollection<Bowler>> GetInactive();
    Task<IReadOnlyCollection<Bowler>> GetSubs();
    Task<bool> Update(Bowler bowler);
}
