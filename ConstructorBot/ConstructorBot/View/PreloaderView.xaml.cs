namespace ConstructorBot.View;

public partial class PreloaderView : ContentPage
{
	public PreloaderView()
	{
		InitializeComponent();

		this.Loaded += async (object sender, EventArgs e) =>
		{
            //Костыль для работы с gif
            await Task.Delay(50);
            preloader.IsAnimationPlaying = false;
            await Task.Delay(50);
            preloader.IsAnimationPlaying = true;

            await Shell.Current.GoToAsync("MainPage");
        };
	}

    //protected override async void OnAppearing()
    //{
    //    base.OnAppearing();
        
    //    //Костыль для работы gif
    //    await Task.Delay(50);
    //    preloader.IsAnimationPlaying = false;
    //    await Task.Delay(50);
    //    preloader.IsAnimationPlaying = true;
    //}
}