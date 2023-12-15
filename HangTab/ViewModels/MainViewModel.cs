using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using HangTab.Data;
using HangTab.Models;
using HangTab.Views;

using System.Collections.ObjectModel;

namespace HangTab.ViewModels;
public partial class MainViewModel(DatabaseContext context) : BaseViewModel
{
    [ObservableProperty]
    private ObservableCollection<BowlerViewModel> _bowlers;

    [ObservableProperty]
    private ObservableCollection<Bowler> _switchBowlers;

    [ObservableProperty]
    private BowlerViewModel _workingBowlerViewModel;

    [ObservableProperty]
    private BusRideViewModel _workingBusRideViewModel;

    [ObservableProperty]
    private Bowler _selectedBowler;

    [ObservableProperty]
    private int _busRides;

    [ObservableProperty]
    private int _totalBusRides;

    private int WorkingWeek { get; set; }

    [RelayCommand]
    private async Task AddUpdateBowlerAsync(BowlerViewModel bowler)
    {
        SetWorkingBowlerViewModelCommand.Execute(bowler);
        await Shell.Current.GoToAsync(nameof(AddBowlerPage), true);
    }

    [RelayCommand]
    private async Task BusRideAsync()
    {
        await ExecuteAsync(async () =>
        {
            WorkingBusRideViewModel.BusRide.TotalBusRides++;
            WorkingBusRideViewModel.BusRideWeek.BusRides++;

            _ = await context.UpdateItemAsync(WorkingBusRideViewModel.BusRide);
            var busRides = await context.GetFilteredAsync<BusRideWeek>(b => b.WeekNumber == WorkingWeek);
            _ = busRides is not null && busRides.Any()
                ? await context.UpdateItemAsync(WorkingBusRideViewModel.BusRideWeek)
                : await context.AddItemAsync(WorkingBusRideViewModel.BusRideWeek);

            BusRides = WorkingBusRideViewModel.BusRideWeek.BusRides;
            TotalBusRides = WorkingBusRideViewModel.BusRide.TotalBusRides;
        }, "Bus Ride!!!");
    }

    [RelayCommand]
    private async Task DropAllTablesAsync()
    {
        if (await Shell.Current.DisplayAlert("Delete", "Are you sure you want to delete ALL data?", "Yes", "No"))
        {
            SetWorkingBowlerViewModelCommand.Execute(new());
            await ExecuteAsync(async () =>
            {
                await context.DropTableAsync<Bowler>();
                await context.DropTableAsync<BowlerWeek>();
                await context.DropTableAsync<BusRide>();
                Bowlers.Clear();
                SwitchBowlers.Clear();
            });
        }
    }

    [RelayCommand]
    private async Task DeleteBowlerAsync(int id)
    {
        if (await Shell.Current.DisplayAlert("Delete", "Are you sure you want to delete this bowler?", "Yes", "No"))
        {
            await ExecuteAsync(async () =>
            {
                if (await context.DeleteItemByIdAsync<Bowler>(id))
                {
                    var bowler = Bowlers.FirstOrDefault(b => b.Bowler.Id == id);
                    Bowlers.Remove(bowler);
                }
                else
                {
                    await Shell.Current.DisplayAlert("Delete Error", "Bowler was not deleted", "Ok");
                }
            }, "Deleting bowler...");
        }
    }

    [RelayCommand]
    private async Task HangBowlerAsync(BowlerViewModel mainViewModel)
    {
        SetWorkingBowlerViewModelCommand.Execute(mainViewModel);
        await ExecuteAsync(async () =>
        {
            WorkingBowlerViewModel.Bowler.TotalHangings++;
            WorkingBowlerViewModel.BowlerWeek.Hangings++;
            WorkingBowlerViewModel.BowlerWeek.WeekNumber = WorkingWeek;

            _ = await context.UpdateItemAsync(WorkingBowlerViewModel.Bowler);

            var weeks = await context.GetFilteredAsync<BowlerWeek>(w => w.WeekNumber == WorkingWeek && w.BowlerId == WorkingBowlerViewModel.Bowler.Id);
            _ = weeks is not null && weeks.Any()
                ? await context.UpdateItemAsync(WorkingBowlerViewModel.BowlerWeek)
                : await context.AddItemAsync(WorkingBowlerViewModel.BowlerWeek);
        }, "Hanging bowler...");
    }

    [RelayCommand]
    public async Task LoadAllBowlersAsync()
    {
        await ExecuteAsync(async () =>
        {
            SetWorkingBowlerViewModelCommand.Execute(new());
            Bowlers ??= [];
            var bowlers = await context.GetAllAsync<Bowler>();
            var weeks = await context.GetAllAsync<BowlerWeek>();
            if (bowlers is not null && bowlers.Any())
            {
                LoadBowlers(bowlers, weeks);
                Bowlers = new ObservableCollection<BowlerViewModel>(Bowlers.OrderBy(i => i.Bowler.FullName));
            }
        }, "Loading bowlers...");
    }

