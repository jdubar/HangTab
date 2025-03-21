namespace HangTab.Services.Impl;
public class SettingsService(IPreferences preferences) : ISettingsService
{
    public decimal CostPerHang
    {
        get => preferences.Get(nameof(CostPerHang), 0.25m);
        set => preferences.Set(nameof(CostPerHang), value);
    }

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
