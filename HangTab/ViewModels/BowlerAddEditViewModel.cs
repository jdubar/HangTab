using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using HangTab.Messages;
using HangTab.Models;
using HangTab.Services;
using HangTab.ViewModels.Base;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HangTab.ViewModels;
public partial class BowlerAddEditViewModel : ViewModelBase
{
    private readonly IBowlerService _bowlerService;
    private readonly IDialogService _dialogService;
    private readonly INavigationService _navigationService;

    public BowlerAddEditViewModel(IBowlerService bowlerService, IDialogService dialogService, INavigationService navigationService)
    {
        _bowlerService = bowlerService;
        _dialogService = dialogService;
        _navigationService = navigationService;

        ErrorsChanged += AddBowlerViewModel_ErrorsChanged;
    }

    private Bowler? _bowler;

    [ObservableProperty]
    private string _pageTitle = string.Empty;

    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    [NotifyDataErrorInfo]
    private string _firstName = string.Empty;

    [ObservableProperty]
    private string? _lastName;

    [ObservableProperty]
    private string? _imageUrl;

    [ObservableProperty]
    private bool _isSub;

    [ObservableProperty]
    private ObservableCollection<ValidationResult> _errors = [];

    [RelayCommand(CanExecute = nameof(CanSubmitBowler))]
    private async Task Submit()
    {
        ValidateAllProperties();
        if (Errors.Any())
        {
            return;
        }

        var model = MapDataToBowler();
        if (Id == 0)
        {
            if (await _bowlerService.AddBowler(model))
            {
                WeakReferenceMessenger.Default.Send(new BowlerAddedOrChangedMessage());
                await _dialogService.Notify("Success", "The bowler was added.");
                await _navigationService.GoToOverview();
            }
            else
            {
                await _dialogService.Notify("Error", "The bowler could not be added.");
            }
        }
        else
        {
            if (await _bowlerService.UpdateBowler(model))
            {
                WeakReferenceMessenger.Default.Send(new BowlerAddedOrChangedMessage());
                await _dialogService.Notify("Success", "The bowler was updated.");
                await _navigationService.GoBack();
            }
            else
            {
                await _dialogService.Notify("Error", "The bowler could not be updated.");
            }
        }
    }

    private bool CanSubmitBowler() => !HasErrors;

    public override async Task LoadAsync()
    {
        await Loading(
            async () =>
            {
                if (_bowler is null && Id > 0)
                {
                    _bowler = await _bowlerService.GetBowler(Id);
                }
                MapBowler(_bowler);

                ValidateAllProperties();
            });
    }

    private Bowler MapDataToBowler()
    {
        return new Bowler
        {
            Id = Id,
            FirstName = FirstName,
            LastName = LastName ?? string.Empty,
            ImageUrl = ImageUrl ?? string.Empty,
            IsSub = IsSub,
        };
    }

    private void MapBowler(Bowler? model)
    {
        if (model is not null)
        {
            Id = model.Id;
            FirstName = model.FirstName;
            LastName = model.LastName;
            ImageUrl = model.ImageUrl;
            IsSub = model.IsSub;
        }

        PageTitle = Id > 0 ? "Edit event" : "Add event";
    }

    private void AddBowlerViewModel_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
    {
        Errors.Clear();
        GetErrors().ToList().ForEach(Errors.Add);
        SubmitCommand.NotifyCanExecuteChanged();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.Count > 0)
        {
            _bowler = query["Bowler"] as Bowler;
        }
    }
}
