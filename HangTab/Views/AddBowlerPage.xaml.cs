using HangTab.Views.ViewModels;

namespace HangTab.Views;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We won't test UI code-behind.")]
public partial class AddBowlerPage
{
    public AddBowlerPage(AddBowlerViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}