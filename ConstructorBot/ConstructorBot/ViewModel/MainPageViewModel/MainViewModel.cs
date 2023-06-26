using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConstructorBot.Model;
using ConstructorBot.Model.Action;
using ConstructorBot.Services;
using ConstructorBot.Services.PopupService;
using ConstructorBot.Services.ServiceStorage;
using ConstructorBot.View;
using ConstructorBot.ViewModel.ConstructorPageViewModel;
using ConstructorBotMessengerApi;
using ConstructorBotMessengerApi.Model;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;
using Sentry;
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
using LogicUser = ConstructorBot.Model.LogicUser;

namespace ConstructorBot.ViewModel.MainPageViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        #region Sevices
        private IStorageService storageService;
        private IMessageService messageService;
        private IBuildMessengerService buildMessengerService;
        private Services.IControlMessengerService controlMessengerService;
        private ConstructorBotMessengerApi.IMessengerService messengerCoreService;
        #endregion

        #region Properties
        [ObservableProperty]
        private bool isStart;
        [ObservableProperty]
        private bool isPageOptionsToken;
        [ObservableProperty]
        private bool isPageViewRequest;
        [ObservableProperty]
        private bool isPageMain = true;
        [ObservableProperty]
        private bool isInternetConnection = true;
        [ObservableProperty]
        private bool isPageSettingsApplaction;
        [ObservableProperty]
        private bool isPageSendReport;
        [ObservableProperty]
        private string connectionToken;
        [ObservableProperty]
        private bool isPageLoading;
        [ObservableProperty]
        private bool isNewNotification;
        #endregion

        [ObservableProperty]
        bool isOneTap = false;

        //Новое сохраенное сообщение
        bool isTimeNewNotification;

        #region PropertiesSettings
        public bool IsBackgroundTask
        {
            get => storageService.GetOptions(nameof(IsBackgroundTask));
            set
            {
                storageService.SetOptions(nameof(IsBackgroundTask), value);
                OnPropertyChanged();
            }
        }

        public bool IsNotificationNewMessage
        {
            get => storageService.GetOptions(nameof(IsNotificationNewMessage));
            set
            {
                storageService.SetOptions(nameof(IsNotificationNewMessage), value);
                OnPropertyChanged();
            }
        }

        public bool IsNotificationNewUser
        {
            get => storageService.GetOptions(nameof(IsNotificationNewUser));
            set
            {
                storageService.SetOptions(nameof(IsNotificationNewUser), value);
                OnPropertyChanged();
            }
        }

        public bool IsNotificationNewSaveAnswer
        {
            get => storageService.GetOptions(nameof(IsNotificationNewSaveAnswer));
            set
            {
                storageService.SetOptions(nameof(IsNotificationNewSaveAnswer), value);
                OnPropertyChanged();
            }
        }
        #endregion

        public InfoTable InfoTable { get; set; }
        public ObservableCollection<LogicUser> SaveMesseges { get; set; }

        public MainViewModel(IStorageService storageService,
            IMessageService messageService,
            IBuildMessengerService buildMessengerService,
            ConstructorBotMessengerApi.IMessengerService messengerCoreService)
        {
            InfoTable = new();
            SaveMesseges = new ObservableCollection<LogicUser>();
            this.storageService = storageService;
            this.messageService = messageService;
            this.buildMessengerService = buildMessengerService;
            this.messengerCoreService = messengerCoreService;
            messengerCoreService.OnNewNotification += NewNotification;


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

        [RelayCommand]
        public async void StartBot()
        {
            if (!IsStart)
            {
                IsPageLoading = true;

                controlMessengerService = buildMessengerService.Initialization(LogicMatrixConverter.GetParentActions(
                    storageService.GetActions()), storageService.GetConnectionToken());

                var resultOnStart = await controlMessengerService.Start(IsBackgroundTask);

                IsPageLoading = false;

                if (resultOnStart != null)
                {
                    InfoTable.NamingBot = resultOnStart.Name;
                    IsStart = !IsStart;

                    InfoTable.OnStartTiming = new DateTime();
                    if (updateTableTask == null || updateTableTask.Status == TaskStatus.RanToCompletion)
                        updateTableTask = UpdateInfoTable();

                    IsInternetConnection = true;
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
        }

        [RelayCommand]
        public async void GoToConstructorPage()
        {
            IsOneTap = false;
            IsPageLoading = true;
            await Shell.Current.GoToAsync("ConstructorPage");
            IsPageLoading = false;
            IsOneTap = true;
        }

        [RelayCommand]
        public void OpenSettingApplication()
        {
            IsPageMain = !IsPageMain;
            IsPageSettingsApplaction = !IsPageSettingsApplaction;
        }

        [RelayCommand]
        public void OpenOptionsToken()
        {
            ConnectionToken = storageService.GetConnectionToken();
            IsPageMain = !IsPageMain;
            IsPageOptionsToken = !IsPageOptionsToken;
        }

        [RelayCommand]
        public void OpenOrExitPageViewRequest()
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
                            .ForEach(x =>
                            {
                                if (x.SaveMessage.Count > 0)
                                    SaveMesseges.Add(new LogicUser()
                                    {
                                        FirstName = x.FirstName,
                                        LastName = x.LastName,
                                        SaveAnswers = new ObservableCollection<SaveMessageItem>(x.SaveMessage.ToList()
                                            .Select(y => new SaveMessageItem()
                                            { Answer = y.Value, Naming = y.Key }).ToList())
                                    });
                            });

                }
            }
        }

        [RelayCommand]
        public void ViewSaveMessageCommand(object sender)
        {
            (sender as StackLayout).IsVisible = !(sender as StackLayout).IsVisible;
        }

        [RelayCommand]
        public void SaveOptionsToken()
        {
            if (ConnectionToken.Trim() != "" || ConnectionToken.Trim() != null)
                storageService.SetConnectionToken(ConnectionToken.Trim());
            IsPageMain = true;
            IsPageOptionsToken = false;
        }

        [RelayCommand]
        public async void OpenBotFather()
        {
            Uri uri = new Uri("https://t.me/BotFather");
            await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }

        [RelayCommand]
        public void OpenPageSendReport()
        {
            IsPageMain = !IsPageMain;
            IsPageSendReport = !IsPageSendReport;
        }

        [RelayCommand]
        public async void GoToPageChats()
        {
            List<LogicUser> logicUsers = new();

            if (messengerCoreService.GetLogger() is not null)
            messengerCoreService.GetLogger().LogicUsers
                .ForEach(x =>
                {
                    var logicUser = new LogicUser()
                    {
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                    };
                    x.ChatMessage.ForEach(y =>
                    {
                        
                        //int row = 0;
                        //foreach (var buttons in y.Keyboard.ArrayButton)
                        //{
                        //    int column = 0;
                        //    foreach (var button in buttons)
                        //    {
                        //        keyboard.KeyboardItems.First(x => x.Column == column && x.Row == row).Text = button.
                        //    }
                        //}

                        logicUser.ChatMessage.Add(new Model.ChatModel.ChatMessageModel()
                        {
                            IsBot = y.FirstName is not null ? false : true,
                            DateTime = DateTime.Now,
                            Message = y.Text
                        });
                    });
                    logicUsers.Add(logicUser);
                });
            await Shell.Current.GoToAsync(nameof(ChatsView), true, new Dictionary<string, object>()
            {
                ["LogicUsers"] = logicUsers
            });
        }

        public void NewNotification(Notification notification)
        {
            NotificationRequest notificationRequest = null!;


            switch (notification.NotificationType)
            {
                case NotificationType.NewSaveAnswer:
                    if (IsNotificationNewSaveAnswer)
                    {
                        notificationRequest = new NotificationRequest()
                        {
                            Title = "Новая заявка",
                            Description = notification.Message.FirstName + " заполнил заявку",
                            BadgeNumber = 42,
                            CategoryType = NotificationCategoryType.Event
                        };

                        if (!isTimeNewNotification
                            && Shell.Current.CurrentPage.ToString() == "ConstructorBot.MainPage")
                            messageService.ShowNotification("Новая заявка", notification.Message.FirstName + " заполнил заявку", 700);
                    }
                    IsNewNotification = true;
                    break;
                case NotificationType.NewUser:
                    if (IsNotificationNewUser)
                    {
                        notificationRequest = new NotificationRequest()
                        {
                            Title = "Новый пользователь",
                            Description = notification.Message.FirstName + " написал боту",
                            BadgeNumber = 42,
                            CategoryType = NotificationCategoryType.Event
                        };

                        if (!isTimeNewNotification
                            && Shell.Current.CurrentPage.ToString() == "ConstructorBot.MainPage")
                            messageService.ShowNotification("Новый пользователь", notification.Message.FirstName + " написал боту", 700);
                    }
                    break;
                case NotificationType.NewMessage:
                    if (IsNotificationNewMessage)
                    {
                        notificationRequest = new NotificationRequest()
                        {
                            Title = "Новое сообщение",
                            Description = notification.Message.FirstName + " написал боту",
                            BadgeNumber = 42,
                            CategoryType = NotificationCategoryType.Event
                        };

                        if (!isTimeNewNotification
                            && Shell.Current.CurrentPage.ToString() == "ConstructorBot.MainPage")
                            messageService.ShowNotification("Новое сообщение", notification.Message.FirstName + " написал боту", 700);

                    }
                    break;
            }
            if (notificationRequest is not null)
                LocalNotificationCenter.Current.Show(notificationRequest);

            if (!isTimeNewNotification)
            {
                isTimeNewNotification = true;
                new Task(async () =>
                {
                    await Task.Delay(1000);
                    isTimeNewNotification = false;
                }).Start();
            }
        }
    }
}
