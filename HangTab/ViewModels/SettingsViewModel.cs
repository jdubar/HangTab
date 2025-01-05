using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using HangTab.Services;
using HangTab.ViewModels.Base;

namespace HangTab.ViewModels;
public partial class SettingsViewModel(
    IDatabaseService databaseService,
    ISettingsService settingsService,
    IDialogService dialogService) : ViewModelBase
{
    [ObservableProperty]
    private int _totalSeasonWeeks = settingsService.TotalSeasonWeeks;

    [RelayCommand]
    private void UpdateSeasonSettings()
    {
        if (TotalSeasonWeeks > 0)
        {
            settingsService.TotalSeasonWeeks = TotalSeasonWeeks;
        }
    }

    [RelayCommand]
    private async Task DropAllTablesAsync()
    {
        if (await dialogService.Ask("Delete", "Are you sure you want to delete ALL databases?"))
        {
            if (await databaseService.DropAllTables())
            {
                await dialogService.ToastAsync("All databases have been deleted");
            }
            else
            {
                await dialogService.AlertAsync("Critical Error", "Error occurred while deleting the databases!", "Ok");
            }
        }
    }

    [RelayCommand]
    private async Task ResetAllHangingsAsync()
    {
        if (await dialogService.Ask("Reset", "Are you sure you want to start a new season and reset all bowler hangings?"))
        {
            //if (await databaseService.ResetHangings())
            //{
            //    await dialogService.ToastAsync("New season has been started");
            //}
            //else
            //{
            //    await dialogService.AlertAsync("Critical Error", "Error occurred while resetting databaseService!", "Ok");
            //}
        }
    }
}