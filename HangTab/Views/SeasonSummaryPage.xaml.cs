using HangTab.Views.ViewModels;

namespace HangTab.Views;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We won't test UI code-behind.")]
public partial class SeasonSummaryPage
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