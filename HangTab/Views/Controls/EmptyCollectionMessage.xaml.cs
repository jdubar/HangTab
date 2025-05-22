namespace HangTab.Views.Controls;

public partial class EmptyCollectionMessage : ContentView
{
	public EmptyCollectionMessage()
	{
		InitializeComponent();
	}

	public string Message
    {
        get => (string)GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    public string SubMessage
    {
        get => (string)GetValue(SubMessageProperty);
        set => SetValue(SubMessageProperty, value);
    }

    public static readonly BindableProperty MessageProperty =
        BindableProperty.Create(nameof(Message), typeof(string), typeof(EmptyCollectionMessage), defaultValue: string.Empty);

    public static readonly BindableProperty SubMessageProperty =
        BindableProperty.Create(nameof(SubMessage), typeof(string), typeof(EmptyCollectionMessage), defaultValue: string.Empty);
}