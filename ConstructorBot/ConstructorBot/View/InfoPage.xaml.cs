namespace ConstructorBot.View;

public partial class InfoPage : ContentPage
{
	public InfoPage()
	{
		InitializeComponent();
		this.Loaded += OnLoading;
	}

    void OnLoading(object sender, EventArgs e)
    {
        CommunityToolkit.Maui.Core.Platform.StatusBar.SetColor(Color.FromArgb("344B6D"));
        CommunityToolkit.Maui.Core.Platform.StatusBar.SetStyle(CommunityToolkit.Maui.Core.StatusBarStyle.LightContent);
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("..");
    }
}