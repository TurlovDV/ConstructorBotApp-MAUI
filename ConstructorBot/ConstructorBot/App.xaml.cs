using ConstructorBot.Model.Action;
using ConstructorBot.Services.ServiceStorage;
using ConstructorBot.View;
using ConstructorBot.ViewModel.ConstructorPageViewModel;
using System.Collections.ObjectModel;

namespace ConstructorBot;

public partial class App : Application
{
    public App()
	{
        InitializeComponent();

        MainPage = new AppShell();
    }
}
