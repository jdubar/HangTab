using HangTab.ViewModels;
using HangTab.Views.Base;

namespace HangTab.Views;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We will not test the view code behind. There's no logic to test.")]
public partial class PersonListOverviewPage : ContentPageBase
{
	public PersonListOverviewPage(PersonListOverviewViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}