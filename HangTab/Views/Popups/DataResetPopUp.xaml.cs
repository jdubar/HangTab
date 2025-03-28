using CommunityToolkit.Maui.Views;

using HangTab.ViewModels.Popups;

namespace HangTab.Views.Popups;

public partial class DataResetPopUp : Popup
{
	public DataResetPopUp(DataResetPopUpViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}