using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using HangTab.Data;
using HangTab.Models;
using HangTab.Views;

using System.Collections.ObjectModel;

namespace HangTab.ViewModels;
public partial class BowlersViewModel : BaseViewModel
{
    private readonly DatabaseContext _context;

    public BowlersViewModel(DatabaseContext context)
    {
        _context = context;
    }

    [ObservableProperty]
    private ObservableCollection<BowlerWeek> _bowlers;

    [ObservableProperty]
    private BowlerWeek _operatingBowler = new();

    [RelayCommand]
    private async Task AddUpdateBowlerAsync(BowlerWeek? bowler)
    {
        SetOperatingBowlerCommand.Execute(bowler);
        await Shell.Current.GoToAsync(nameof(AddBowlerPage), true);
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
        SetOperatingBowlerCommand.Execute(bowler);
        await ExecuteAsync(async () =>
        {
            OperatingBowler.Bowler.TotalHangings++;
            await _context.UpdateItemAsync(OperatingBowler.Bowler);

            RefreshBowler();
        }, "Hanging bowler...");
    }

    [RelayCommand]
    public async Task LoadMainBowlersAsync()
    {
        await ExecuteAsync(async () =>
        {
            SetOperatingBowlerCommand.Execute(new());
            Bowlers ??= new ObservableCollection<BowlerWeek>();
            var bowlers = await _context.GetFilteredAsync<Bowler>(b => !b.IsSub);
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
    private async Task SaveBowlerAsync()
    {
        if (OperatingBowler.Bowler is null)
        {
            return;
        }

        var (isValid, errorMessage) = OperatingBowler.Bowler.Validate();
        if (!isValid)
        {
            await Shell.Current.DisplayAlert("Validation Error", errorMessage, "Ok");
            return;
        }

        if (Bowlers.FirstOrDefault(b => b.Bowler.FirstName == OperatingBowler.Bowler.FirstName
                                        && b.Bowler.LastName == OperatingBowler.Bowler.LastName) is not null
            && OperatingBowler.Bowler.Id == 0)
        {
            await Shell.Current.DisplayAlert("Validation Error", "This bowler already exists", "Ok");
            return;
        }

        var busyText = OperatingBowler.Bowler.Id == 0
            ? "Creating bowler..."
            : "Updating bowler...";

        await ExecuteAsync(async () =>
        {
            if (OperatingBowler.Bowler.Id == 0)
            {
                await _context.AddItemAsync(OperatingBowler.Bowler);
                Bowlers.Add(OperatingBowler);
            }
            else
            {
                await _context.UpdateItemAsync(OperatingBowler.Bowler);

                RefreshBowler();
            }
            await Shell.Current.GoToAsync("..", true);
        }, busyText);
    }

    [RelayCommand]
    private void SetOperatingBowler(BowlerWeek? bowler) =>
        OperatingBowler = bowler ?? new();

    private void RefreshBowler()
    {
        var bowlerCopy = OperatingBowler.Clone();

        var index = Bowlers.IndexOf(OperatingBowler);
        Bowlers.RemoveAt(index);

        Bowlers.Insert(index, bowlerCopy);

        SetOperatingBowlerCommand.Execute(new());
    }
}
