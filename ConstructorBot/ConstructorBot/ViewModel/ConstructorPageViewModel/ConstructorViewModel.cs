using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using static ConstructorBot.App;
using System.Reflection.Metadata.Ecma335;
using ConstructorBot.Language;
using ConstructorBot.Model.Action;
using ConstructorBot.Model.Action.ConnectionElement;
using ConstructorBot.Services.PopupService;
using ConstructorBot.Services.ServiceStorage;

namespace ConstructorBot.ViewModel.ConstructorPageViewModel
{
    public partial class ConstructorViewModel : BaseViewModel
    {
        private bool isPutBoxAction;
        private bool isAddButtons;
        private bool isPutButton;
        private bool isAddAction;
        private bool isAddRequest;
        private ActionBox tapLastAction;
        private IMessageService messageService;
        private IStorageService storageService;
        private KeyboardItem keyboardItem;

        public KeyboardItem KeyboardItem
        {
            get => keyboardItem;
            set
            {
                keyboardItem = value;
                OnPropertyChanged();
            }
        }

        public bool IsPutBoxAction
        {
            get => isPutBoxAction;
            set
            {
                isPutBoxAction = value;
                OnPropertyChanged();        
            }
        }

        public bool IsAddButtons
        {
            get => isAddButtons;
            set
            {
                isAddButtons = value;
                OnPropertyChanged();
            }
        }

        public bool IsAddRequest
        {
            get => isAddRequest;
            set
            {
                isAddRequest = value;
                OnPropertyChanged();
            }
        }

        public bool IsPutButton
        {
            get => isPutButton;
            set
            {
                isPutButton = value;
                OnPropertyChanged();
            }
        }

        public bool IsAddAction
        {
            get => isAddAction;
            set
            {
                isAddAction = value;
                OnPropertyChanged();
            }
        }

        public ActionBox TapLastAction
        {
            get => tapLastAction; 
            set
            {
                tapLastAction = value;
                OnPropertyChanged();
            }
        }

        
        public ActionBoxBuilder ActionBoxBuilder;
        
        public ObservableCollection<ActionBox> ActionBoxes { get; set; }
               

        //Вернуться к главному блоку
        public ICommand GoToMainBox
        {
            get
            {
                return new Command(() =>
                {
                    double X = ActionBoxes[0].TranslationX;
                    double Y = ActionBoxes[0].TranslationY;
                    ActionBoxes.ToList().ForEach(x =>
                    {
                        x.TranslationX -= X;
                        x.TranslationY -= Y;
                    });
                    ActionBoxes.ToList().ForEach(x => x.ConnectionActions.ToList()
                        .ForEach(y =>
                        {
                            y.UpdateConnectionLine();
                        })
                    );
                });
            }
        }

        //Закрыть окно редактирования блока
        public ICommand ClosePutActionBox
        {
            get
            {
                return new Command(() =>
                {
                    TapLastAction.IncludedAttachments = null;
                    IsPutBoxAction = false;
                    TapLastAction = null;
                });
            }
        }

        //Создание блока => воод именования
        public ICommand AddActionCommand 
        {
            get
            {
                return new Command(() =>
                {
                    IsAddAction = !IsAddAction;
                });
            }
        }

        //Создание нового блока
        public ICommand AddActionCreateCommand
        {
            get
            {
                return new Command((object sender) =>
                {
                    var nam = (sender as Entry).Text;
                    if (nam == "" || nam == null)
                        nam = LocalizationResourceManager.Instance["Message"] + $" {ActionBoxes.Count}";
                    ActionBoxes.Add(ActionBoxBuilder
                        .BuildMessageText(LocalizationResourceManager.Instance["Message"].ToString())
                        .BuildQuestion("")
                        .BuildNaming(nam)
                        .GetActionBox());
                    IsAddAction = false;
                });
            }
        }

        //Редактирование блока
        public ICommand TapOptionsActionCommand
        {
            get
            {
                return new Command((object sender) =>
                {
                    ActionBoxes.ToList()
                        .ForEach(x => x.SetStatusActionType(ActionStatusType.None));
                    IsPutBoxAction = true;
                    TapLastAction = sender as ActionBox;
                });
            }
        }

        //Привязка блоков
        public ICommand TapActionCommand
        {
            get
            {
                return new Command((object sender) =>
                {
                    var actionBox = sender as ActionBox;
                    
                    //Проверка, что пользователь не пытается привязать один и тот же блок на себя
                    if (TapLastAction == sender as ActionBox)
                    {
                        TapLastAction.SetStatusActionType(ActionStatusType.None);
                        tapLastAction = null;
                        return;
                    }
                    else if (TapLastAction == null)
                    {
                        actionBox.SetStatusActionType(ActionStatusType.Tap);
                        tapLastAction = actionBox;
                        return; 
                    }                    
                    else if (TapLastAction != null)
                    {
                        //Провека, что конекта между блоками не существует
                        ConnectionActionBox connection = null;
                        ActionBoxes.ToList()
                            .ForEach(x => x.ConnectionActions.ToList()
                            .ForEach(y =>
                            {
                                if (y.OutConnect == sender as ActionBox && y.Connect == TapLastAction
                                || y.OutConnect == TapLastAction && y.Connect == sender as ActionBox)
                                {
                                    connection = y;
                                    x.ConnectionActions.Remove(y);
                                }
                            }));

                        if (connection != null)
                        {
                            TapLastAction.SetStatusActionType(ActionStatusType.None);
                            TapLastAction = null;
                            return;
                        }

                        //Создание нового коннекта
                        ConnectionActionBox connectionActionBox = new ConnectionActionBox();
                        connectionActionBox.Connect = actionBox;
                        connectionActionBox.OutConnect = TapLastAction;
                        connectionActionBox.UpdateConnectionLine();

                        TapLastAction.ConnectionActions.Add(connectionActionBox);
                        TapLastAction.SetStatusActionType(ActionStatusType.None);
                        TapLastAction = null;
                    }
                });
            }
        }

