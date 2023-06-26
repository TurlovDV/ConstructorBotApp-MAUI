using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConstructorBot.Model.Action;
using ConstructorBot.View.PopupConstructorPageView;
using ConstructorBot.ViewModel.ConstructorPageViewModel;
using Mopups.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Requests;

namespace ConstructorBot.ViewModel.PopupConstructorViewModel
{
    public partial class PopupAddButtonsViewModel : ObservableObject
    {
        [ObservableProperty]
        ActionBox actionBoxTap;

        public PopupAddButtonsViewModel(ActionBox actionBoxTap)
        {
            ActionBoxTap = actionBoxTap;
        }

        [RelayCommand]
        public async void OpenPopupPutBox()
        {            
            await MopupService.Instance.PopAsync();
            await MopupService.Instance.PushAsync(new PopupPutBoxView(ActionBoxTap));
        }

        [RelayCommand]
        public void AddButton(object button)
        {
            var item = button as KeyboardItem;
            item.IsEnabled = !item.IsEnabled;
        }

        [RelayCommand]
        public async void OpenPutButton(object button)
        {
            await MopupService.Instance.PopAsync();
            await MopupService.Instance.PushAsync(new PopupPutButtonView(button as KeyboardItem, ActionBoxTap));
        }

    }
}
