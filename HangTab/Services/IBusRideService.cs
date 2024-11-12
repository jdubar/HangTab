namespace HangTab.Services;
public interface IBusRideService
{
    Task<IReadOnlyCollection<BusRide>> GetAll();
    Task<int> GetTotal();
}
