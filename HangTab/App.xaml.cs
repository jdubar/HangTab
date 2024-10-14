namespace HangTab;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We won't test dependency injection code.")]
public partial class App
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }
}