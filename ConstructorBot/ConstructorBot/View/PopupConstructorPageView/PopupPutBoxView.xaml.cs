using ConstructorBot.Model.Action;
using ConstructorBot.Services.PopupService;
using ConstructorBot.ViewModel.PopupConstructorViewModel;

namespace ConstructorBot.View.PopupConstructorPageView;

public partial class PopupPutBoxView
{
	public PopupPutBoxView(ActionBox actionBoxTapLast)
	{
		InitializeComponent();

		this.BindingContext = new PopupPutBoxViewModel(actionBoxTapLast);
	}
}