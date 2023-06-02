namespace ConstructorBot.Mopups;

public partial class PopupNotificationPage 
{
	public PopupNotificationPage(string title, string message)
	{
		InitializeComponent();

        this.title.Text = title;
        this.message.Text = message;

        
    }
}