using ConstructorBot.Model;
using ConstructorBot.ViewModel.ChatPageViewModel;

namespace ConstructorBot.View;

[QueryProperty(nameof(LogicUser), "LogicUser")]
public partial class ChatUserView : ContentPage
{
    LogicUser logicUser;
    public LogicUser LogicUser
    {
        set
        {
            logicUser = value;
            this.BindingContext = new ChatUserViewModel(value);
        }
    }

    public ChatUserView()
	{
		InitializeComponent();
	}
}