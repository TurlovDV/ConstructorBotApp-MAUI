
using ConstructorBot.Model.Action;
using ConstructorBot.ViewModel.PopupConstructorViewModel;

namespace ConstructorBot.View.PopupConstructorPageView;

public partial class PopupAddButtonsView 
{
	public PopupAddButtonsView(ActionBox actionBoxTap)
	{
		InitializeComponent();

		this.BindingContext = new PopupAddButtonsViewModel(actionBoxTap);
	}
}