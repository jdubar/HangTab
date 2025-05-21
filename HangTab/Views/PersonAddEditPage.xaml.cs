using HangTab.ViewModels;

namespace HangTab.Views;

public partial class PersonAddEditPage : ContentPageBase
{
	public PersonAddEditPage(PersonAddEditViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}