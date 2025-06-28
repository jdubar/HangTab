using HangTab.ViewModels;
using HangTab.Views.Base;

namespace HangTab.Views;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We will not test the view code behind. There's no logic to test.")]
public partial class BowlerOverviewPage : ContentPageBase
{
	public BowlerOverviewPage(PersonOverviewViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }

    //private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    //{

    //}
}