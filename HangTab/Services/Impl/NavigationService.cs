using HangTab.Constants;
using HangTab.Models;

namespace HangTab.Services.Impl;
public class NavigationService : INavigationService
{
    public Task GoBack() => Shell.Current.GoToAsync("..");

    public async Task GoToAddBowler() => await Shell.Current.GoToAsync(Routes.BowlerAdd);

    public async Task GoToEditBowler(Bowler bowler)
    {
        var navigationParameter = new ShellNavigationQueryParameters
        {
            { nameof(Bowler), bowler }
        };

        await Shell.Current.GoToAsync(Routes.BowlerEdit, navigationParameter);
    }

    public async Task GoToSwitchBowler(WeeklyLineup weeklyLineup)
    {
        var navigationParameter = new ShellNavigationQueryParameters
        {
            { nameof(WeeklyLineup), weeklyLineup }
        };

        await Shell.Current.GoToAsync(Routes.BowlerSwitch, navigationParameter);
    }

    public Task GoToBowlerOverview() => Shell.Current.GoToAsync(Routes.BowlerOverview);
    public Task GoToCurrentWeekOverview() => Shell.Current.GoToAsync(Routes.CurrentWeekOverview);
    public Task GoToSettings() => Shell.Current.GoToAsync(Routes.Settings);
}
