using HangTab.ViewModels;

using The49.Maui.BottomSheet;

namespace HangTab.Views;

public partial class AvatarSelectBottomSheet : BottomSheet
{
	public AvatarSelectBottomSheet(AvatarSelectViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}