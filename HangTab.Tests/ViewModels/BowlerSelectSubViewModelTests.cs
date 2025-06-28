using CommunityToolkit.Mvvm.Messaging;

using HangTab.Mappers;
using HangTab.Messages;
using HangTab.Models;
using HangTab.Services;
using HangTab.ViewModels;
using HangTab.ViewModels.Items;

namespace HangTab.Tests.ViewModels;

public class BowlerSelectSubViewModelTests
{
    private readonly IPersonService _personService = A.Fake<IPersonService>();
    private readonly IBowlerService _bowlerService = A.Fake<IBowlerService>();
    private readonly INavigationService _navigationService = A.Fake<INavigationService>();
    private readonly IMapper<IEnumerable<Person>, IEnumerable<SubListItemViewModel>> _mapper = A.Fake<IMapper<IEnumerable<Person>, IEnumerable<SubListItemViewModel>>>();
    
    private BowlerSelectSubViewModel CreateVm() =>
        new(_personService, _bowlerService, _navigationService, _mapper);

    [Fact]
    public async Task LoadAsync_LoadsSubsAndSetsSelectedSub()
    {
        // Arrange
        var bowler = new Bowler { Id = 1, SubId = 2, WeekId = 10 };
        var people = new List<Person> { new() { Id = 2, Name = "Sub1", IsSub = true } };
        var subVms = new List<SubListItemViewModel> { new(2, "Sub1", true) };
        var vm = CreateVm();
        vm.ApplyQueryAttributes(new Dictionary<string, object> { { nameof(Bowler), bowler } });

        A.CallTo(() => _personService.GetSubstitutes()).Returns(Task.FromResult<IEnumerable<Person>>(people));
        A.CallTo(() => _bowlerService.GetAllBowlersByWeekId(10)).Returns(Task.FromResult<IEnumerable<Bowler>>([]));
        A.CallTo(() => _mapper.Map(people)).Returns(subVms);

        // Act
        await vm.LoadAsync();

        // Assert
        Assert.Single(vm.Subs);
        Assert.Equal(2, vm.SelectedSub?.Id);
        Assert.True(vm.SelectedSub?.IsSelected);
        Assert.Equal(1, vm.Id);
    }

    [Fact]
    public void ShowCheckmarkOnSelectedSub_DeselectsOthersAndSelectsCurrent()
    {
        // Arrange
        var vm = CreateVm();
        var sub1 = new SubListItemViewModel(1, "A", true) { IsSelected = true };
        var sub2 = new SubListItemViewModel(2, "B", true) { IsSelected = false };
        vm.Subs = [sub1, sub2];
        vm.SelectedSub = sub2;

        // Act
        vm.ShowCheckmarkOnSelectedSubCommand.Execute(null);

        // Assert
        Assert.False(sub1.IsSelected);
        Assert.True(sub2.IsSelected);
    }

    [Fact]
    public async Task SubmitSelectedSubAsync_UpdatesBowler_SendsMessage_AndNavigatesBack()
    {
        // Arrange
        var id = 4;
        var subId = 5;
        var bowler = new Bowler { Id = id, WeekId = 2, PersonId = 3, Status = Enums.Status.UsingSub, HangCount = 0 };
        var selectedSub = new SubListItemViewModel(subId, "Sub", true);
        var vm = CreateVm();
        vm.ApplyQueryAttributes(new Dictionary<string, object> { { nameof(Bowler), bowler } });
        vm.Id = id;
        vm.SelectedSub = selectedSub;

        A.CallTo(() => _bowlerService.UpdateBowler(A<Bowler>.That.Matches(b => b.SubId == 5 && b.Id == 4))).Returns(Task.FromResult(true));
        var messenger = WeakReferenceMessenger.Default;
        BowlerSubChangedMessage? receivedMsg = null;
        messenger.Register<BowlerSubChangedMessage>(this, (_, msg) => receivedMsg = msg);

        // Act
        await vm.SubmitSelectedSubCommand.ExecuteAsync(null);

        // Assert
        A.CallTo(() => _bowlerService.UpdateBowler(A<Bowler>.That.Matches(b => b.SubId == 5 && b.Id == 4))).MustHaveHappenedOnceExactly();
        A.CallTo(() => _navigationService.GoBack()).MustHaveHappenedOnceExactly();
        Assert.NotNull(receivedMsg);
        Assert.Equal(id, receivedMsg.Id);
        Assert.Equal(subId, receivedMsg.SubId);
    }

    [Fact]
    public void EnableSubmitButton_IsTrue_WhenSelectedSubIsNotNull()
    {
        // Arrange
        var vm = CreateVm();
        vm.SelectedSub = new SubListItemViewModel(1, "A", true);

        // Act & Assert
        Assert.True(vm.EnableSubmitButton);
    }

    [Fact]
    public void EnableSubmitButton_IsFalse_WhenSelectedSubIsNull()
    {
        // Arrange
        var vm = CreateVm();
        vm.SelectedSub = null;

        // Act & Assert
        Assert.False(vm.EnableSubmitButton);
    }
}