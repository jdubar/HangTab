namespace HangTab.Services;
public interface IBowlerService
{
    Task<bool> Add(Bowler bowler);
    Task<bool> Delete(int id);
    Task<bool> Exists(Bowler bowler);
    Task<IReadOnlyCollection<Bowler>> GetAll();
    Task<IReadOnlyCollection<Bowler>> GetActiveOnly();
    Task<IReadOnlyCollection<Bowler>> GetInactiveOnly();
    Task<IReadOnlyCollection<Bowler>> GetSubsOnly();
    Task<bool> Update(Bowler bowler);
}
