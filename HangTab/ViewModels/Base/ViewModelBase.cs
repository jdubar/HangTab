using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace HangTab.ViewModels.Base;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a ViewModel for the UI and does not require unit tests.")]
public partial class ViewModelBase : ObservableValidator, IViewModelBase
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(LoadingComplete))]
    private bool _isLoading;

    [ObservableProperty]
    private string _loadingText = "Processing...";

     public bool LoadingComplete => !IsLoading;

    public IAsyncRelayCommand InitializeAsyncCommand { get; }

    public ViewModelBase()
    {
        InitializeAsyncCommand = new AsyncRelayCommand(async () =>
        {
            IsLoading = true;
            await Loading(LoadAsync);
            IsLoading = false;
        });
    }

    protected static async Task Loading(Func<Task> unitOfWork) => await unitOfWork();

    public virtual Task LoadAsync() => Task.CompletedTask;
}