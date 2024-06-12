using HangTab.Views.ViewModels;

namespace HangTab.Views;

public partial class SeasonSummaryPage : ContentPage
{
    private readonly SeasonSummaryViewModel _viewModel;

    public SeasonSummaryPage(SeasonSummaryViewModel viewModel)
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