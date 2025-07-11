namespace HangTab.Services;
public interface ISettingsService
{
    int CurrentWeekPrimaryKey { get; set; }
    int TotalSeasonWeeks { get; set; }
    int Theme { get; set; }
    bool SeasonComplete { get; set; }
}
