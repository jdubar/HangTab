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
using HangTab.Views.BottomSheets;

using Plugin.Maui.BottomSheet.Navigation;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HangTab.ViewModels;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a ViewModel for the UI and does not require unit tests.")]
public partial class PersonAddEditViewModel :
    ViewModelBase,
    IQueryAttributable,
    IRecipient<PersonImageAddedOrChangedMessage>
{
    private readonly IPersonService _personService;
    private readonly IDialogService _dialogService;
    private readonly INavigationService _navigationService;
    private readonly IBottomSheetNavigationService _bottomSheetNavigationService;

    public PersonAddEditViewModel(
        IPersonService personService,
        IDialogService dialogService,
        INavigationService navigationService,
        IBottomSheetNavigationService bottomSheetNavigationService)
    {
        _personService = personService;
        _dialogService = dialogService;
        _navigationService = navigationService;
        _bottomSheetNavigationService = bottomSheetNavigationService;

        WeakReferenceMessenger.Default.Register(this);

        ErrorsChanged += AddBowlerViewModel_ErrorsChanged;
    }

    [ObservableProperty]
    [Required(ErrorMessage="Bowler type is a required field")]
    [Range((int)BowlerType.Regular, (int)BowlerType.Sub, ErrorMessage="You must select a bowler type")]
    [NotifyDataErrorInfo]
    private int _bowlerTypeIndex = -1;

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

    public IReadOnlyList<BowlerType> BowlerTypes { get; } = Enum.GetValues<BowlerType>();

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

        await _bottomSheetNavigationService.NavigateToAsync(nameof(AvatarSelectBottomSheet));
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
            IsSub = BowlerTypeIndex == (int)BowlerType.Sub,
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

            BowlerTypeIndex = model.IsSub
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
            _person = query[nameof(Person)] as Person;
        }
    }

    public void Receive(PersonImageAddedOrChangedMessage message) => ImageUrl = message.ImageUrl;
}
