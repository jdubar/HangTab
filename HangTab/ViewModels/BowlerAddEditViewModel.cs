using CommunityToolkit.Maui.Core.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using HangTab.Enums;
using HangTab.Extensions;
using HangTab.Messages;
using HangTab.Models;
using HangTab.Services;
using HangTab.ViewModels.Base;
using HangTab.Views;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HangTab.ViewModels;
public partial class BowlerAddEditViewModel :
    ViewModelBase,
    IQueryAttributable,
    IRecipient<BowlerImageAddedOrChangedMessage>
{
    private readonly IBowlerService _bowlerService;
    private readonly IDialogService _dialogService;
    private readonly INavigationService _navigationService;
    private readonly IMediaPickerService _mediaPickerService;

    private readonly AvatarSelectBottomSheet _avatarOptionsBottomSheet;

    public BowlerAddEditViewModel(
        IBowlerService bowlerService,
        IDialogService dialogService,
        INavigationService navigationService,
        IMediaPickerService mediaPickerService)
    {
        _bowlerService = bowlerService;
        _dialogService = dialogService;
        _navigationService = navigationService;
        _mediaPickerService = mediaPickerService;

        _avatarOptionsBottomSheet = new AvatarSelectBottomSheet(new AvatarSelectViewModel(_dialogService, _mediaPickerService));

        WeakReferenceMessenger.Default.Register(this);

        ErrorsChanged += AddBowlerViewModel_ErrorsChanged;
    }

    private Bowler? _bowler;

    [ObservableProperty]
    private string _pageTitle = string.Empty;

    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    [Required(ErrorMessage="Bowler name is a required field")]
    [MinLength(3, ErrorMessage="The bowler's name must have at least 3 characters")]
    [MaxLength(50, ErrorMessage="Bowler's name has a maximum of 50 characters")]
    [NotifyDataErrorInfo]
    private string _name = string.Empty;

    [ObservableProperty]
    private string? _imageUrl = null;

    [ObservableProperty]
    private string _initials = string.Empty;

    [ObservableProperty]
    private bool _isInactive;

    [ObservableProperty]
    private bool _isSub;

    [ObservableProperty]
    private ObservableCollection<ValidationResult> _errors = [];

    [ObservableProperty]
    private bool _isExistingBowler;

	[ObservableProperty]
    [Required(ErrorMessage="Bowler type is a required field")]
    [Range((int)BowlerType.Regular, (int)BowlerType.Sub, ErrorMessage="You must select a bowler type")]
    [NotifyDataErrorInfo]
    private int _selectedType = -1;

    public IReadOnlyList<BowlerType> AllTypes { get; } = Enum.GetValues(typeof(BowlerType)).Cast<BowlerType>().ToList();
    
    public override async Task LoadAsync()
    {
        await Loading(
            async () =>
            {
                if (_bowler is null && Id > 0)
                {
                    _bowler = await _bowlerService.GetBowlerById(Id);
                }

                if (_bowler is not null)
                {
                    IsExistingBowler = true;
                }

                MapBowler(_bowler);
            });
    }

    [RelayCommand]
    private async Task DeleteBowler()
    {
        if (_bowler is null)
        {
            await _dialogService.AlertAsync("Error", "No bowler selected to delete.", "Ok");
            return;
        }

        if (await _dialogService.Ask("Delete", "Are you sure you want to delete this bowler?"))
        {
            if (await _bowlerService.DeleteBowler(_bowler.Id))
            {
                WeakReferenceMessenger.Default.Send(new BowlerDeletedMessage(_bowler.Id));
            }
            else
            {
                await _dialogService.AlertAsync("Critical Error", "Error occurred while deleting the bowler!", "Ok");
            }
            await _navigationService.GoBack();
        }
    }

    [RelayCommand]
    private async Task ShowAvatarOptionsBottomSheet(ITextInput view, CancellationToken token)
    {
        if (view.IsSoftKeyboardShowing())
        {
            await view.HideKeyboardAsync(token);
        }

        await _avatarOptionsBottomSheet.ShowAsync();
    }

    [RelayCommand(CanExecute = nameof(CanSubmitBowler))]
    private async Task Submit()
    {
        ValidateAllProperties();
        if (Errors.Any())
        {
            return;
        }

        var bowler = MapDataToBowler();
        if (Id == 0)
        {
            if (await _bowlerService.AddBowler(bowler))
            {
                WeakReferenceMessenger.Default.Send(new BowlerAddedOrChangedMessage(bowler.Id, bowler.IsSub));
            }
            else
            {
                await _dialogService.Notify("Error", "The bowler could not be added.");
            }
            await _navigationService.GoBack();
        }
        else
        {
            if (await _bowlerService.UpdateBowler(bowler))
            {
                WeakReferenceMessenger.Default.Send(new BowlerAddedOrChangedMessage());
            }
            else
            {
                await _dialogService.Notify("Error", "The bowler could not be updated.");
            }
            await _navigationService.GoBack();
        }
    }

    private void AddBowlerViewModel_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
    {
        Errors.Clear();
        GetErrors().ToList().ForEach(Errors.Add);
        SubmitCommand.NotifyCanExecuteChanged();
    }

    private bool CanSubmitBowler() => !HasErrors;

    private Bowler MapDataToBowler()
    {
        return new Bowler
        {
            Id = Id,
            Name = Name,
            ImageUrl = ImageUrl,
            IsInactive = IsInactive,
            IsSub = SelectedType == (int)BowlerType.Sub,
        };
    }    
    private void MapBowler(Bowler? model)
    {
        if (model is not null)
        {
            Id = model.Id;
            Name = model.Name;
            ImageUrl = model.ImageUrl;
            IsInactive = model.IsInactive;
            IsSub = model.IsSub;
            Initials = model.Id > 0 ? model.Name.GetInitials() : string.Empty;

            SelectedType = model.IsSub
                ? (int)BowlerType.Sub
                : (int)BowlerType.Regular;
        }

        PageTitle = Id > 0
            ? "Edit Bowler"
            : "Add Bowler";
    }
        
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.Count > 0)
        {
            _bowler = query["Bowler"] as Bowler;
        }
    }

    public void Receive(BowlerImageAddedOrChangedMessage message)
    {
        ImageUrl = message.ImageUrl;
        _avatarOptionsBottomSheet.DismissAsync();
    }
}
