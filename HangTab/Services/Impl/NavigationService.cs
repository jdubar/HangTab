using HangTab.Constants;
using HangTab.Models;

namespace HangTab.Services.Impl;
public class NavigationService : INavigationService
{
    public Task GoBack() => Shell.Current.GoToAsync("..");

    public async Task GoToAddBowler() => await Shell.Current.GoToAsync(Routes.BowlerAdd);

    public async Task GoToEditBowler(Person person)
    {
        var navigationParameter = new ShellNavigationQueryParameters
        {
            { nameof(Person), person }
        };

        await Shell.Current.GoToAsync(Routes.BowlerEdit, navigationParameter);
    }

    public async Task GoToSwitchBowler(Bowler bowler)
    {
        var navigationParameter = new ShellNavigationQueryParameters
        {
            { nameof(Bowler), bowler }
        };

        await Shell.Current.GoToAsync(Routes.BowlerSwitch, navigationParameter);
    }

    public async Task GoToSelectSub(Bowler bowler)
    {
        var navigationParameter = new ShellNavigationQueryParameters
        {
            { nameof(Bowler), bowler }
        };

        await Shell.Current.GoToAsync(Routes.BowlerSelectSub, navigationParameter);
    }

    public Task GoToBowlerOverview() => Shell.Current.GoToAsync(Routes.BowlerOverview);
    public Task GoToCurrentWeekOverview() => Shell.Current.GoToAsync(Routes.CurrentWeekOverview);
    public Task GoToSettings() => Shell.Current.GoToAsync(Routes.Settings);
}
