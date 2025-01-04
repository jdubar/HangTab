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
            { "Bowler", bowler }
        };

        await Shell.Current.GoToAsync("bowler/edit", navigationParameter);
    }

    public Task GoToOverview() => Shell.Current.GoToAsync("//overview");
}
