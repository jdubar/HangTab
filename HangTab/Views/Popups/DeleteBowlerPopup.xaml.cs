using CommunityToolkit.Maui.Views;

using HangTab.ViewModels.Popups;

namespace HangTab.Views.Popups;

public partial class DeleteBowlerPopup : Popup
{
	public DeleteBowlerPopup(DeleteBowlerPopupViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}