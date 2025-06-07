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

    public override async Task LoadAsync() => await Loading(InitializeSettingsAsync);

    [RelayCommand]
    private async Task DeleteAllDataAsync()
    {
        if (!await dialogService.Ask("Delete", "Are you sure you want to delete ALL data?", "Yes", "No"))
        {
            return;
        }

        if (await databaseService.DeleteAllData())
        {
            SendSystemResetMessage();
            await dialogService.ToastAsync("All data has been deleted");
        }
        else
        {
            await dialogService.AlertAsync("Critical Error", "Error occurred while deleting the databases!", "Ok");
        }
    }

    [RelayCommand]
    private async Task StartNewSeasonAsync()
    {
        if (!await dialogService.Ask("Season Reset", "Are you ready to start a new season and reset all bowler hangings?", "Yes", "No"))
        {
            return;
        }

        if (await databaseService.DeleteSeasonData())
        {
            SendSystemResetMessage();
            await dialogService.ToastAsync("A new season has started");
        }
        else
        {
            await dialogService.AlertAsync("Critical Error", "An error occurred while starting a new season!", "Ok");
        }
    }

    private Task InitializeSettingsAsync()
    {
        TotalSeasonWeeks = settingsService.TotalSeasonWeeks;
        DarkThemeEnabled = settingsService.Theme == (int)Theme.Dark;
        return Task.CompletedTask;
    }
    
    private void SendSystemResetMessage()
    {
        settingsService.CurrentWeekPrimaryKey = 0;
        WeakReferenceMessenger.Default.Send(new SystemResetMessage());
    }
}