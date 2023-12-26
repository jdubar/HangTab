﻿using HangTab.Models;
using HangTab.ViewModels;

using System.Linq.Expressions;

namespace HangTab.Services;
public interface IDatabaseService
{
    Task<IEnumerable<Bowler>> GetAllBowlers();
    Task<IEnumerable<Bowler>> GetFilteredBowlers(Expression<Func<Bowler, bool>> predicate);

    Task<IEnumerable<BowlerWeek>> GetAllBowlerWeeks();
    Task<IEnumerable<BowlerWeek>> GetFilteredBowlerWeeks(int week);

    Task<BusRideViewModel> GetLatestBusRide(int week);

    Task<int> GetWorkingWeek();

    Task<bool> AddBowler(Bowler bowler);
    Task<bool> DeleteBowler(int id);
    Task<bool> IsBowlerExists(Bowler bowler);
    Task<bool> UpdateBowler(Bowler bowler);

    Task<bool> UpdateBowlerHangingsByWeek(BowlerViewModel viewModel, int week);
    Task<bool> UpdateBusRidesByWeek(BusRideViewModel viewModel, int week);
}
