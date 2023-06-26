using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBot.ViewModel.PopupConstructorViewModel
{
    public partial class PopupImageZoomViewModel : ObservableObject
    {
        [ObservableProperty]
        string pathImage;

        public PopupImageZoomViewModel(string pathImage)
        {
            PathImage = pathImage;
        }

        [RelayCommand]
        public async void CloseThisPopup()
        {
            await MopupService.Instance.PopAsync();
        }
    }
}
