using ConstructorBot.ViewModel.PopupConstructorViewModel;

namespace ConstructorBot.View.PopupConstructorPageView;

public partial class PopupImageZoomView
{
	public PopupImageZoomView(string pathImage)
	{
		InitializeComponent();

		this.BindingContext = new PopupImageZoomViewModel(pathImage);
	}
}