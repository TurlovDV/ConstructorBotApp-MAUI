using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConstructorBot.Controls;
using ConstructorBot.Model;
using ConstructorBot.Model.Action;
using ConstructorBot.Model.ChatModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Keyboard = ConstructorBot.Model.Action.Keyboard;

namespace ConstructorBot.ViewModel.ChatPageViewModel
{
    public partial class ChatUserViewModel : ObservableObject
    {
        public ObservableCollection<ChatMessageModel> ChatMessageModels { get; set; }

        [ObservableProperty]
        LogicUser logicUser;

        public ChatUserViewModel(LogicUser logicUser)
        {
            LogicUser = logicUser;
            ChatMessageModels = new ObservableCollection<ChatMessageModel>(logicUser.ChatMessage);


            /*
            //ChatMessageModels = new()
            //{
            //    new ChatMessageModel()
            //    {
            //        IsBot = true,
            //        Message = "Hello",
            //        DateTime = DateTime.Now,
            //        Keyboard = new ConstructorBot.Model.Action.Keyboard()
            //        {
            //            KeyboardItems = new ObservableCollection<ConstructorBot.Model.Action.KeyboardItem>()
            //            {
            //                new KeyboardItem()
            //                {
            //                    Text = "Hello",
            //                    Row = 0,
            //                    Column = 0,
            //                },
            //                new KeyboardItem()
            //                {
            //                    Text = "Hello",
            //                    Row = 0,
            //                    Column = 1,
            //                    IsEnabled = true
            //                },
            //                new KeyboardItem()
            //                {
            //                    Text = "Hello",
            //                    Row = 0,
            //                    Column = 2,                                
            //                    IsEnabled = true
            //                },
            //                new KeyboardItem()
            //                {
            //                    Text = "Hello",
            //                    Row = 1,
            //                    Column = 0,
            //                },
            //                new KeyboardItem()
            //                {
            //                    Text = "Hello",
            //                    Row = 1,
            //                    Column = 1,
            //                },
            //                new KeyboardItem()
            //                {
            //                    Text = "Hello",
            //                    Row = 1,
            //                    Column = 2,
            //                },
            //                new KeyboardItem()
            //                {
            //                    Text = "Hello",
            //                    Row = 2,
            //                    Column = 0,
            //                },
            //                new KeyboardItem()
            //                {
            //                    Text = "Hello",
            //                    Row = 2,
            //                    Column = 1,
            //                },
            //                new KeyboardItem()
            //                {
            //                    Text = "Hello",
            //                    Row = 2,
            //                    Column = 2,
            //                },
            //            }
            //        }
            //    },
            //    new ChatMessageModel()
            //    {
            //        IsBot = false,
            //        Message = "Да, я знаю",
            //        DateTime = DateTime.Now,
            //        Keyboard = new()
            //    },
            //    new ChatMessageModel()
            //    {
            //        IsBot = true,
            //        Message = "А как оно идет",
            //        DateTime = DateTime.Now,
            //        Keyboard = new()
            //    },
            //    new ChatMessageModel()
            //    {
            //        IsBot = false,
            //        Message = "Нет кто уже когда",
            //        DateTime = DateTime.Now,
            //        Keyboard = new()
            //    }
            //};
            */
        }

        [RelayCommand]
        public async void BackToPageChats()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
