namespace HangTab.Services.Impl;
public sealed class SettingsService(IPreferences preferences) : ISettingsService
{
    public decimal CostPerHang
    {
        get => preferences.Get(nameof(CostPerHang), 0.25m);
        set => preferences.Set(nameof(CostPerHang), value);
    }

    public int CurrentSeasonWeek
    {
        get => preferences.Get(nameof(CurrentSeasonWeek), 1);
        set => preferences.Set(nameof(CurrentSeasonWeek), value);
    }

    public int TotalSeasonWeeks
    {
        get => preferences.Get(nameof(TotalSeasonWeeks), 34);
        set => preferences.Set(nameof(TotalSeasonWeeks), value);
    }
}