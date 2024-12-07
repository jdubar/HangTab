using CommunityToolkit.Mvvm.Messaging.Messages;

namespace HangTab.Messages;

public class WeekUpdateMessage(Week week) : ValueChangedMessage<Week>(week);