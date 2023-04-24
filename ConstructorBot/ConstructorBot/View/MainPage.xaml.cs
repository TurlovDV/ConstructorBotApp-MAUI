using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core.Platform;
using ConstructorBot.ViewModel.ConstructorPageViewModel;
using ConstructorBot.ViewModel.ConstructorPageViewModel.Action;
using ConstructorBot.ViewModel.MainPageViewModel;
using System.Collections.ObjectModel;

namespace ConstructorBot;

public partial class MainPage : ContentPage
{
    public MainViewModel MainViewModel { get; set; }    
    public MainPage()
	{
        MainViewModel = ServiceProvider.GetService<MainViewModel>();

        InitializeComponent();
        
        BindingContext = MainViewModel;

        ////Получение сохраненных actionBoxes
        //ServiceProvider.GetService<ConstructorViewModel>().ActionBoxes
        //    = new ObservableCollection<ActionBox>(SaveSettingOrActionBoxes.Get());

        //Анимация работы elipse_loading 

        Task.Run(async () =>
        {
            while (true)
            {
                //SinInOut
                await elipse_loading.RotateTo(360, 1000, Easing.SinInOut);
                elipse_loading.Rotation = 0;
            }
        });

        //this.Loaded += Loading;
    }


    //public void Loading(object sender, EventArgs e)
    //{        
    //}

    private async void Button_PushToConstructor(object sender, EventArgs e)
    {
        /*
        //await Shell.Current.GoToAsync("ConstructorPage");
        //await Navigation.PushAsync(new ConstructorPage());
        //await Navigation.PopAsync();
        //var _backGround = ServiceProvider.GetService<IServiceDomainBot>();
        //_backGround.Start();
        */
        await Shell.Current.GoToAsync("ConstructorPage");               
    }

    private async void Button_PushToInfo(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("InfoPage");
    }

    protected override bool OnBackButtonPressed()
    {
        return true;
    }

    //private void ImageButton_Clicked(object sender, EventArgs e)
    //{
    //    ((sender as ImageButton).CommandParameter as StackLayout).IsVisible = !((sender as ImageButton).CommandParameter as StackLayout).IsVisible;
    //}
}