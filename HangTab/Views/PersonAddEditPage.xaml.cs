using HangTab.ViewModels;
using HangTab.Views.Base;

namespace HangTab.Views;

public partial class PersonAddEditPage : ContentPageBase
{
	public PersonAddEditPage(PersonAddEditViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}