using CommunityToolkit.Mvvm.ComponentModel;

namespace HangTab.ViewModels;
public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _busyText;

    public async Task ExecuteAsync(Func<Task> operation, string? busyText = null)
    {
        IsBusy = true;
        BusyText = busyText ?? "Processing...";
        try
        {
            await operation?.Invoke();
        }
        finally
        {
            IsBusy = false;
            BusyText = "Processing...";
        }
    }

}
