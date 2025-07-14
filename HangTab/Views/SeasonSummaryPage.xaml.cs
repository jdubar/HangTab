using HangTab.ViewModels;
using HangTab.Views.Base;

namespace HangTab.Views;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We will not test the view code behind. There's no logic to test.")]
public partial class SeasonSummaryPage : ContentPageBase
{
	public SeasonSummaryPage(SeasonSummaryViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}
