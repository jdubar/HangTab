using HangTab.ViewModels;

namespace HangTab.Views;

public partial class WeekOverviewPage : ContentPageBase
{
	public WeekOverviewPage(WeekOverviewViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}