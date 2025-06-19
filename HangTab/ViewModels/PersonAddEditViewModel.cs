using CommunityToolkit.Maui.Core;
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
using HangTab.ViewModels.BottomSheets;
using HangTab.ViewModels.Popups;
using HangTab.Views.BottomSheets;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HangTab.ViewModels;
public partial class PersonAddEditViewModel :
    ViewModelBase,
    IQueryAttributable,
    IRecipient<PersonImageAddedOrChangedMessage>
{
    private readonly IPersonService _personService;
    private readonly IDialogService _dialogService;
    private readonly INavigationService _navigationService;
    private readonly IMediaPickerService _mediaPickerService;
    private readonly IPopupService _popupService;

    private readonly AvatarSelectBottomSheet _avatarOptionsBottomSheet;

    public PersonAddEditViewModel(
        IPersonService personService,
        IDialogService dialogService,
        INavigationService navigationService,
        IMediaPickerService mediaPickerService,
        IPopupService popupService)
    {
        _personService = personService;
        _dialogService = dialogService;
        _navigationService = navigationService;
        _mediaPickerService = mediaPickerService;
        _popupService = popupService;

        _avatarOptionsBottomSheet = new AvatarSelectBottomSheet(new AvatarSelectViewModel(_dialogService, _mediaPickerService));

        WeakReferenceMessenger.Default.Register(this);

        ErrorsChanged += AddBowlerViewModel_ErrorsChanged;
    }

    private Person? _person;

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
    private bool _isSub;

    [ObservableProperty]
    private ObservableCollection<ValidationResult> _errors = [];

    [ObservableProperty]
    private bool _isExistingBowler;

    private int selectedType = -1;

    [ObservableProperty]
    [Required(ErrorMessage = "Bowler type is a required field")]
    [NotifyDataErrorInfo]
    private string _selectedTypeName = string.Empty;

    public IReadOnlyList<BowlerType> AllTypes { get; } = Enum.GetValues<BowlerType>().ToList();
    
    public override async Task LoadAsync()
    {
        await Loading(
            async () =>
            {
                if (_person is null && Id > 0)
                {
                    _person = await _personService.GetPersonById(Id);
                }

                if (_person is not null)
                {
                    IsExistingBowler = true;
                }

                MapPerson(_person);
            });
    }

    [RelayCommand]
    private async Task DeletePerson()
    {
        if (_person is null)
        {
            await _dialogService.AlertAsync("Error", "No bowler selected to delete.", "Ok");
            return;
        }

        if (await _dialogService.Ask("Delete", "Are you sure you want to delete this bowler?"))
        {
            if (await _personService.DeletePerson(_person.Id))
            {
                WeakReferenceMessenger.Default.Send(new PersonDeletedMessage(_person.Id));
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

    [RelayCommand]
    private async Task ShowBowlerTypePicker()
    {
        var result = await _popupService.ShowPopupAsync<BowlerTypePopupViewModel>(onPresenting: vm => vm.RadioSubstituteOption = selectedType == (int)BowlerType.Sub);
        if (result is null)
        {
            return;
        }

        selectedType = (bool)result
            ? (int)BowlerType.Sub
            : (int)BowlerType.Regular;

        UpdateSelectedTypeName();
    }

    [RelayCommand(CanExecute = nameof(CanSubmitBowler))]
    private async Task Submit()
    {
        ValidateAllProperties();
        if (Errors.Any())
        {
            return;
        }

        var person = MapDataToPerson();
        if (Id == 0)
        {
            if (await _personService.AddPerson(person))
            {
                WeakReferenceMessenger.Default.Send(new PersonAddedOrChangedMessage(person.Id, person.IsSub));
            }
            else
            {
                await _dialogService.AlertAsync("Error", "The bowler could not be added.", "Ok");
            }
            await _navigationService.GoBack();
        }
        else
        {
            if (await _personService.UpdatePerson(person))
            {
                WeakReferenceMessenger.Default.Send(new PersonAddedOrChangedMessage());
            }
            else
            {
                await _dialogService.AlertAsync("Error", "The bowler could not be updated.", "Ok");
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

    private Person MapDataToPerson()
    {
        return new Person
        {
            Id = Id,
            Name = Name,
            ImageUrl = ImageUrl,
            IsSub = selectedType == (int)BowlerType.Sub,
        };
    }

    private void MapPerson(Person? model)
    {
        if (model is not null)
        {
            Id = model.Id;
            Name = model.Name;
            ImageUrl = model.ImageUrl;
            IsSub = model.IsSub;
            Initials = model.Id > 0 ? model.Name.GetInitials() : string.Empty;

            selectedType = model.IsSub
                ? (int)BowlerType.Sub
                : (int)BowlerType.Regular;

            UpdateSelectedTypeName();
        }

        PageTitle = Id > 0
            ? "Edit Bowler"
            : "Add Bowler";
    }

    private void UpdateSelectedTypeName()
    {
        SelectedTypeName = selectedType == (int)BowlerType.Sub
            ? "Substitute"
            : "Regular";
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.Count > 0)
        {
            _person = query[nameof(Person)] as Person;
        }
    }

    public void Receive(PersonImageAddedOrChangedMessage message)
    {
        ImageUrl = message.ImageUrl;
        _avatarOptionsBottomSheet.DismissAsync();
    }
}
