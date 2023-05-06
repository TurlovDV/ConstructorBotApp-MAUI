using ConstructorBot.SaveData;
using ConstructorBot.ViewModel.ConstructorPageViewModel;
using ConstructorBot.ViewModel.ConstructorPageViewModel.Action;
using ConstructorBotCore;
using ConstructorBotCore.DomainMessagModel.ElementMessage;
using ConstructorBotCore.UserModel.Action;
using MessengerBotApi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
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
        private bool isPageViewRequest;
        private bool isPageMain = true;
        private bool isInternetConnection = true;
        private string telegramToken;
        private bool isPageSettingsApplaction;

        private List<ActionBox> _actions;

        public ObservableCollection<LogicUser> SaveMesseges { get; set; }        

        public bool IsPageOptionsToken
        { 
            get => isPageOptionsToken; 
            set
            {
                isPageOptionsToken = value;
                OnPropertyChanged();
            }
        }

        public bool IsPageSettingsApplaction
        {
            get => isPageSettingsApplaction;
            set
            {
                isPageSettingsApplaction = value;
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

        public bool IsPageViewRequest
        {
            get => isPageViewRequest;
            set
            {
                isPageViewRequest = value;
                OnPropertyChanged();
            }
        }

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

        public string TelegramToken
        { 
            get => telegramToken; 
            set
            {
                telegramToken = value;
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
            SaveMesseges = new ObservableCollection<LogicUser>();
            //MessagerInfo = new ScoreboardMessagerInfo();
            TelegramToken = Storage.GetKey();
        }

        public async Task UpdateInfoTable()
        {
            while (IsStart)
            {
                await Task.Delay(1000);
                OnStartTiming = OnStartTiming.AddSeconds(1);
                var logger = MessagerCore.GetMessengerInfo();
                CountMessage = logger.CountMessage;
                CountProfile = logger.CountProfile;
                SpeedInternet = new Random().Next(30, 70);

                if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                {
                    if (!IsInternetConnection)
                    {
                        MessagerCore.Stop();
                        await MessagerCore.Start();
                        IsInternetConnection = true;
                    }
                }
                else
                    IsInternetConnection = false;

                if (OnStartTiming.Minute % 5 == 0 && OnStartTiming.Second == 0 && OnStartTiming.Minute != 0)
                {
                    MessagerCore.Stop();
                    await MessagerCore.Start();
                }
            }
        }


        private Task updateTableTask;

        public ICommand OnStartCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (!IsStart)
                    {
                        _actions = ServiceProvider.GetService<ConstructorViewModel>().ActionBoxes.ToList();
                        MessagerCore = new Domain(LogicMatrixConverter.GetParentActions(_actions), TelegramToken);
                        var resultOnStart = await MessagerCore.Start();
                        if (resultOnStart != null)
                        {
                            NamingBot = resultOnStart.Name;
                            var _backGround = ServiceProvider.GetService<IServiceDomainBot>();
                            _backGround.Start();
                            IsStart = !IsStart;

                            OnStartTiming = new DateTime();
                            if (updateTableTask == null || updateTableTask.Status == TaskStatus.RanToCompletion)
                                updateTableTask = UpdateInfoTable();
                            
                            isInternetConnection = true;
                        }
                        else
                        {
                            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                                await DependencyService.Get<App.IMessageService>().ShowAsync("Ошибка", "Нет подключения к интернету");
                            else
                                await DependencyService.Get<App.IMessageService>().ShowAsync("Ошибка", "Убедитесь в достоверности токена");
                        }
                    }
                    else
                    {
                        MessagerCore.Stop();
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


        //Открыть/закрыть настройки приложения
        public ICommand OpenSettingApplication
        {
            get
            {
                return new Command(() =>
                {
                    IsPageMain = !IsPageMain;
                    IsPageSettingsApplaction = !IsPageSettingsApplaction;
                });
            }
        }

        ////Сохранить настройки приложения
        //public ICommand SaveSettingApplication
        //{
        //    get
        //    {
        //        return new Command((object sender, EventArgs e) =>
        //        {
        //            IsPageMain = !IsPageMain;
        //            IsPageSettingsApplaction = !IsPageSettingsApplaction;
        //        });
        //    }
        //}

        //Открыть настройки воода токена
        public ICommand OpenOptionsTokenCommand
        {
            get
            {
                return new Command(() =>
                {
                    IsPageMain = false;
                    IsPageOptionsToken = true;
                });
            }
        }

        //Открыть / закрыть окно просмотра сохраненных заявок
        public ICommand OpenOrExitPageViewRequest
        {
            get
            {
                return new Command(() =>
                {
                    IsPageMain = !IsPageMain;
                    IsPageViewRequest = !IsPageViewRequest;

                    SaveMesseges.Clear();
                    if(MessagerCore != null)
                        MessagerCore.GetMessengerInfo().LogicUsers
                            .ForEach(x => SaveMesseges.Add(new LogicUser()
                            {
                                FirstName = x.FirstName,
                                LastName = x.LastName,
                                SaveAnswers = new ObservableCollection<SaveMessageItem>(x.SaveMessage.ToList()
                                    .Select(y => new SaveMessageItem() { Answer = y.Value, Naming = y.Key }).ToList())
                            }));                    
                });
            }
        }

        //Открыть подробнее заявку пользователя
        public ICommand ViewSaveMessage
        {
            get
            {
                return new Command((object sender) =>
                {
                    (sender as StackLayout).IsVisible = !(sender as StackLayout).IsVisible;
                    //(sender as LogicUser).IsView = !(sender as LogicUser).IsView;
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
                    if(TelegramToken.Trim() != "" || TelegramToken.Trim() != null)
                        Storage.SetKey(TelegramToken.Trim());
                    IsPageMain = true;
                    IsPageOptionsToken = false;
                });
            }
        }
     
        public event PropertyChangedEventHandler PropertyChanged;
        
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
