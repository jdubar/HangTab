using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using HangTab.Enums;
using HangTab.Services;
using HangTab.ViewModels.Base;

namespace HangTab.ViewModels;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a ViewModel for the UI and does not require unit tests.")]
public partial class SettingsViewModel(
    ISettingsService settingsService,
    IThemeService themeService,
    DataManagerViewModel dataManager) : ViewModelBase
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
    private async Task DeleteAllDataAsync() => await dataManager.DeleteAllDataAsync();

    [RelayCommand]
    private async Task StartNewSeasonAsync() => await dataManager.StartNewSeasonAsync();

    private Task InitializeSettingsAsync()
    {
        TotalSeasonWeeks = settingsService.TotalSeasonWeeks;
        DarkThemeEnabled = settingsService.Theme == (int)Theme.Dark;
        return Task.CompletedTask;
    }
}
