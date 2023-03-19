using ConstructorBot.ViewModel.ConstructorPageViewModel;
using ConstructorBot.ViewModel.ConstructorPageViewModel.Action;
using ConstructorBotCore;
using ConstructorBotCore.DomainMessagModel.ElementMessage;
using ConstructorBotCore.UserModel.Action;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static ConstructorBot.App;

namespace ConstructorBot.ViewModel.MainPageViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private bool isStart;

        private DateTime onStartTiming;

        private string namingBot;

        private int countMessage;
        private int countProfile;
        private int speedInternet;
        private bool isPageOptionsToken;
        private bool isPageMain = true;
        private bool isInternetConnection = true;


        private List<ActionBox> _actions;
        public ScoreboardMessagerInfo MessagerInfo { get; set; }

        public bool IsPageOptionsToken
        { 
            get => isPageOptionsToken; 
            set
            {
                isPageOptionsToken = value;
                OnPropertyChanged();
            }
        }

        public bool IsPageMain
        {
            get => isPageMain;
            set
            {
                isPageMain = value;
                OnPropertyChanged();
            }
        }

        public Domain MessagerCore { get; set; }

        public bool IsInternetConnection
        {
            get => isInternetConnection;
            set
            {
                isInternetConnection = value;
                OnPropertyChanged();
            }
        }

        public int CountMessage
        { 
            get => countMessage; 
            set
            {
                countMessage = value;
                OnPropertyChanged();
            }
        }

        public int CountProfile
        {
            get => countProfile;
            set
            {
                countProfile = value;
                OnPropertyChanged();
            }
        }

        public int SpeedInternet
        {
            get => speedInternet;
            set
            {
                speedInternet = value;
                OnPropertyChanged();
            }
        }

        public DateTime OnStartTiming
        {
            get => onStartTiming;
            set
            {
                onStartTiming = value;
                OnPropertyChanged();
            }
        }

        public string NamingBot 
        {
            get => namingBot; 
            set
            {
                namingBot = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            MessagerCore = new Domain();
            MessagerInfo = new ScoreboardMessagerInfo();
        }

        //Запуск бара с инфо о боте в 1_сек

        public async void UpdateInfoTable()
        {
            OnStartTiming = new DateTime();
            while(IsStart)
            {
                await Task.Delay(1000);
                OnStartTiming = OnStartTiming.AddSeconds(1);
                var logger = MessagerCore.GetLogger();
                CountMessage = logger.CountMessage;
                CountProfile = logger.CountProfile;
                SpeedInternet = new Random().Next(30, 70);

                if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                {
                    if(!IsInternetConnection)
                    {
                        await MessagerCore.OnStartRestart();
                        IsInternetConnection = true;
                    }
                }
                else
                    IsInternetConnection = false;

                if (OnStartTiming.Minute % 5 == 0 && OnStartTiming.Second == 0 && OnStartTiming.Minute != 0)
                {
                    await MessagerCore.OnStartRestart();
                }
            }            
        }

        //Кнопка старт/стоп
        public ICommand OnStartCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (!IsStart)
                    {
                        _actions = ServiceProvider.GetService<ConstructorViewModel>().ActionBoxes.ToList();
                        var resultOnStart = await MessagerCore.UpdateBot(GetParentActions()).OnStart();
                        if (resultOnStart)
                        {
                            NamingBot = MessagerCore.GetNamingBot();
                            var _backGround = ServiceProvider.GetService<IServiceDomainBot>();
                            _backGround.Start();
                            IsStart = !IsStart;
                            await Task.Run(UpdateInfoTable);
                            isInternetConnection = true;
                        }
                        else
                        {
                            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                                await DependencyService.Get<App.IMessageService>().ShowAsync("Ошибка", "Убедитесь в достоверности подключения к интернету");
                            else
                            await DependencyService.Get<App.IMessageService>().ShowAsync("Ошибка", "Убедитесь в достоверности токена");
                        }
                    }
                    else
                    {
                        MessagerCore.OnStop();
                        var _backGround = ServiceProvider.GetService<IServiceDomainBot>();
                        _backGround.Stop();
                        IsStart = !IsStart;
                    }

                });
            }
        }

        public bool IsStart
        { 
            get => isStart;
            set
            {
                isStart = value;
                OnPropertyChanged();
            }
        }
        public ICommand PushConstructorPage
        {
            get
            {
                return new Command(() =>
                {
                                
                });
            }
        }

        //Открыть настройки воода токена
        public ICommand OpenOptionsTokenCommand
        {
            get
            {
                return new Command(() =>
                {
                    //if (IsStart)
                    //{
                    //    var result = await ServiceProvider.GetService<IMessageService>().ShowOrOkAsync("Настройки бота", "Отключить бота ?");
                    //    if (result == "Нет")
                    //        return;
                    //    IsStart = false;
                    //}
                    
                    IsPageMain = false;
                    IsPageOptionsToken = true;
                });
            }
        }

        //Кнопка сохранения токена для бота
        public ICommand SaveOptionsTokenCommand
        {
            get
            {
                return new Command(() =>
                {
                    IsPageMain = true;
                    IsPageOptionsToken = false;
                });
            }
        }

        private void GetInlineKeyboard(ActionBox actionBox, ref DomainChildAction domainChild)
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

                        if(item.Text != "")
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
        private void GetReplyKeyboard(ActionBox actionBox, ref DomainChildAction domainChild)
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

                        if(item.Text != "")
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
        private void GetMedia(ActionBox actionBox, ref DomainChildAction domainChild)
        {
            actionBox.MediaItems = 
                new System.Collections.ObjectModel.ObservableCollection<MediaItem>(
                    actionBox.MediaItems.ToList().Where(x => File.Exists(x.PathMediaSource)));

            if (actionBox.MediaItems.Count == 0)
                return;
            if(actionBox.MediaItems.Count == 1)
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
        public DomainChildAction GetChildAction(ActionBox actionBox)
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

            return domainChild;
        }
        public List<DomainParentAction> GetParentActions()
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

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
