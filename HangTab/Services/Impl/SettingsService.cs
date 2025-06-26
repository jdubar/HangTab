namespace HangTab.Services.Impl;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a simple settings service with no complex logic, so we don't need to cover it extensively.")]
public class SettingsService(IPreferences preferences) : ISettingsService
{
    public int CurrentWeekPrimaryKey
    {
        get => preferences.Get(nameof(CurrentWeekPrimaryKey), 0);
        set => preferences.Set(nameof(CurrentWeekPrimaryKey), value);
    }

    public int TotalSeasonWeeks
    {
        get => preferences.Get(nameof(TotalSeasonWeeks), 34);
        set => preferences.Set(nameof(TotalSeasonWeeks), value);
    }

    public int Theme
    {
        get => preferences.Get(nameof(Theme), 0);
        set => preferences.Set(nameof(Theme), value);
    }
}
