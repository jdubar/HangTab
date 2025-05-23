using HangTab.ViewModels;
using HangTab.Views.Base;

namespace HangTab.Views;

public partial class BowlerSwitchPage : ContentPageBase
{
	public BowlerSwitchPage(BowlerSwitchViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}