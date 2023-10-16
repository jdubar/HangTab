using HangTab.Views;

namespace HangTab
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(AddBowlerPage), typeof(AddBowlerPage));
        }
    }
}