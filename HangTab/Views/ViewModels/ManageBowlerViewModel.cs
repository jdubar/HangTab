using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using HangTab.Models;
using HangTab.Services;

using System.Collections.ObjectModel;

namespace HangTab.Views.ViewModels;
public partial class ManageBowlerViewModel(IDatabaseService data, IShellService shell) : BaseViewModel
{
    [ObservableProperty]
    private ObservableCollection<Bowler> _allBowlers;

    public async Task LoadAllBowlersAsync()
    {
        await ExecuteAsync(async () =>
        {
            AllBowlers ??= [];
            var bowlers = await data.GetAllBowlers();
            if (bowlers is not null && bowlers.Any())
            {
                AllBowlers.Clear();
                foreach (var bowler in bowlers.OrderBy(b => b.FullName))
                {
                    AllBowlers.Add(bowler);
                }
            }
            else
            {
                AllBowlers.Clear();
            }
        }, "Loading bowlers...");
    }

    [RelayCommand]
    private async Task ShowAddUpdateBowlerViewAsync(Bowler bowler)
    {
        bowler ??= new Bowler();
        var navigationParameter = new ShellNavigationQueryParameters
        {
            { "Bowler", bowler }
        };
        await shell.GoToPage(nameof(AddBowlerPage), navigationParameter);
    }
}
