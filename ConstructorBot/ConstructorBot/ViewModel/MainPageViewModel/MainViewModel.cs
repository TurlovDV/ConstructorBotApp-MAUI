using ConstructorBot.SaveData;
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


        private List<ActionBox> _actions;
        
        //public ScoreboardMessagerInfo MessagerInfo { get; set; }

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
            MessagerCore = new Domain();
            //MessagerInfo = new ScoreboardMessagerInfo();
            TelegramToken = Storage.GetKey();
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
                        var resultOnStart = await MessagerCore.UpdateBot(LogicMatrixConverter.GetParentActions(_actions)).OnStart(TelegramToken);
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
