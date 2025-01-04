using HangTab.Views;

namespace HangTab;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("bowler/add", typeof(BowlerAddEditPage));
        Routing.RegisterRoute("bowler/edit", typeof(BowlerAddEditPage));
    }
}