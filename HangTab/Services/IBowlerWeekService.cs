namespace HangTab.Services;
public interface IBowlerWeekService
{
    Task<bool> Add(BowlerWeek week);
    Task<IReadOnlyCollection<BowlerWeek>> GetAll();
    Task<bool> Update(BowlerWeek week);
}
