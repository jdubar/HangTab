namespace HangTab.Repositories;

public interface IWeekRepository
{
    Task<Week> GetWeek(int weekNumber);
}