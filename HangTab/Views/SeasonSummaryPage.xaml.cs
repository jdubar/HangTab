using HangTab.ViewModels;
using HangTab.Views.Base;

namespace HangTab.Views;

public partial class SeasonSummaryPage : ContentPageBase
{
	public SeasonSummaryPage(SeasonSummaryViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}
