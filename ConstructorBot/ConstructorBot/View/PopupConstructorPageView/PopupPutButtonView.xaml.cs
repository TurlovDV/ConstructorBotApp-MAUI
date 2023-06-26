using ConstructorBot.Model.Action;
using ConstructorBot.ViewModel.PopupConstructorViewModel;

namespace ConstructorBot.View.PopupConstructorPageView;

public partial class PopupPutButtonView
{
	public PopupPutButtonView(KeyboardItem keyboard, ActionBox actionBoxTap)
	{
		InitializeComponent();

		this.BindingContext = new PopupPutButtonViewModel(keyboard, actionBoxTap);
	}
}