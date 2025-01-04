namespace HangTab.Services;
internal interface ISettingsService
{
    decimal CostPerHang { get; set; }
    int CurrentSeasonWeek { get; set; }
    int TotalSeasonWeeks { get; set; }
}
