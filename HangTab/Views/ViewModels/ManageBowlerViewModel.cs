using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using HangTab.Data;
using HangTab.Models;
using HangTab.ViewModels;

using System.Collections.ObjectModel;

namespace HangTab.Views.ViewModels;
public partial class ManageBowlerViewModel(IDatabaseContext context) : BaseViewModel
{
    [ObservableProperty]
    private ObservableCollection<BowlerViewModel> _allBowlers;

    [ObservableProperty]
    private BowlerViewModel _workingBowlerViewModel;

    [RelayCommand]
    private async Task DeleteBowlerAsync(int id)
    {
        if (await Shell.Current.DisplayAlert("Delete", "Are you sure you want to delete this bowler?", "Yes", "No"))
        {
            await ExecuteAsync(async () =>
            {
                if (await context.DeleteItemByIdAsync<Bowler>(id))
                {
                    var bowler = AllBowlers.FirstOrDefault(b => b.Bowler.Id == id);
                    AllBowlers.Remove(bowler);
                }
                else
                {
                    await Shell.Current.DisplayAlert("Delete Error", "Bowler was not deleted", "Ok");
                }
            }, "Deleting bowler...");
        }
    }

    public async Task LoadAllBowlersAsync()
    {
        await ExecuteAsync(async () =>
        {
            SetWorkingBowlerViewModelCommand.Execute(new());
            AllBowlers ??= [];
            var bowlers = await context.GetAllAsync<Bowler>();
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
            await Shell.Current.DisplayAlert("Validation Error", errorMessage, "Ok");
            return;
        }

        var busyText = "Updating bowler...";
        if (WorkingBowlerViewModel.Bowler.Id == 0)
        {
            busyText = "Creating bowler...";
            var find = await context.GetFilteredAsync<Bowler>(b => b.FirstName == WorkingBowlerViewModel.Bowler.FirstName
                                                                   && b.LastName == WorkingBowlerViewModel.Bowler.LastName);
            if (find.Any())
            {
                await Shell.Current.DisplayAlert("Validation Error", "This bowler already exists", "Ok");
                return;
            }
        }

        await ExecuteAsync(async () =>
        {
            _ = WorkingBowlerViewModel.Bowler.Id == 0
                ? await context.AddItemAsync(WorkingBowlerViewModel.Bowler)
                : await context.UpdateItemAsync(WorkingBowlerViewModel.Bowler);

            await Shell.Current.GoToAsync("..", true);
        }, busyText);
    }

    [RelayCommand]
    private void SetWorkingBowlerViewModel(BowlerViewModel viewModel) =>
        WorkingBowlerViewModel = viewModel ?? new();

    [RelayCommand]
    private async Task ShowAddUpdateBowlerViewAsync(BowlerViewModel bowler)
    {
        SetWorkingBowlerViewModelCommand.Execute(bowler);
        await Shell.Current.GoToAsync(nameof(AddBowlerPage), true);
    }
}
