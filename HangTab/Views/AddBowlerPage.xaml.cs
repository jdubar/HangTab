using HangTab.Views.ViewModels;

namespace HangTab.Views;

public partial class AddBowlerPage
{
    public AddBowlerPage(AddBowlerViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}