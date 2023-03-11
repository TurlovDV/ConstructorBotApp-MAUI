using ConstructorBot.ViewModel.ConstructorPageViewModel;
using ConstructorBot.ViewModel.ConstructorPageViewModel.Action;
using System.Collections.ObjectModel;

namespace ConstructorBot;

public partial class App : Application
{
    public App()
	{
        DependencyService.Register<IMessageService, MessageService>();
        
        InitializeComponent();

        //Routing.RegisterRoute(nameof(ConstructorPage), typeof(ConstructorPage));
        MainPage = new AppShell();
         
        //Получение сохраненных actionBoxes
        ServiceProvider.GetService<ConstructorViewModel>().ActionBoxes
            = new ObservableCollection<ActionBox>(SaveSettingOrActionBoxes.Get());
    }

    //Выход из приложения
    //protected async override void OnSleep()
    //{
    //    await Shell.Current.GoToAsync("..");
    //    base.OnSleep();
    //}

    public interface IMessageService
    {
        Task ShowAsync(string title, string message);
        Task<string> ShowOrOkAsync(string title, string message);
    }

    public class MessageService : IMessageService
    {
        public async Task ShowAsync(string title, string message)
        {            
            await App.Current.MainPage.DisplayAlert(title, message, "Ok");
        }

        public async Task<string> ShowOrOkAsync(string title, string message)
        {
            return await App.Current.MainPage.DisplayPromptAsync(title, message, "Да", "Нет");
        }

        //public async Task ShowSheetAsync(string title, string message)
        //{
        //    await App.Current.MainPage.DisplayPromptAsync(title, message, "Ok");
        //}


    }
}
