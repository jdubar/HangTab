using CommunityToolkit.Maui.Views;

using HangTab.ViewModels.Popups;

namespace HangTab.Views.Popups;

public partial class StartNewSeasonPopup : Popup
{
	public StartNewSeasonPopup(StartNewSeasonPopupViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}