using ConstructorBot.Model.Action;
using ConstructorBot.Services;
using ConstructorBot.Services.PopupService;
using ConstructorBot.Services.ServiceStorage;
using ConstructorBot.ViewModel.ConstructorPageViewModel;
using ConstructorBotMessengerApi;
using ConstructorBotMessengerApi.Model;
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
    public class MainViewModel : BaseViewModel
    {
        private bool isStart;
        private bool isPageOptionsToken;
        private bool isPageViewRequest;
        private bool isPageMain = true;
        private bool isInternetConnection = true;
        private bool isPageSettingsApplaction;
        private IStorageService storageService;
        private IMessageService messageService;
        private IBuildMessengerService buildMessengerService;
        private Services.IControlMessengerService controlMessengerService;
        private ConstructorBotMessengerApi.IMessengerService messengerCoreService;
        private string connectionToken;
        private bool isPageLoading;
        public InfoTable InfoTable { get; set; } = new();
        public ObservableCollection<LogicUser> SaveMesseges { get; set; }

        public bool IsPageLoading
        {
            get => isPageLoading;
            set
            {
                isPageLoading = value;
                OnPropertyChanged();
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

        public string ConnectionToken
        {
            get => connectionToken;
            set
            {
                connectionToken = value;
                OnPropertyChanged();
            }
        }
        
        public MainViewModel(IStorageService storageService,  
            IMessageService messageService,
            IBuildMessengerService buildMessengerService,
            ConstructorBotMessengerApi.IMessengerService messengerCoreService)
        {
            SaveMesseges = new ObservableCollection<LogicUser>();
            this.storageService = storageService;
            this.messageService = messageService;
            this.buildMessengerService = buildMessengerService;
            this.messengerCoreService = messengerCoreService;
            messengerCoreService.OnNewLogger += NewNotification;
        }

        private Task updateTableTask;

        public async Task UpdateInfoTable()
        {
            while (IsStart)
            {
                await Task.Delay(1000);
                InfoTable.OnStartTiming = InfoTable.OnStartTiming.AddSeconds(1);
                var logger = controlMessengerService.GetLogger();
                InfoTable.CountMessage = logger.CountMessage;
                InfoTable.CountUsers = logger.CountUsers;
                InfoTable.PingInternet = new Random().Next(30, 70);

                if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                {
                    if (!IsInternetConnection)
                    {
                        await controlMessengerService.Restart();
                        IsInternetConnection = true;
                    }
                }
                else
                    IsInternetConnection = false;

                if (InfoTable.OnStartTiming.Minute % 5 == 0 && InfoTable.OnStartTiming.Second == 0 && InfoTable.OnStartTiming.Minute != 0)
                {
                    await controlMessengerService.Restart();
                }
            }
        }
        
        //Начало работы
        public ICommand OnStartCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (!IsStart)
                    {
                        controlMessengerService = buildMessengerService.Initialization(LogicMatrixConverter.GetParentActions(
                            storageService.GetActions()), storageService.GetConnectionToken());

                        var resultOnStart = await controlMessengerService.Start();
                        if (resultOnStart != null)
                        {
                            
                            InfoTable.NamingBot = resultOnStart.Name;
                            IsStart = !IsStart;

                            InfoTable.OnStartTiming = new DateTime();
                            if (updateTableTask == null || updateTableTask.Status == TaskStatus.RanToCompletion)
                                updateTableTask = UpdateInfoTable();
                            
                            isInternetConnection = true;
                        }
                        else
                        {
                            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
                                await messageService.ShowAsync("Ошибка", "Нет подключения к интернету");
                            else
                                await messageService.ShowAsync("Ошибка", "Убедитесь в достоверности токена");
                        }
                    }
                    else
                    {
                        controlMessengerService.Stop();
                        IsStart = !IsStart;
                    }
                });
            }
        }

        //Для отслеживания одного нажатия для прехода на страницу ConstructorPage
        bool isOneTap = false;
        public bool IsOneTap
        {
            get => isOneTap;
            set 
            {
                isOneTap = value;
                OnPropertyChanged();
            }
        }
        
        //Перейти к странице коснтсруктора
        public ICommand GoToConstructorPage
        {
            get
            {
                return new Command(async () =>
                {
                    IsOneTap = false;
                    IsPageLoading = true;
                    await Shell.Current.GoToAsync("ConstructorPage");
                    IsPageLoading = false;
                    IsOneTap = true;
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

        //Открыть настройки воода токена
        public ICommand OpenOptionsTokenCommand
        {
            get
            {
                return new Command(() =>
                {
                    ConnectionToken = storageService.GetConnectionToken();
                    IsPageMain = false;
                    IsPageOptionsToken = true;
                });
            }
        }


        private bool isNewNotification;
        public bool IsNewNotification
        {
            get => isNewNotification;
            set
            {
                isNewNotification = value;
                OnPropertyChanged();
            }
        }

        //Новое сохраенное сообщение
        public void NewNotification(Logger logger)
        {            
             IsNewNotification = true;            
        }

        //Открыть / закрыть окно просмотра сохраненных заявок
        public ICommand OpenOrExitPageViewRequest
        {
            get
            {
                return new Command(() =>
                {
                    IsNewNotification = false;
                    IsPageMain = !IsPageMain;
                    IsPageViewRequest = !IsPageViewRequest;

                    if (IsPageViewRequest)
                    {
                        SaveMesseges.Clear();
                        if (controlMessengerService is not null)
                        {
                            var actions = controlMessengerService.GetLogger();
                            if (actions is not null)
                                actions.LogicUsers
                                    .ForEach(x => SaveMesseges.Add(new LogicUser()
                                    {
                                        FirstName = x.FirstName,
                                        LastName = x.LastName,
                                        SaveAnswers = new ObservableCollection<SaveMessageItem>(x.SaveMessage.ToList()
                                            .Select(y => new SaveMessageItem() { Answer = y.Value, Naming = y.Key }).ToList())
                                    }));
                        }
                    }
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
                    if(ConnectionToken.Trim() != "" || ConnectionToken.Trim() != null)
                        storageService.SetConnectionToken(ConnectionToken.Trim());
                    IsPageMain = true;
                    IsPageOptionsToken = false;
                });
            }
        }     
    }
}
