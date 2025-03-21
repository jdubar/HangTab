using HangTab.Constants;
using HangTab.Views;

namespace HangTab;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(Routes.BowlerAdd, typeof(BowlerAddEditPage));
        Routing.RegisterRoute(Routes.BowlerEdit, typeof(BowlerAddEditPage));
        Routing.RegisterRoute(Routes.BowlerSwitch, typeof(BowlerSwitchPage));
    }
}