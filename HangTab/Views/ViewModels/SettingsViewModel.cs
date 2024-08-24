using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using HangTab.Models;
using HangTab.Services;

namespace HangTab.Views.ViewModels;
public partial class SettingsViewModel(IDatabaseService data,
                                       IShellService shell) : BaseViewModel
{
    [ObservableProperty]
    private SeasonSettings _seasonSettings;

    [RelayCommand]
    private async Task InitializeDataAsync()
    {
        SeasonSettings = await data.GetSeasonSettings();
    }

    [RelayCommand]
    private async Task UpdateSeasonSettingsAsync()
        => await data.UpdateSeasonSettings(SeasonSettings);

    [RelayCommand]
    private async Task DropAllTablesAsync()
    {
        if (await shell.DisplayPromptAsync("Delete", "Are you sure you want to delete ALL data?", "Yes", "No"))
        {
            await ExecuteAsync(data.DropAllTables, "Clearing all data...");
        }
    }

    [RelayCommand]
    private async Task ResetAllHangingsAsync()
    {
        if (await shell.DisplayPromptAsync("Reset", "Are you sure you want to start a new season and reset all bowler hangings?", "Yes", "No"))
        {
            await ExecuteAsync(async () =>
            {
                if (!await data.ResetHangings())
                {
                    await shell.DisplayAlertAsync("Critical Error", "Error occurred while resetting data!", "Ok");
                }
            }, "Resetting hangings...");
        }
    }
}
