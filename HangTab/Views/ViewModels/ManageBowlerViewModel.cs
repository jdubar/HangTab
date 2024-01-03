using CommunityToolkit.Mvvm.Input;

using HangTab.Models;
using HangTab.Services;

using MvvmHelpers;

namespace HangTab.Views.ViewModels;
public partial class ManageBowlerViewModel(IDatabaseService data,
                                           IShellService shell) : BaseViewModel
{
    public ObservableRangeCollection<Bowler> AllBowlers { get; set; } = [];

    [RelayCommand]
    public async Task InitializeData()
    {
        await ExecuteAsync(async () =>
        {
            var bowlers = await data.GetAllBowlers();
            if (AllBowlers.Count > 0)
            {
                AllBowlers.Clear();
            }

            if (bowlers.Any())
            {
                AllBowlers.AddRange(bowlers);
            }
        }, "");
    }

    [RelayCommand]
    private async Task ShowAddUpdateBowlerViewAsync(Bowler bowler) =>
        await shell.GoToPageWithData(nameof(AddBowlerPage), bowler);
}
