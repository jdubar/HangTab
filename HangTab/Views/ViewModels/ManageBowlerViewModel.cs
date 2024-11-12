using CommunityToolkit.Mvvm.Input;

using MvvmHelpers;

namespace HangTab.Views.ViewModels;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We won't test UI code-behind.")]
public partial class ManageBowlerViewModel(IBowlerService bowlerService,
                                           IShellService shell) : BaseViewModel
{
    public ObservableRangeCollection<Bowler> AllBowlers { get; } = [];

    [RelayCommand]
    private async Task InitializeDataAsync()
    {
        AllBowlers.ReplaceRange(await bowlerService.GetAll());
    }

    [RelayCommand]
    private async Task ShowAddUpdateBowlerViewAsync(Bowler bowler) => await shell.GoToPageWithDataAsync(nameof(AddBowlerPage), bowler);
}
