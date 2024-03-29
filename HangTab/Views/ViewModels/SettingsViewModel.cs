﻿using CommunityToolkit.Mvvm.Input;

using HangTab.Services;

namespace HangTab.Views.ViewModels;
public partial class SettingsViewModel(IDatabaseService data,
                                       IShellService shell) : BaseViewModel
{
    [RelayCommand]
    private async Task DropAllTablesAsync()
    {
        if (await shell.DisplayPrompt("Delete", "Are you sure you want to delete ALL data?", "Yes", "No"))
        {
            await ExecuteAsync(data.DropAllTables, "Clearing all data...");
        }
    }

    [RelayCommand]
    private async Task ResetAllHangingsAsync()
    {
        if (await shell.DisplayPrompt("Reset", "Are you sure you want to reset all bowler hangings?", "Yes", "No"))
        {
            await ExecuteAsync(async () =>
            {
                if (!await data.ResetHangings())
                {
                    await shell.DisplayAlert("Critical Error", "Error occurred while restting data!", "Ok");
                }
            }, "Resetting hangings...");
        }
    }
}
