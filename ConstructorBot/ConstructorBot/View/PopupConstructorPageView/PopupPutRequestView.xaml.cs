using ConstructorBot.Model.Action;
using ConstructorBot.ViewModel.PopupConstructorViewModel;

namespace ConstructorBot.View.PopupConstructorPageView;

public partial class PopupPutRequestView 
{
	public PopupPutRequestView(ActionBox actionBoxTap)
	{
		InitializeComponent();

		this.BindingContext = new PopupPutRequestViewModel(actionBoxTap);
	}
}