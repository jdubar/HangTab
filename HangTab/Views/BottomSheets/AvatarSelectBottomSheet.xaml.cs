using HangTab.ViewModels.BottomSheets;

using Plugin.Maui.BottomSheet;

namespace HangTab.Views.BottomSheets;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We will not test the view code behind. There's no logic to test.")]
public partial class AvatarSelectBottomSheet : BottomSheet
{
	public AvatarSelectBottomSheet(AvatarSelectViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}