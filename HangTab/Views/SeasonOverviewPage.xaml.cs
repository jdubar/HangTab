using HangTab.ViewModels;
using HangTab.Views.Base;

namespace HangTab.Views;

public partial class SeasonOverviewPage : ContentPageBase
{
	public SeasonOverviewPage(SeasonOverviewViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}