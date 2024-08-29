using CommunityToolkit.Mvvm.Input;

using HangTab.Models;
using HangTab.Services;

using MvvmHelpers;

namespace HangTab.Views.ViewModels;
public partial class ManageBowlerViewModel(IDatabaseService data,
                                           IShellService shell) : BaseViewModel
{
    public ObservableRangeCollection<Bowler> AllBowlers { get; } = [];

    [RelayCommand]
    private async Task InitializeDataAsync()
    {
        AllBowlers.ReplaceRange(await data.GetAllBowlers());
    }

    [RelayCommand]
    private async Task ShowAddUpdateBowlerViewAsync(Bowler bowler) =>
        await shell.GoToPageWithDataAsync(nameof(AddBowlerPage), bowler);
}
