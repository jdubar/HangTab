using CommunityToolkit.Mvvm.ComponentModel;

namespace HangTab.Models.Wrappers;
public class SeasonSettingsWrapper : ObservableObject
{
    public SeasonSettingsWrapper(SeasonSettings settings)
    {
        if (settings is null)
        {
            return;
        }

        Id = settings.Id;
        CurrentSeasonWeek = settings.CurrentSeasonWeek;
        TotalSeasonWeeks = settings.TotalSeasonWeeks;
    }

    public int Id { get; set; }
    public int CurrentSeasonWeek { get; set; }
    public int TotalSeasonWeeks { get; set; }
}
