using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConstructorBot.Language;
using ConstructorBot.Model.Action;
using ConstructorBot.Services.PopupService;
using ConstructorBot.ViewModel.ConstructorPageViewModel;
using Mopups.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBot.ViewModel.PopupConstructorViewModel
{
    public partial class PopupCreateActionViewModel : ObservableObject
    {
        ObservableCollection<ActionBox> actionBoxes;

        public PopupCreateActionViewModel(ObservableCollection<ActionBox> actionBoxes)
        {
            this.actionBoxes = actionBoxes;
        }

        [RelayCommand]
        public void CreateActionBox(object sender)
        {
            var nam = (sender as Entry).Text;
            if (nam == "" || nam == null)
                nam = LocalizationResourceManager.Instance["Message"] + $" {actionBoxes.Count}";

            actionBoxes.Add(new ActionBoxBuilder()
                .BuildMessageText(LocalizationResourceManager.Instance["Message"].ToString())
                .BuildQuestion("")
                .BuildNaming(nam)
                .GetActionBox());

            CloseThisPopup();
        }

        [RelayCommand]
        public async void CloseThisPopup()
        {
            await MopupService.Instance.PopAsync();
        }
    }
}
