using HangTab.Constants;
using HangTab.Views;

namespace HangTab;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We will not test the view code behind. There's no logic to test.")]
public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(Routes.BowlerAdd, typeof(PersonAddEditPage));
        Routing.RegisterRoute(Routes.BowlerEdit, typeof(PersonAddEditPage));
        Routing.RegisterRoute(Routes.BowlerSelectSub, typeof(BowlerSelectSubPage));
        Routing.RegisterRoute(Routes.PersonAdd, typeof(PersonAddEditPage));
        Routing.RegisterRoute(Routes.PersonEdit, typeof(PersonAddEditPage));
        Routing.RegisterRoute(Routes.WeekDetails, typeof(WeekDetailsPage));
    }
}
