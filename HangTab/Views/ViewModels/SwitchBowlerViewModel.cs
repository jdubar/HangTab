using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using HangTab.Models;
using HangTab.Services;

using MvvmHelpers;

namespace HangTab.Views.ViewModels;

[QueryProperty(nameof(Bowler), nameof(Bowler))]
public partial class SwitchBowlerViewModel(IDatabaseService data, IShellService shell) : BaseViewModel
{
    [ObservableProperty]
    private Bowler _bowler;

    [ObservableProperty]
    private Bowler _selectedBowler;

    public ObservableRangeCollection<Bowler> SwitchBowlers { get; set; } = [];

    [RelayCommand]
    public async Task InitializeDataAsync()
    {
        await ExecuteAsync(async () =>
        {
            var bowlers = await data.GetFilteredBowlers(b => b.Id != Bowler.Id && b.IsHidden);
            SwitchBowlers.Clear();

            if (bowlers.Any())
            {
                SwitchBowlers.AddRange(bowlers);
            }
        }, "Loading bowlers...");
    }

    [RelayCommand]
    private async Task SwitchBowlerAsync()
    {
        if (!await ChangeBowlerHiddenStateAsync(Bowler)
            || !await ChangeBowlerHiddenStateAsync(SelectedBowler))
        {
            await shell.DisplayAlert("Update Error", "Error updating bowler state", "OK");
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
}
