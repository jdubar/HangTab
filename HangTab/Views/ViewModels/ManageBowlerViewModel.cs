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

    public async Task InitializeData() =>
        await ExecuteAsync(SetAllBowlersListAsync, "Loading all bowlers...");

    private async Task SetAllBowlersListAsync()
    {
        var bowlers = await data.GetAllBowlers();
        AllBowlers = bowlers is not null && bowlers.Any()
            ? new ObservableCollection<Bowler>(bowlers.OrderBy(b => b.FullName))
            : [];
    }

    [RelayCommand]
    private async Task ShowAddUpdateBowlerViewAsync(Bowler bowler) =>
        await shell.GoToPageWithData(nameof(AddBowlerPage), bowler);
}
