using HangTab.ViewModels.BottomSheets;

using Plugin.Maui.BottomSheet;

namespace HangTab.Views.BottomSheets;

public partial class AvatarSelectBottomSheet : BottomSheet
{
	public AvatarSelectBottomSheet(AvatarSelectViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}