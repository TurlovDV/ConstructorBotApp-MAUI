using ConstructorBot.View;

namespace ConstructorBot;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute(nameof(ConstructorPage), typeof(ConstructorPage));
        Routing.RegisterRoute(nameof(InfoPage), typeof(InfoPage));
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        Routing.RegisterRoute(nameof(ChatsView), typeof(ChatsView));
        Routing.RegisterRoute(nameof(ChatUserView), typeof(ChatUserView));


        //Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
    }
}
