using HangTab.Views;

namespace HangTab;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We won't test UI code-behind.")]
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