namespace ConstructorBot.Mopups;

public partial class PopupPage
{
	
	public PopupPage(string title, string message)
	{        
		InitializeComponent();

        this.title.Text = title;
        this.message.Text = message;

    }
}