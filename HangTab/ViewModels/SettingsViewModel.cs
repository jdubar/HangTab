using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using HangTab.Enums;
using HangTab.Messages;
using HangTab.Services;
using HangTab.ViewModels.Base;

namespace HangTab.ViewModels;
public partial class SettingsViewModel(
    IDatabaseService databaseService,
    IDialogService dialogService,
    ISettingsService settingsService,
    IThemeService themeService) : ViewModelBase
{
    [ObservableProperty]
    private int _totalSeasonWeeks;
    partial void OnTotalSeasonWeeksChanged(int value) => settingsService.TotalSeasonWeeks = value;

    
    [ObservableProperty]
    private bool _darkThemeEnabled;
    partial void OnDarkThemeEnabledChanged(bool value)
    {
        if (value)
        {
            settingsService.Theme = (int)Theme.Dark;
            themeService.SetDarkTheme();
        }
        else
        {
            settingsService.Theme = (int)Theme.Light;
            themeService.SetLightTheme();
        }
    }

    public override async Task LoadAsync()
    {
        await Loading(
            () =>
            {
                TotalSeasonWeeks = settingsService.TotalSeasonWeeks;
                DarkThemeEnabled = settingsService.Theme == (int)Theme.Dark;
                return Task.CompletedTask;
            });

    }

    [RelayCommand]
    private async Task DropAllTablesAsync()
    {
        if (await dialogService.Ask("Delete", "Are you sure you want to delete ALL databases?"))
        {
            if (await databaseService.DropAllTables())
            {
                WeakReferenceMessenger.Default.Send(new BowlerAddedOrChangedMessage());
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
        //TODO: Implement this method
        if (await dialogService.Ask("Reset", "Are you sure you want to start a new season and reset all bowler hangings?"))
        {
            WeakReferenceMessenger.Default.Send(new BowlerAddedOrChangedMessage());
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