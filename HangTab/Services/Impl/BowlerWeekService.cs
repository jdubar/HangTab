using HangTab.Data.Impl;

namespace HangTab.Services.Impl;
public class BowlerWeekService(DatabaseContext context) : IBowlerWeekService
{
    public async Task<bool> Add(BowlerWeek week) => await context.AddItemAsync(week);

    public async Task<IReadOnlyCollection<BowlerWeek>> GetAll() => await context.GetAllAsync<BowlerWeek>();

    public async Task<bool> Update(BowlerWeek week) => await context.UpdateItemAsync(week);
}
