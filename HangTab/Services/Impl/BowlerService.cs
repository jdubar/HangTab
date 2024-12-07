using HangTab.Data;

namespace HangTab.Services.Impl;

public class BowlerService(IDatabaseContext context) : IBowlerService
{
    public async Task<bool> Add(Bowler bowler) => await context.AddItemAsync(bowler);

    public async Task<bool> Delete(int id) => await context.DeleteItemByIdAsync<Bowler>(id);

    public async Task<bool> Exists(Bowler bowler)
    {
        return !string.IsNullOrEmpty(bowler.FirstName)
               && (await context.GetFilteredAsync<Bowler>(b =>
                   b.FirstName == bowler.FirstName && b.LastName == bowler.LastName)).Count > 0;
    }

    public async Task<IReadOnlyCollection<Bowler>> GetAll() => await context.GetAllAsync<Bowler>();

    public async Task<IReadOnlyCollection<Bowler>> GetActive() => await context.GetFilteredAsync<Bowler>(b => !b.IsHidden);

    public async Task<IReadOnlyCollection<Bowler>> GetInactive() => await context.GetFilteredAsync<Bowler>(b => b.IsHidden);

    public async Task<IReadOnlyCollection<Bowler>> GetSubs() => await context.GetFilteredAsync<Bowler>(b => b.IsSub);

    public async Task<bool> Update(Bowler bowler) => await context.UpdateItemAsync(bowler);
}