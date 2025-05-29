using HangTab.Constants;
using HangTab.Views;

namespace HangTab;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(Routes.BowlerAdd, typeof(PersonAddEditPage));
        Routing.RegisterRoute(Routes.BowlerEdit, typeof(PersonAddEditPage));
        Routing.RegisterRoute(Routes.BowlerSelectSub, typeof(BowlerSelectSubPage));
    }
}