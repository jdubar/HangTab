using CommunityToolkit.Maui.Views;

using HangTab.ViewModels.Popups;

namespace HangTab.Views.Popups;

public partial class BowlerTypePopup : Popup
{
	public BowlerTypePopup(BowlerTypePopupViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}