namespace HangTab.Services;
public interface ISettingsService
{
    decimal CostPerHang { get; set; }
    int CurrentWeekPrimaryKey { get; set; }
    int TotalSeasonWeeks { get; set; }
    int Theme { get; set; }
}
