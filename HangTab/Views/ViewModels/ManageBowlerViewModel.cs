using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using HangTab.Services;
using HangTab.ViewModels;

using System.Collections.ObjectModel;

namespace HangTab.Views.ViewModels;
public partial class ManageBowlerViewModel(IDatabaseService data, IShellService shell) : BaseViewModel
{
    [ObservableProperty]
    private ObservableCollection<BowlerViewModel> _allBowlers;

    [ObservableProperty]
    private BowlerViewModel _workingBowlerViewModel;

    [RelayCommand]
    private async Task DeleteBowlerAsync(int id)
    {
        if (await shell.DisplayPrompt("Delete", "Are you sure you want to delete this bowler?", "Yes", "No"))
        {
            await ExecuteAsync(async () =>
            {
                if (await data.DeleteBowler(id))
                {
                    var bowler = AllBowlers.FirstOrDefault(b => b.Bowler.Id == id);
                    AllBowlers.Remove(bowler);
                    await shell.ReturnToPage();
                }
                else
                {
                    await shell.DisplayAlert("Delete Error", "Bowler was not deleted", "Ok");
                }
            }, "Deleting bowler...");
        }
    }

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
                    var viewModel = new BowlerViewModel()
                    {
                        Bowler = bowler,
                        BowlerWeek = new()
                    };
                    AllBowlers.Add(viewModel);
                }
            }
            else
            {
                AllBowlers.Clear();
            }
        }, "Loading bowlers...");
    }

    [RelayCommand]
    private async Task SaveBowlerAsync()
    {
        if (WorkingBowlerViewModel.Bowler is null)
        {
            return;
        }

        var (isValid, errorMessage) = WorkingBowlerViewModel.Bowler.ValidateEmptyFields();
        if (!isValid)
        {
            await shell.DisplayAlert("Validation Error", errorMessage, "Ok");
            return;
        }

        var busyText = "Updating bowler...";
        if (WorkingBowlerViewModel.Bowler.Id == 0)
        {
            busyText = "Creating bowler...";
            if (await data.IsBowlerExists(WorkingBowlerViewModel.Bowler))
            {
                await shell.DisplayAlert("Validation Error", "This bowler already exists", "Ok");
                return;
            }
        }

        await ExecuteAsync(async () =>
        {
            if (!(WorkingBowlerViewModel.Bowler.Id == 0
                ? await data.AddBowler(WorkingBowlerViewModel.Bowler)
                : await data.UpdateBowler(WorkingBowlerViewModel.Bowler)))
            {
                await shell.DisplayAlert("Update Error", "Unable to save bowler", "Ok");
            }
            await shell.ReturnToPage();
        }, busyText);
    }

    [RelayCommand]
    private void SetWorkingBowlerViewModel(BowlerViewModel viewModel) =>
        WorkingBowlerViewModel = viewModel ?? new();

    [RelayCommand]
    private async Task ShowAddUpdateBowlerViewAsync(BowlerViewModel bowler)
    {
        SetWorkingBowlerViewModelCommand.Execute(bowler);
        await shell.GoToPage(nameof(AddBowlerPage), true);
    }
}
