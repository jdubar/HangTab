using HangTab.Data;

namespace HangTab.Services.Impl;
public class BusRideService(IDatabaseContext context) : IBusRideService
{
    public async Task<IReadOnlyCollection<BusRide>> GetAll() => await context.GetAllAsync<BusRide>();

    public async Task<int> GetTotal()
    {
        var busRide = await context.GetItemByIdAsync<BusRide>(1);
        return busRide.Total;
    }
}
