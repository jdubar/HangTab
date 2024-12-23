namespace HangTab.Services;

public interface IWeekService
{
    Task<Week> GetWeek(int weekNumber);
}