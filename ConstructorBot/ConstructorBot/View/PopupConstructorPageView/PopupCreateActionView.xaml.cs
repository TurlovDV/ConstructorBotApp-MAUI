using ConstructorBot.Model.Action;
using ConstructorBot.ViewModel.PopupConstructorViewModel;
using System.Collections.ObjectModel;

namespace ConstructorBot.View.PopupConstructorPageView;

public partial class PopupCreateActionView 
{
	public PopupCreateActionView(ObservableCollection<ActionBox> actionBoxes)
	{
		InitializeComponent();

		this.BindingContext = new PopupCreateActionViewModel(actionBoxes);
	}
}