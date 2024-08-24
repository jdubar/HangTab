using CommunityToolkit.Mvvm.Input;

using HangTab.Models;
using HangTab.Services;

using MvvmHelpers;

namespace HangTab.Views.ViewModels;
public partial class ManageBowlerViewModel(IDatabaseService data,
                                           IShellService shell) : BaseViewModel
{
    public ObservableRangeCollection<Bowler> AllBowlers { get; set; } = [];

    protected bool IsInitialized { get; set; }

    [RelayCommand]
    public async Task InitializeDataAsync()
    {
        if (IsInitialized)
        {
            return;
        }

        var bowlers = await data.GetAllBowlers();
        AllBowlers.Clear();
        AllBowlers.AddRange(bowlers);
        IsInitialized = true;
    }

    [RelayCommand]
    private async Task ShowAddUpdateBowlerViewAsync(Bowler bowler) =>
        await shell.GoToPageWithDataAsync(nameof(AddBowlerPage), bowler);
}
