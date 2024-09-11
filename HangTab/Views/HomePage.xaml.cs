using HangTab.Views.ViewModels;

namespace HangTab.Views;

public partial class HomePage
{
    private readonly HomeViewModel _viewModel;

    public HomePage(HomeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.InitializeDataCommand.Execute(null);
    }

    private void SlideCompleted(object sender, EventArgs e)
        => _viewModel.ExecuteSlideCommand.Execute(null);
}