namespace ConstructorBot.Mopups;

public partial class PopupLoading
{
	public PopupLoading()
	{
		InitializeComponent();
        Loaded += Loading;
	}

    public async void Loading(object sender, EventArgs e)
    {
        await Task.Delay(50);
        preloader.IsAnimationPlaying = false;
        await Task.Delay(50);
        preloader.IsAnimationPlaying = true;

    }
}