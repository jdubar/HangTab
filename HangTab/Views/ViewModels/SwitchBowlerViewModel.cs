using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using HangTab.Models;
using HangTab.Services;

using System.Collections.ObjectModel;

namespace HangTab.Views.ViewModels;

[QueryProperty(nameof(Bowler), nameof(Bowler))]
public partial class SwitchBowlerViewModel(IDatabaseService data, IShellService shell) : BaseViewModel
{
    [ObservableProperty]
    private Bowler _bowler;

    [ObservableProperty]
    private ObservableCollection<Bowler> _switchBowlers;

    [ObservableProperty]
    private Bowler _selectedBowler;

    public async Task InitializeDataAsync() =>
        await ExecuteAsync(SetSwitchBowlersListAsync, "Loading bowlers...");

    [RelayCommand]
    private async Task SwitchBowlerAsync()
    {
        if (!await ChangeBowlerHiddenStateAsync(Bowler)
            || !await ChangeBowlerHiddenStateAsync(SelectedBowler))
        {
            await shell.DisplayAlert("Update Error", "Error updating bowler state", "Ok");
        }
        else
        {
            await shell.ReturnToPage();
        }
    }

    private async Task<bool> ChangeBowlerHiddenStateAsync(Bowler bowler)
    {
        bowler.IsHidden = !bowler.IsHidden;
        return await data.UpdateBowler(bowler);
    }

    private async Task SetSwitchBowlersListAsync()
    {
        var bowlers = await data.GetFilteredBowlers(b => b.Id != Bowler.Id && b.IsHidden);
        SwitchBowlers = bowlers is not null && bowlers.Any()
            ? new ObservableCollection<Bowler>(bowlers)
            : ([]);
    }
}
