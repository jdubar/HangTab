using HangTab.Data.Impl;
using HangTab.Models.Wrappers;

namespace HangTab.Services.Impl;
public class WeekService(DatabaseContext context) : IWeekService
{
    public async Task<bool> Add(Week week) => await context.AddItemAsync(week);

    public async Task<bool> AddBusRide(WeekWrapper wrapper)
    {
        wrapper.BusRideCount++;
        return await Update(Week.GenerateNewFromWrapper(wrapper));
    }

    public async Task<Week> Get(int weekNumber)
    {
        var weeks = await context.GetAllAsync<Week>();
        var week = weeks.FirstOrDefault(w => w.WeekNumber == weekNumber);
        return week ?? new Week();
    }

    public async Task<IReadOnlyCollection<Week>> GetAll() => await context.GetAllAsync<Week>();

    public async Task<int> GetBusRideTotal()
    {
        var weeks = await context.GetAllAsync<Week>();
        return weeks.Sum(week => week.BusRideCount);
    }

    public async Task<bool> UndoBusRide(Week week)
    {
        week.BusRideCount--;
        return await Update(week);
    }

    public async Task<bool> Update(Week week) => await context.UpdateItemAsync(week);
}
