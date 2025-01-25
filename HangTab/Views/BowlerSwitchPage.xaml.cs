using HangTab.ViewModels;

namespace HangTab.Views;

public partial class BowlerSwitchPage : ContentPageBase
{
	public BowlerSwitchPage(BowlerSwitchViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}