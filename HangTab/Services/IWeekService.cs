using HangTab.Models.Wrappers;

namespace HangTab.Services;
public interface IWeekService
{
    Task<bool> Add(Week week);
    Task<bool> AddBusRide(WeekWrapper wrapper);
    Task<Week> Get(int weekNumber);
    Task<IReadOnlyCollection<Week>> GetAll();
    Task<int> GetBusRideTotal();
    Task<bool> UndoBusRide(Week week);
    Task<bool> Update(Week week);
}