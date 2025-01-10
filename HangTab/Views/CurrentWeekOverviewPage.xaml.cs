using HangTab.ViewModels;

namespace HangTab.Views;

public partial class CurrentWeekOverviewPage : ContentPageBase
{
	public CurrentWeekOverviewPage(CurrentWeekOverviewViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}