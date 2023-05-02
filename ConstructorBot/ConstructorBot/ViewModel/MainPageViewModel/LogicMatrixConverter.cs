using ConstructorBot.ViewModel.ConstructorPageViewModel.Action;
using ConstructorBotCore.UserModel.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBot.ViewModel.MainPageViewModel
{
    public static class LogicMatrixConverter
    {
        private static void GetInlineKeyboard(ActionBox actionBox, ref DomainChildAction domainChild)
        {
            domainChild.MessageAnswer.KeyboardButton = new ConstructorBotCore.DomainMessagModel.ElementMessage.DomainKeyboardButton();
            domainChild.MessageAnswer.KeyboardButton.inline_keyboard = new List<List<ConstructorBotCore.DomainMessagModel.ElementMessage.DomainButton>>();

            int kRow = -1;
            int i = 0;
            int column = 0;
            while (i < 3)
            {
                foreach (var item in actionBox.Keyboard.KeyboardItems)
                {
                    if (item.Row == i && item.IsEnabled)
                    {
                        if (column == 0)
                        {
                            kRow++;
                            domainChild.MessageAnswer.KeyboardButton.inline_keyboard.Add(new List<ConstructorBotCore.DomainMessagModel.ElementMessage.DomainButton>());
                        }

                        if (item.Text != "")
                            domainChild.MessageAnswer.KeyboardButton.inline_keyboard[kRow]
                                .Add(new ConstructorBotCore.DomainMessagModel.ElementMessage.DomainButton()
                                {
                                    text = item.Text,
                                    callback_data = item.Text,
                                    url = item.Url
                                });
                        else
                            domainChild.MessageAnswer.KeyboardButton.inline_keyboard[kRow]
                                .Add(new ConstructorBotCore.DomainMessagModel.ElementMessage.DomainButton()
                                {
                                    text = "Кнопка",
                                    callback_data = "Кнопка",
                                    url = item.Url
                                });

                        column++;
                        if (column == 3)
                        {
                            i++;
                            column = 0;
                            continue;
                        }
                    }
                }
                column = 0;
                i++;
            }
        }
        private static void GetReplyKeyboard(ActionBox actionBox, ref DomainChildAction domainChild)
        {
            domainChild.MessageAnswer.ReplyKeyboardMarkup = new ConstructorBotCore.DomainMessagModel.ElementMessage.DomainReplyKeyboardMarkup();
            domainChild.MessageAnswer.ReplyKeyboardMarkup.Keyboard = new List<List<string>>();

            int kRow = -1;
            int i = 0;
            int column = 0;
            while (i < 3)
            {
                foreach (var item in actionBox.Keyboard.KeyboardItems)
                {
                    if (item.Row == i && item.IsEnabled)
                    {
                        if (column == 0)
                        {
                            kRow++;
                            domainChild.MessageAnswer.ReplyKeyboardMarkup.Keyboard.Add(new List<string>());
                        }

                        if (item.Text != "")
                            domainChild.MessageAnswer.ReplyKeyboardMarkup.Keyboard[kRow]
                                .Add(item.Text);
                        else
                            domainChild.MessageAnswer.ReplyKeyboardMarkup.Keyboard[kRow]
                            .Add("Конпка");


                        column++;
                        if (column == 3)
                        {
                            i++;
                            column = 0;
                            continue;
                        }
                    }
                }
                column = 0;
                i++;
            }
        }
        private static void GetMedia(ActionBox actionBox, ref DomainChildAction domainChild)
        {
            actionBox.MediaItems =
                new System.Collections.ObjectModel.ObservableCollection<MediaItem>(
                    actionBox.MediaItems.ToList().Where(x => File.Exists(x.PathMediaSource)));

            //for i in range(len(strings)):
            //    for j in range(len(i)):

            //            List<string> strings = new();
            //for(int i = 0; i < strings.Count; i++)
            //    for(int j = 0; j < strings[i].Length; j++)
            //        strings[i][j]


            if (actionBox.MediaItems.Count == 0)
                return;
            if (actionBox.MediaItems.Count == 1)
            {
                switch (actionBox.MediaItems[0].Type)
                {
                    case ConstructorPageViewModel.Action.MediaType.Photo:
                        domainChild.MessageAnswer.MessageType = ConstructorBotCore.DomainMessageModel.MessageType.Photo;
                        domainChild.MessageAnswer.Photo = new ConstructorBotCore.DomainMessagModel.ElementMessage.DomainPhoto()
                        {
                            //File = actionBox.MediaItems[0].Bytes                           
                            File = File.ReadAllBytes(actionBox.MediaItems[0].PathMediaSource)
                        };
                        return;
                    case ConstructorPageViewModel.Action.MediaType.Video:
                        domainChild.MessageAnswer.MessageType = ConstructorBotCore.DomainMessageModel.MessageType.Video;
                        domainChild.MessageAnswer.Video = new ConstructorBotCore.DomainMessageModel.ElementMessage.DomainVideo()
                        {
                            //File = actionBox.MediaItems[0].Bytes
                            File = File.ReadAllBytes(actionBox.MediaItems[0].PathMediaSource)
                        };
                        return;
                    case ConstructorPageViewModel.Action.MediaType.Audio:
                        domainChild.MessageAnswer.MessageType = ConstructorBotCore.DomainMessageModel.MessageType.Audio;
                        domainChild.MessageAnswer.Audio = new ConstructorBotCore.DomainMessageModel.ElementMessage.DomainAudio()
                        {
                            File = File.ReadAllBytes(actionBox.MediaItems[0].PathMediaSource)
                            //    File = actionBox.MediaItems[0].Bytes
                        };
                        return;
                    case ConstructorPageViewModel.Action.MediaType.Document:
                        domainChild.MessageAnswer.Document = new ConstructorBotCore.DomainMessageModel.ElementMessage.DomainDocument()
                        {
                            File = File.ReadAllBytes(actionBox.MediaItems[0].PathMediaSource)
                            //    File = actionBox.MediaItems[0].Bytes
                        };
                        domainChild.MessageAnswer.MessageType = ConstructorBotCore.DomainMessageModel.MessageType.Audio;
                        return;
                }
            }

            domainChild.MessageAnswer.MessageType = ConstructorBotCore.DomainMessageModel.MessageType.GroupMedia;
            foreach (var item in actionBox.MediaItems)
            {
                domainChild.MessageAnswer.MediaGroups.Add(new ConstructorBotCore.DomainMessagModel.ElementMessage.DomainMediaGroup()
                {
                    File = File.ReadAllBytes(item.PathMediaSource),
                    //File = item.Bytes,
                    type = item.Type switch
                    {
                        ConstructorPageViewModel.Action.MediaType.Photo
                            => ConstructorBotCore.DomainMessagModel.ElementMessage.MediaType.Photo,
                        ConstructorPageViewModel.Action.MediaType.Video
                            => ConstructorBotCore.DomainMessagModel.ElementMessage.MediaType.Video,
                        ConstructorPageViewModel.Action.MediaType.Audio
                            => ConstructorBotCore.DomainMessagModel.ElementMessage.MediaType.Audio,
                        _ => ConstructorBotCore.DomainMessagModel.ElementMessage.MediaType.Document
                    }
                });
            }
        }
        private static DomainChildAction GetChildAction(ActionBox actionBox)
        {
            DomainChildAction domainChild = new DomainChildAction();
            domainChild.Question = actionBox.Question;
            domainChild.MessageAnswer = new ConstructorBotCore.DomainMessageModel.DomainMessage()
            {
                Text = actionBox.MessageText
            };

            if (actionBox.Keyboard.IsInline)
                GetInlineKeyboard(actionBox, ref domainChild);
            else
                GetReplyKeyboard(actionBox, ref domainChild);

            GetMedia(actionBox, ref domainChild);

            domainChild.SaveName = actionBox.NameSaveMessage;

            return domainChild;
        }
        public static List<DomainParentAction> GetParentActions(List<ActionBox> _actions)
        {
            //_messageService.ShowAsync("Откладка бота...", "1");

            List<DomainParentAction> result = new List<DomainParentAction>();
            int numParent = 1;
            var logic = new List<DomainParentAction>();
            List<ActionBox> acts = new List<ActionBox>(_actions);

            //acts = Actions.Select(x => new Action(x));

            var c = GetChildAction(acts[0]);
            c.Id = acts[0].Id;
            result.Add(new DomainParentAction()
            {
                NumberMainAction = 0,
                ChildActions = new List<DomainChildAction>()
                {
                    c
                }
            });

            foreach (var action in acts)
            {
                DomainParentAction parentAction = new DomainParentAction();
                parentAction.ChildActions = new List<DomainChildAction>();
                parentAction.NumberMainAction = numParent;
                parentAction.Id = action.Id;
                numParent++;
                if (action.ConnectionActions.Count == 0)
                    continue;

                foreach (var child in action.ConnectionActions)
                {
                    //ChildAction childAction = child.Connect.ChildAction;
                    DomainChildAction childAction = GetChildAction(_actions.First(x => x.Id == child.Connect.Id)); //new
                    childAction.Id = child.Connect.Id;//new
                    parentAction.ChildActions.Add(childAction);
                }
                result.Add(parentAction);
            }

            foreach (var parent in result)
            {
                if (parent.ChildActions == null)
                    continue;
                foreach (var child in parent.ChildActions)
                {
                    var action = acts.First(x => x.Id == child.Id); //Actions.
                    if (action.ConnectionActions.Count == 0)
                    {
                        child.ForwardAction = parent.NumberMainAction;
                    }
                    foreach (var connection in action.ConnectionActions)
                    {
                        foreach (var parent2 in result)
                        {
                            if (parent2.ChildActions == null)
                                continue;
                            foreach (var child2 in parent2.ChildActions)
                                if (child2.Id == connection.Connect.Id)
                                    child.ForwardAction = parent2.NumberMainAction;
                        }
                    }
                }
            }
            return result;
        }
    }
}
