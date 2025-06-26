using HangTab.ViewModels;
using HangTab.Views.Base;

namespace HangTab.Views;

public partial class WeekDetailsPage : ContentPageBase
{
	public WeekDetailsPage(WeekDetailsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}