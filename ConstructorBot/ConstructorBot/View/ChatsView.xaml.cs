using ConstructorBot.Model;
using ConstructorBot.ViewModel;

namespace ConstructorBot.View;

[QueryProperty(nameof(LogicUsers), "LogicUsers")]
public partial class ChatsView : ContentPage
{    
    List<LogicUser> logicUsers;
    public List<LogicUser> LogicUsers
    {
        set
        {
            logicUsers = value;
            this.BindingContext = new ChatsViewModel(value);
        }
    }
    public ChatsView()
	{
		InitializeComponent();
	}
}