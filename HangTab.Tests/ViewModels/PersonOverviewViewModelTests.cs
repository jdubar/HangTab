using HangTab.Mappers;
using HangTab.Messages;
using HangTab.Models;
using HangTab.Services;
using HangTab.ViewModels;
using HangTab.ViewModels.Items;

using System.Collections.ObjectModel;

namespace HangTab.Tests.ViewModels;

public class PersonOverviewViewModelTests
{
    private readonly IPersonService _personService = A.Fake<IPersonService>();
    private readonly INavigationService _navigationService = A.Fake<INavigationService>();
    private readonly IWeekService _weekService = A.Fake<IWeekService>();
    private readonly IMapper<BowlerListItemViewModel, Person> _personMapper = A.Fake<IMapper<BowlerListItemViewModel, Person>>();
    private readonly IMapper<IEnumerable<Person>, IEnumerable<BowlerListItemViewModel>> _bowlerListItemViewModelMapper = A.Fake<IMapper<IEnumerable<Person>, IEnumerable<BowlerListItemViewModel>>>();

    private PersonOverviewViewModel CreateVm() =>
        new(_personService, _navigationService, _weekService, _personMapper, _bowlerListItemViewModelMapper);

    [Fact]
    public async Task LoadAsync_WhenNoBowlers_LoadsAllBowlers()
    {
        // Arrange
        var vm = CreateVm();
        var people = new List<Person>
        {
            new() { Id = 1, Name = "Alice" },
            new() { Id = 2, Name = "Bob" }
        };
        var bowlers = new List<BowlerListItemViewModel>
        {
            new(id: 1, name: "Alice", isSub: false),
            new(id: 2, name: "Bob", isSub: false)
        };
        A.CallTo(() => _personService.GetAllPeople()).Returns(Task.FromResult<IEnumerable<Person>>(people));
        A.CallTo(() => _bowlerListItemViewModelMapper.Map(A<IEnumerable<Person>>._)).Returns(bowlers);

        // Act
        await vm.LoadAsync();

        // Assert
        Assert.Equal(2, vm.Bowlers.Count);
        Assert.Contains(vm.Bowlers, b => b.Name == "Alice");
        Assert.Contains(vm.Bowlers, b => b.Name == "Bob");
    }

    [Fact]
    public void OnSearchTextChanged_FiltersBowlers()
    {
        // Arrange
        var vm = CreateVm();
        var bowlers = new List<BowlerListItemViewModel>
        {
            new( id: 1, name: "Alice", isSub: false),
            new(id : 2, name : "Bob", isSub : false)
        };
        typeof(PersonOverviewViewModel)
            .GetProperty("AllBowlers", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
            .SetValue(vm, bowlers);
        vm.Bowlers = new ObservableCollection<BowlerListItemViewModel>(bowlers);

        // Act
        vm.SearchText = "Bob";

        // Assert
        Assert.Single(vm.Bowlers);
        Assert.Equal("Bob", vm.Bowlers[0].Name);
    }

    [Fact]
    public async Task NavigateToAddBowlerCommand_NavigatesToAddBowler()
    {
        // Arrange
        var vm = CreateVm();

        // Act
        await vm.NavigateToAddBowlerCommand.ExecuteAsync(null);

        // Assert
        A.CallTo(() => _navigationService.GoToAddBowler()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task NavigateToEditSelectedBowlerCommand_NavigatesToEditAndClearsSelection()
    {
        // Arrange
        var vm = CreateVm();
        var bowlerVm = new BowlerListItemViewModel(id: 1, name: "Joe", isSub: false);
        var person = new Person { Id = 1, Name = "Alice" };
        vm.SelectedBowler = bowlerVm;
        A.CallTo(() => _personMapper.Map(bowlerVm)).Returns(person);

        // Act
        await vm.NavigateToEditSelectedBowlerCommand.ExecuteAsync(null);

        // Assert
        A.CallTo(() => _navigationService.GoToEditBowler(person)).MustHaveHappenedOnceExactly();
        Assert.Null(vm.SelectedBowler);
    }

    [Fact]
    public void Receive_SystemResetMessage_ClearsBowlers()
    {
        // Arrange
        var vm = CreateVm();
        vm.Bowlers =
        [
            new(id: 1, name: "Alice", isSub: false)
        ];

        // Act
        vm.Receive(new SystemResetMessage());

        // Assert
        Assert.Empty(vm.Bowlers);
    }

    [Fact]
    public async Task Receive_PersonAddedOrChangedMessage_RefreshesBowlers()
    {
        // Arrange
        var vm = CreateVm();
        var people = new List<Person> { new() { Id = 1, Name = "Alice" } };
        var bowlers = new List<BowlerListItemViewModel> { new(id: 1, name: "Alice", isSub: false) };
        A.CallTo(() => _personService.GetAllPeople()).Returns(Task.FromResult<IEnumerable<Person>>(people));
        A.CallTo(() => _bowlerListItemViewModelMapper.Map(A<IEnumerable<Person>>._)).Returns(bowlers);

        // Act
        await Task.Run(() => vm.Receive(new PersonAddedOrChangedMessage(1, false)));

        // Assert
        Assert.Single(vm.Bowlers);
        Assert.Equal("Alice", vm.Bowlers[0].Name);
    }

    [Fact]
    public async Task Receive_PersonDeletedMessage_RefreshesBowlers()
    {
        // Arrange
        var vm = CreateVm();
        var people = new List<Person> { new() { Id = 2, Name = "Bob" } };
        var bowlers = new List<BowlerListItemViewModel> { new(id: 2, name: "Bob", isSub: false) };
        A.CallTo(() => _personService.GetAllPeople()).Returns(Task.FromResult<IEnumerable<Person>>(people));
        A.CallTo(() => _bowlerListItemViewModelMapper.Map(A<IEnumerable<Person>>._)).Returns(bowlers);

        // Act
        await Task.Run(() => vm.Receive(new PersonDeletedMessage(1)));

        // Assert
        Assert.Single(vm.Bowlers);
        Assert.Equal("Bob", vm.Bowlers[0].Name);
    }
}