    [RelayCommand]
    public async Task LoadSwitchBowlersAsync()
    {
        await ExecuteAsync(async () =>
        {
            SwitchBowlers ??= [];
            var bowlers = await context.GetFilteredAsync<Bowler>(b => b.Id != WorkingBowlerViewModel.Bowler.Id && !b.IsHidden);
            var weeks = await context.GetAllAsync<BowlerWeek>();
            if (bowlers is not null && bowlers.Any())
            {
                LoadBowlers(bowlers, weeks);
            }
        });
    }

    [RelayCommand]
    public async Task LoadMainBowlersAsync()
    {
        await ExecuteAsync(async () =>
        {
            await SetWorkingWeekAsync(context);

            await LoadBusRidesAsync();
            BusRides = WorkingBusRideViewModel.BusRideWeek.BusRides;
            TotalBusRides = WorkingBusRideViewModel.BusRide.TotalBusRides;

            SetWorkingBowlerViewModelCommand.Execute(new());
            Bowlers ??= [];
            var bowlers = await context.GetFilteredAsync<Bowler>(b => !b.IsHidden);
            var weeks = await context.GetFilteredAsync<BowlerWeek>(week => week.WeekNumber == WorkingWeek);
            if (bowlers is not null && bowlers.Any())
            {
                LoadBowlers(bowlers, weeks);
            }
        }, "Loading bowlers...");
    }

    [RelayCommand]
    public async Task LoadBusRidesAsync()
    {
        await ExecuteAsync(async () =>
        {
            SetWorkingBusRideViewModelCommand.Execute(new());
            var rides = await context.GetAllAsync<BusRide>();
            if (rides.Any())
            {
                WorkingBusRideViewModel.BusRide = rides.Last();
            }
            else
            {
                _ = await context.AddItemAsync(WorkingBusRideViewModel.BusRide);
            }
            var weeks = await context.GetFilteredAsync<BusRideWeek>(week => week.WeekNumber == WorkingWeek);
            if (weeks.Any())
            {
                WorkingBusRideViewModel.BusRideWeek = weeks.Last();
            }
            else
            {
                WorkingBusRideViewModel.BusRideWeek.BusRideId = WorkingBusRideViewModel.BusRide.Id;
                WorkingBusRideViewModel.BusRideWeek.WeekNumber = WorkingWeek;
                _ = await context.AddItemAsync(WorkingBusRideViewModel.BusRideWeek);
            }
        });
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
            var find = await context.GetFilteredAsync<Bowler>(b => b.FirstName == WorkingBowlerViewModel.Bowler.FirstName && b.LastName == WorkingBowlerViewModel.Bowler.LastName);
            if (find.Any())
            {
                await Shell.Current.DisplayAlert("Validation Error", "This bowler already exists", "Ok");
                return;
            }
        }

        await ExecuteAsync(async () =>
        {
            if (WorkingBowlerViewModel.Bowler.Id == 0)
            {
                _ = await context.AddItemAsync(WorkingBowlerViewModel.Bowler);
                Bowlers.Add(WorkingBowlerViewModel);
            }
            else
            {
                _ = await context.UpdateItemAsync(WorkingBowlerViewModel.Bowler);
            }
            await Shell.Current.GoToAsync("..", true);
        }, busyText);
    }

    [RelayCommand]
    private void SetWorkingBowlerViewModel(BowlerViewModel viewModel) =>
        WorkingBowlerViewModel = viewModel ?? new();

    [RelayCommand]
    private void SetWorkingBusRideViewModel(BusRideViewModel viewModel) =>
        WorkingBusRideViewModel = viewModel ?? new();

    [RelayCommand]
    private async Task StartNewWeekAsync()
    {
        await ExecuteAsync(async () =>
        {
            WorkingWeek++;
            await LoadMainBowlersAsync();
        }, "Starting new week...");
    }

    [RelayCommand]
    private async Task SwitchBowlerAsync(BowlerViewModel bowler)
    {
        SetWorkingBowlerViewModelCommand.Execute(bowler);
        await Shell.Current.GoToAsync(nameof(SwitchBowlerPage), true);
    }

    private void LoadBowlers(IEnumerable<Bowler> bowlers, IEnumerable<BowlerWeek> weeks)
    {
        Bowlers.Clear();

        foreach (var bowler in bowlers)
        {
            var week = weeks.FirstOrDefault(w => w.BowlerId == bowler.Id);
            week ??= new BowlerWeek()
            {
                WeekNumber = WorkingWeek,
                BowlerId = bowler.Id,
                Hangings = 0
            };

            var mainViewModel = new BowlerViewModel()
            {
                Bowler = bowler,
                BowlerWeek = week
            };
            Bowlers.Add(mainViewModel);
        }
    }

    private async Task SetWorkingWeekAsync(DatabaseContext context)
    {
        if (WorkingWeek == 0)
        {
            var allWeeks = await context.GetAllAsync<BowlerWeek>();
            WorkingWeek = allWeeks is not null && allWeeks.Any()
                ? allWeeks.OrderBy(w => w.WeekNumber).Last().WeekNumber
                : 1;
        }
    }
}
