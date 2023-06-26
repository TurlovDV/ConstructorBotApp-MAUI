using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConstructorBot.Model.Action;
using ConstructorBot.View.PopupConstructorPageView;
using Mopups.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBot.ViewModel.PopupConstructorViewModel
{
    public partial class PopupPutRequestViewModel : ObservableObject
    {
        [ObservableProperty]
        ActionBox actionBoxTap;
        
        public PopupPutRequestViewModel(ActionBox actionBoxTap)
        {
            ActionBoxTap = actionBoxTap; 
        }

        [RelayCommand]
        public async void SaveRequest(object nameRequest)
        {            
            ActionBoxTap.NameSaveMessage = (nameRequest as Entry).Text;

            await MopupService.Instance.PopAsync();
            await MopupService.Instance.PushAsync(new PopupPutBoxView(ActionBoxTap));
        }

        [RelayCommand]
        public async void OpenPopupPutBox()
        {
            await MopupService.Instance.PopAsync();
            await MopupService.Instance.PushAsync(new PopupPutBoxView(ActionBoxTap));
        }
    }
}
