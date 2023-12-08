using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using HangTab.Data;
using HangTab.Models;
using HangTab.Views;

using System.Collections.ObjectModel;

namespace HangTab.ViewModels;
public partial class BowlerViewModel : BaseViewModel
{
    private readonly DatabaseContext _context;

    public BowlerViewModel(DatabaseContext context)
    {
        _context = context;
    }

    [ObservableProperty]
    private ObservableCollection<BowlerWeek> _bowlers;

    [ObservableProperty]
    private ObservableCollection<Bowler> _hiddenBowlers;

    [ObservableProperty]
    private BowlerWeek _workingBowlerWeek = new();

    [ObservableProperty]
    private Bowler _selectedBowler;

    [RelayCommand]
    private async Task AddUpdateBowlerAsync(BowlerWeek? bowler)
    {
        SetWorkingBowlerWeekCommand.Execute(bowler);
        await Shell.Current.GoToAsync(nameof(AddBowlerPage), true);
    }

    [RelayCommand]
    private async Task DropAllTablesAsync()
    {
        if (await Shell.Current.DisplayAlert("Delete", "Are you sure you want to delete ALL data?", "Yes", "No"))
        {
            await ExecuteAsync(async () =>
            {
                await _context.DropTableAsync<Bowler>();
                await _context.DropTableAsync<Week>();
                Bowlers.Clear();
                HiddenBowlers.Clear();
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
                if (await _context.DeleteItemByIdAsync<Bowler>(id))
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
    private async Task HangBowlerAsync(BowlerWeek? bowler)
    {
        SetWorkingBowlerWeekCommand.Execute(bowler);
        await ExecuteAsync(async () =>
        {
            WorkingBowlerWeek.Bowler.TotalHangings++;
            await _context.UpdateItemAsync(WorkingBowlerWeek.Bowler);

            RefreshBowler();
        }, "Hanging bowler...");
    }

    [RelayCommand]
    public async Task LoadHiddenBowlersAsync()
    {
        await ExecuteAsync(async () =>
        {
            HiddenBowlers ??= new ObservableCollection<Bowler>();
            var bowlers = await _context.GetFilteredAsync<Bowler>(b => b.IsHidden);
            if (bowlers is not null && bowlers.Any())
            {
                var oldItemsIndexes = HiddenBowlers?.Select((bowler, index) => new { Bowler = bowler, Index = index });
                bowlers.ToList().ForEach(bowler =>
                {
                    var oldItem = oldItemsIndexes?.FirstOrDefault(i => i.Bowler.Id == bowler.Id);
                    if (oldItem == null)
                    {
                        HiddenBowlers.Add(bowler);
                    }
                    else
                    {
                        HiddenBowlers[oldItem.Index] = bowler;
                    }
                });
            }
        });
    }

    [RelayCommand]
    public async Task LoadMainBowlersAsync()
    {
        await ExecuteAsync(async () =>
        {
            SetWorkingBowlerWeekCommand.Execute(new());
            Bowlers ??= new ObservableCollection<BowlerWeek>();
            var bowlers = await _context.GetFilteredAsync<Bowler>(b => !b.IsHidden);
            var weeks = await _context.GetAllAsync<Week>();
            if (bowlers is not null && bowlers.Any())
            {
                Bowlers.Clear();

                foreach (var bowler in bowlers)
                {
                    var week = weeks.FirstOrDefault(w => w.BowlerId == bowler.Id);
                    week ??= new Week()
                    {
                        BowlerId = bowler.Id
                    };

                    var bowlerWeek = new BowlerWeek()
                    {
                        Bowler = bowler,
                        Week = week
                    };
                    Bowlers.Add(bowlerWeek);
                }
            }
        }, "Loading bowlers...");
    }

    [RelayCommand]
    public async Task LoadAllBowlersAsync()
    {
        await ExecuteAsync(async () =>
        {
            SetWorkingBowlerWeekCommand.Execute(new());
            Bowlers ??= new ObservableCollection<BowlerWeek>();
            var bowlers = await _context.GetAllAsync<Bowler>();
            var weeks = await _context.GetAllAsync<Week>();
            if (bowlers is not null && bowlers.Any())
            {
                Bowlers.Clear();

                foreach (var bowler in bowlers)
                {
                    var week = weeks.FirstOrDefault(w => w.BowlerId == bowler.Id);
                    week ??= new Week()
                    {
                        BowlerId = bowler.Id
                    };

                    var bowlerWeek = new BowlerWeek()
                    {
                        Bowler = bowler,
                        Week = week
                    };
                    Bowlers.Add(bowlerWeek);
                }
                Bowlers = new ObservableCollection<BowlerWeek>(Bowlers.OrderBy(i => i.Bowler.FullName));
            }
        }, "Loading bowlers...");
    }

    [RelayCommand]
    private async Task SaveBowlerAsync()
    {
        if (WorkingBowlerWeek.Bowler is null)
        {
            return;
        }

        var (isValid, errorMessage) = WorkingBowlerWeek.Bowler.ValidateEmptyFields();
        if (!isValid)
        {
            await Shell.Current.DisplayAlert("Validation Error", errorMessage, "Ok");
            return;
        }

        var busyText = "Updating bowler...";
        if (WorkingBowlerWeek.Bowler.Id == 0)
        {
            busyText = "Creating bowler...";
            var find = await _context.GetFilteredAsync<Bowler>(b => b.FirstName == WorkingBowlerWeek.Bowler.FirstName && b.LastName == WorkingBowlerWeek.Bowler.LastName);
            if (find.Any())
            {
                await Shell.Current.DisplayAlert("Validation Error", "This bowler already exists", "Ok");
                return;
            }
        }

        await ExecuteAsync(async () =>
        {
            if (WorkingBowlerWeek.Bowler.Id == 0)
            {
                await _context.AddItemAsync(WorkingBowlerWeek.Bowler);
                Bowlers.Add(WorkingBowlerWeek);
            }
            else
            {
                await _context.UpdateItemAsync(WorkingBowlerWeek.Bowler);
                RefreshBowler();
            }
            await Shell.Current.GoToAsync("..", true);
        }, busyText);
    }

    [RelayCommand]
    private void SetWorkingBowlerWeek(BowlerWeek? bowler) =>
    WorkingBowlerWeek = bowler ?? new();

    [RelayCommand]
    private async Task SwitchBowlerAsync(BowlerWeek? bowler)
    {
        SetWorkingBowlerWeekCommand.Execute(bowler);
        await Shell.Current.GoToAsync(nameof(SwitchBowlerPage), true);
    }

    private void RefreshBowler()
    {
        var bowlerCopy = WorkingBowlerWeek.Clone();

        var index = Bowlers.IndexOf(WorkingBowlerWeek);
        Bowlers.RemoveAt(index);

        Bowlers.Insert(index, bowlerCopy);

        SetWorkingBowlerWeekCommand.Execute(new());
    }
}
