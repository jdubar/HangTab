using HangTab.Views;

namespace HangTab;
public partial class AppShell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(AddBowlerPage), typeof(AddBowlerPage));
        Routing.RegisterRoute(nameof(SeasonSummaryPage), typeof(SeasonSummaryPage));
        Routing.RegisterRoute(nameof(SwitchBowlerPage), typeof(SwitchBowlerPage));
        Routing.RegisterRoute(nameof(WeekDetailsPage), typeof(WeekDetailsPage));
    }
}