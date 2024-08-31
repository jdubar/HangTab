using HangTab.Views.ViewModels;

namespace HangTab.Views;

public partial class SeasonPage
{
    private readonly SeasonViewModel _viewModel;

    public SeasonPage(SeasonViewModel viewModel)
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
}