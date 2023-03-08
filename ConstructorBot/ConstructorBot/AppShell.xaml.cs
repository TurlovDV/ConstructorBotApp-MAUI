namespace ConstructorBot;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute(nameof(ConstructorPage), typeof(ConstructorPage));

        //Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
    }
}
