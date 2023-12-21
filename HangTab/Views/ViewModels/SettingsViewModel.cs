using CommunityToolkit.Mvvm.Input;

using HangTab.Data;
using HangTab.Models;

namespace HangTab.Views.ViewModels;
public partial class SettingsViewModel(DatabaseContext context) : BaseViewModel
{
    [RelayCommand]
    private async Task DropAllTablesAsync()
    {
        if (await Shell.Current.DisplayAlert("Delete", "Are you sure you want to delete ALL data?", "Yes", "No"))
        {
            await ExecuteAsync(async () =>
            {
                try
                {
                    _ = await context.DropTableAsync<Bowler>();
                    _ = await context.DropTableAsync<BowlerWeek>();
                    _ = await context.DropTableAsync<BusRide>();
                    _ = await context.DropTableAsync<BusRideWeek>();
                }
                catch (Exception)
                {
                    await Shell.Current.DisplayAlert("Critical Error", "Error occurred while deleting data!", "Ok");
                }
            }, "Clearing all data...");
        }
    }
}
