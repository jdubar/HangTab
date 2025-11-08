using HangTab.Models;
using HangTab.Repositories;

namespace HangTab.Services.Impl;
public class WeekService(IWeekRepository repo) : IWeekService
{
    public Task<Week> CreateAsync(int weekNumber) => repo.CreateAsync(weekNumber);
    public Task<IEnumerable<Week>> GetAllAsync() => repo.GetAllAsync();
    public Task<Week> GetByIdAsync(int id) => repo.GetByIdAsync(id);
    public Task UpdateAsync(Week week) => repo.UpdateAsync(week);
}
