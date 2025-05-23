using HangTab.ViewModels;
using HangTab.Views.Base;

namespace HangTab.Views;

public partial class BowlerOverviewPage : ContentPageBase
{
	public BowlerOverviewPage(BowlerListOverviewViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }

    //private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    //{

    //}
}