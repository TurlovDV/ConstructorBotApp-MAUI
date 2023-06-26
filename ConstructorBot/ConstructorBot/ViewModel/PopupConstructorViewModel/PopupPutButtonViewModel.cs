using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConstructorBot.Model.Action;
using ConstructorBot.View.PopupConstructorPageView;
using Mopups.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBot.ViewModel.PopupConstructorViewModel
{
    public partial class PopupPutButtonViewModel : ObservableObject
    {
        ActionBox actionBoxTap;

        [ObservableProperty]
        KeyboardItem keyboardItem;

        public PopupPutButtonViewModel(KeyboardItem keyboard, ActionBox actionBoxTap)
        {
            KeyboardItem = keyboard;
            this.actionBoxTap = actionBoxTap;
        }

        [RelayCommand]
        public async void OpenPopupAddButtons()
        {
            await MopupService.Instance.PopAsync();
            await MopupService.Instance.PushAsync(new PopupAddButtonsView(actionBoxTap));
        }
    }
}
