using HangTab.Constants;
using HangTab.Models;

namespace HangTab.Services.Impl;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a service class for navigation, not unit tested directly.")]
public class NavigationService : INavigationService
{
    public Task GoBack() => Shell.Current.GoToAsync("..");

    public async Task GoToAddBowler() => await Shell.Current.GoToAsync(Routes.PersonAdd);

    public async Task GoToEditBowler(Person person)
    {
        var navigationParameter = new ShellNavigationQueryParameters
        {
            { nameof(Person), person }
        };

        await Shell.Current.GoToAsync(Routes.PersonEdit, navigationParameter);
    }

    public async Task GoToSeasonSummary()
    {
        await Shell.Current.GoToAsync(Routes.SeasonSummary);
    }

    public async Task GoToSelectSub(Bowler bowler)
    {
        var navigationParameter = new ShellNavigationQueryParameters
        {
            { nameof(Bowler), bowler }
        };

        await Shell.Current.GoToAsync(Routes.BowlerSelectSub, navigationParameter);
    }

    public async Task GoToWeekDetails(int weekId)
    {
        var navigationParameter = new ShellNavigationQueryParameters
        {
            { nameof(weekId), weekId }
        };

        await Shell.Current.GoToAsync(Routes.WeekDetails, navigationParameter);
    }
}
