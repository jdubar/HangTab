using HangTab.ViewModels;

namespace HangTab.Views;

public partial class BowlerAddEditPage : ContentPageBase
{
	public BowlerAddEditPage(BowlerAddEditViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}