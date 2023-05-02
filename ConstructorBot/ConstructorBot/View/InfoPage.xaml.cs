using ConstructorBot.ViewModel.InfoPageViewModel;

namespace ConstructorBot.View;

public partial class InfoPage : ContentPage
{
	public InfoPage()
	{
		InitializeComponent();
        this.BindingContext = new InfoViewModel();
		this.Loaded += OnLoading;
        this.Disappearing += (object sender, EventArgs e) =>
        {
            CommunityToolkit.Maui.Core.Platform.StatusBar.SetColor(Color.FromArgb("344B6D"));
        };
	}

    void OnLoading(object sender, EventArgs e)
    {
        CommunityToolkit.Maui.Core.Platform.StatusBar.SetColor(Color.FromArgb("101621"));
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("..");
    }
}