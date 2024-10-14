namespace HangTab;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We won't test UI code-behind.")]
public partial class App
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }
}