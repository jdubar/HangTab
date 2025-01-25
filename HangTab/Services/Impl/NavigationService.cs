using HangTab.Models;

namespace HangTab.Services.Impl;
public class NavigationService : INavigationService
{
    public Task GoBack() => Shell.Current.GoToAsync("..");

    public async Task GoToAddBowler() => await Shell.Current.GoToAsync("bowler/add");

    public async Task GoToEditBowler(Bowler bowler)
    {
        var navigationParameter = new ShellNavigationQueryParameters
        {
            { nameof(Bowler), bowler }
        };

        await Shell.Current.GoToAsync("bowler/edit", navigationParameter);
    }

    public async Task GoToSwitchBowler(WeeklyLineup weeklyLineup)
    {
        var navigationParameter = new ShellNavigationQueryParameters
        {
            { nameof(WeeklyLineup), weeklyLineup }
        };

        await Shell.Current.GoToAsync("bowler/switch", navigationParameter);
    }

    public Task GoToBowlerOverview() => Shell.Current.GoToAsync("//bowleroverview");
    public Task GoToCurrentWeekOverview() => Shell.Current.GoToAsync("//currentweekoverview");
    public Task GoToSettings() => Shell.Current.GoToAsync("//settings");
}