        //Удаление блока
        public ICommand RemoveActionCommand
        {
            get
            {
                return new Command((object sender) =>
                {
                    ActionBoxes.ToList()
                        .ForEach(x => x.ConnectionActions.ToList()
                        .ForEach(y =>
                        {
                            if (y.Connect == sender as ActionBox)
                                x.ConnectionActions.Remove(y);
                        }));
                    if (TapLastAction == sender as ActionBox)
                    {
                        TapLastAction = null;
                        (sender as ActionBox).SetStatusActionType(ActionStatusType.None);
                    }
                    ActionBoxes.Remove(sender as ActionBox);
                });
            }
        }

        //Добавить медиа к сообщению
        public ICommand AddMediaCommand
        {
            get
            {
                return new Command(async () =>
                {
                    var file = await FilePicker.PickAsync(PickOptions.Images);

                    if (file == null)
                        return;

                    if (new FileInfo(file.FullPath).Length / 1024 / 1024 > 10)
                    {
                        await messageService.ShowAsync("Ограничение", "Файлы более 10МБ добавить невозможно");
                        return;
                    }
                    if (TapLastAction.MediaItems.Count > 5)
                    {
                        await messageService.ShowAsync("Ограничение", "Более 5 файлов добавить нельзя");
                        return;
                    }

                    if (file.ContentType.Split('/')[0] == "image")
                    {
                        TapLastAction.MediaItems.Add(new MediaItem()
                        {
                            Name = file.FileName,
                            Type = MediaType.Photo,
                            Source = file.FullPath,
                            PathMediaSource = file.FullPath,
                        });
                    }
                });
            }
        }

        //Удалить медиа
        public ICommand RemoveMediaCommand
        {
            get
            {
                return new Command((object sender) =>
                {
                    TapLastAction.MediaItems.Remove(sender as MediaItem);
                });
            }
        }

        //Добавить клавиатуру
        public ICommand AddKeyboardCommand
        {
            get
            {
                return new Command((object sedner) =>
                {
                       var item = sedner as KeyboardItem;
                     item.IsEnabled = !item.IsEnabled;
                });
            }
        }

        //Редактировать item клавиатуры
        public ICommand PutKeyboardCommand
        {
            get
            {
                return new Command((object sender) =>
                {
                    if (sender is KeyboardItem item)
                        KeyboardItem = item;

                    IsAddButtons = !IsAddButtons;
                    IsPutButton = !IsPutButton;
                });
            }
        }

        //Открыть редактирование конопок
        public ICommand OpenPutKeyboard
        {
            get
            {
                return new Command(() =>
                {
                    IsPutBoxAction = !IsPutBoxAction;
                    IsAddButtons = !IsAddButtons;
                });
            }
        }

        //Открыть заявку
        public ICommand OpenOrExitRequestCommand
        {
            get
            {
                return new Command(() =>
                {
                    IsPutBoxAction = !IsPutBoxAction;
                    IsAddRequest = !IsAddRequest;
                });
            }
        }

        //Сохранить заявки
        public ICommand SaveRequestCommand
        {
            get
            {
                return new Command((object nameRequest) =>
                {
                    if (nameRequest is null)
                    {
                        TapLastAction.NameSaveMessage = null;
                        return;
                    }
                    IsPutBoxAction = !IsPutBoxAction;
                    IsAddRequest = !IsAddRequest;
                    TapLastAction.NameSaveMessage = (nameRequest as Entry).Text; 
                });
            }
        }


        string pathOpenPhoto;
        public string PathOpenPhoto 
        { 
            get => pathOpenPhoto; 
            set
            {
                pathOpenPhoto = value;
                OnPropertyChanged();
            }
        }

        bool isOpenPhoto;
        public bool IsOpenPhoto
        {
            get => isOpenPhoto;
            set
            {
                isOpenPhoto = value;
                OnPropertyChanged();
            }
        }

        //Открыть фото
        public ICommand OpenPhotoCommand
        {
            get
            {
                return new Command((object image) =>
                {
                    if (image is not null)
                    {
                        var pathImage = (image as Image).Source;
                        PathOpenPhoto = pathImage.ToString().Split(' ')[1];                        
                    }
                    IsOpenPhoto = !isOpenPhoto;
                });
            }
        }


        public ConstructorViewModel(IMessageService messageService, IStorageService storageService)
        {
            ActionBoxBuilder = new ActionBoxBuilder();
            this.messageService = messageService;
            this.storageService = storageService;
        }
    }
}
