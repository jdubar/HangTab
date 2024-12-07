using CommunityToolkit.Mvvm.ComponentModel;

namespace HangTab.Views.ViewModels;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We won't test UI code-behind.")]
public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _busyText;

    [ObservableProperty]
    private string _title;

    protected async Task ExecuteAsync(Func<Task> operation, string busyText = null)
    {
        IsBusy = true;
        BusyText = busyText ?? "Processing...";
        try
        {
            await operation.Invoke();
        }
        finally
        {
            IsBusy = false;
            BusyText = "Processing...";
        }
    }
}
