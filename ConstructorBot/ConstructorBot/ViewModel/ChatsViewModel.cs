using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConstructorBot.Model;
using ConstructorBot.Model.ChatModel;
using ConstructorBot.Mopups;
using ConstructorBot.View;
using Mopups.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBot.ViewModel
{
    public partial class ChatsViewModel : ObservableObject
    {
        public ObservableCollection<LogicUser> Users { get; set; }

        public ChatsViewModel(List<LogicUser> logicUsers)
        {
            Users = new ObservableCollection<LogicUser>(logicUsers);
            //Users = new()
            //{
            //    new LogicUser()
            //    {
            //        FirstName = "Daniil",
            //        LastName = "Turlov"
            //    },
            //    new LogicUser()
            //    {
            //        FirstName = "Hello",
            //        LastName = "World"
            //    }
            //};      
        }

        [RelayCommand]
        public async void GoToPageChatUser(object sender)
        {
            var popupLoading = new PopupLoading();
            await MopupService.Instance.PushAsync(popupLoading);

            await Shell.Current.GoToAsync(nameof(ChatUserView), true, new Dictionary<string, object>()
            {
                ["LogicUser"] = (LogicUser)sender
            });

            await MopupService.Instance.RemovePageAsync(popupLoading);
        }

        [RelayCommand]
        public async void BackToMainPage()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
