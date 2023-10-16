using HangTab.Data;
using HangTab.Models;
using HangTab.Views;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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
    private ObservableCollection<Bowler> _bowlers;

    [ObservableProperty]
    private Bowler _operatingBowler = new();

    [RelayCommand]
    private async Task DeleteBowlerAsync(int id)
    {
        if (await Shell.Current.DisplayAlert("Delete", "Are you sure you want to delete this bowler?", "Yes", "No"))
        {
            await ExecuteAsync(async () =>
            {
                if (await _context.DeleteItemByIdAsync<Bowler>(id))
                {
                    var bowler = Bowlers.FirstOrDefault(b => b.Id == id);
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
    public async Task LoadBowlersAsync()
    {
        await ExecuteAsync(async () =>
        {
            SetOperatingBowlerCommand.Execute(new());
            Bowlers ??= new ObservableCollection<Bowler>();
            var bowlers = await _context.GetAllAsync<Bowler>();
            if (bowlers is not null && bowlers.Any())
            {
                Bowlers.Clear();

                foreach (var bowler in bowlers)
                {
                    Bowlers.Add(bowler);
                }
            }
        }, "Loading bowlers...");
    }

    [RelayCommand]
    private async Task AddUpdateBowlerAsync(Bowler? bowler)
    {
        SetOperatingBowlerCommand.Execute(bowler);
        await Shell.Current.GoToAsync(nameof(AddBowlerPage), true);
    }

    [RelayCommand]
    private async Task SaveBowlerAsync()
    {
        if (OperatingBowler is null)
        {
            return;
        }

        var (isValid, errorMessage) = OperatingBowler.Validate();
        if (!isValid)
        {
            await Shell.Current.DisplayAlert("Validation Error", errorMessage, "Ok");
            return;
        }

        if (Bowlers.FirstOrDefault(b => b.FirstName == OperatingBowler.FirstName
                                        && b.LastName == OperatingBowler.LastName) is not null)
        {
            await Shell.Current.DisplayAlert("Validation Error", "This bowler already exists", "Ok");
            return;
        }

        var busyText = OperatingBowler.Id == 0
            ? "Creating bowler..."
            : "Updating bowler...";

        await ExecuteAsync(async () =>
        {
            if (OperatingBowler.Id == 0)
            {
                await _context.AddItemAsync(OperatingBowler);
                Bowlers.Add(OperatingBowler);
            }
            else
            {
                await _context.UpdateItemAsync(OperatingBowler);

                var bowlerCopy = OperatingBowler.Clone;

                var index = Bowlers.IndexOf(OperatingBowler);
                Bowlers.RemoveAt(index);

                Bowlers.Insert(index, bowlerCopy);
            }
            SetOperatingBowlerCommand.Execute(new());
            await Shell.Current.GoToAsync("..", true);
        }, busyText);
    }

    [RelayCommand]
    private void SetOperatingBowler(Bowler? bowler) =>
        OperatingBowler = bowler ?? new();
}
