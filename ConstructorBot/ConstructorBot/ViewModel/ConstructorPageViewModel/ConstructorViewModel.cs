using ConstructorBot.ViewModel.ConstructorPageViewModel.Action;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using ConstructorBot.ViewModel.ConstructorPageViewModel.Action.ConnectionElement;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using static ConstructorBot.App;

namespace ConstructorBot.ViewModel.ConstructorPageViewModel
{
    public partial class ConstructorViewModel : INotifyPropertyChanged
    {
        private bool isPutBoxAction = false;
        private bool isAddButtons = false;
        private bool isPutButton = false;
        private bool isAddAction = false;
        public IMessageService messageService { get; set; }

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

        private ActionBox tapLastAction;

        //public MatrixGrid MatrixGridLines { get; set; } 
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

        
        public event PropertyChangedEventHandler PropertyChanged;
        
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
       
        //Закрыть окно редактирования блока
        public ICommand ClosePutActionBox
        {
            get
            {
                return new Command(() =>
                {
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
                        nam = $"Сообщение {ActionBoxes.Count}";
                    ActionBoxes.Add(ActionBoxBuilder
                        .BuildMessageText("Сообщение")
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
                        TapLastAction = null;
                        return;
                    }
                    if (TapLastAction == null)
                    {
                        actionBox.SetStatusActionType(ActionStatusType.Tap);
                        TapLastAction = actionBox;
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

        //Кнопка назат в главное меню
        public ICommand PushMainPageCommand
        {
            get
            {
                return new Command((object sender) =>
                {
                    
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
                    var file = await FilePicker.PickAsync();

                    if (file == null)
                        return;

                    var stream = await file.OpenReadAsync();

                    if (file.ContentType.Split('/')[0] == "image")
                    {
                        if (TapLastAction.MediaItems
                        .Where(x => x.Type == MediaType.Document || x.Type == MediaType.Audio).ToList().Count != 0)
                        {
                            await messageService.ShowAsync("Ограничение", "Чередовать фото с документами и аудио невозможно");
                            return;
                        }
                        TapLastAction.MediaItems.Add(new MediaItem()
                        {
                            //Bytes = stream.ReadFully(),
                            Name = file.FileName,
                            Type = MediaType.Photo,
                            Source = file.FullPath,
                            PathMediaSource = file.FullPath,
                        });
                    }
                    else if (file.ContentType.Split('/')[0] == "video")
                    {
                        if (TapLastAction.MediaItems
                        .Where(x => x.Type == MediaType.Document || x.Type == MediaType.Audio).ToList().Count != 0)
                        {
                            await messageService.ShowAsync("Ограничение", "Чередовать видео с документами и аудио невозможно");
                            return;
                        }
                        TapLastAction.MediaItems.Add(new MediaItem()
                        {
                            //Bytes = stream.ReadFully(),
                            Name = file.FileName,
                            Type = MediaType.Video,
                            Source = "video_icon.png",
                            PathMediaSource = file.FullPath,
                        });
                    }
                    else if (file.ContentType.Split('/')[0] == "application")
                    {
                        if (TapLastAction.MediaItems
                        .Where(x => x.Type == MediaType.Photo ||
                            x.Type == MediaType.Video ||
                            x.Type == MediaType.Audio).ToList().Count != 0)
                        {
                            await messageService.ShowAsync("Ограничение", "Чередовать документы с другими элементами невозможно");
                            return;
                        }
                        TapLastAction.MediaItems.Add(new MediaItem()
                        {
                            //Bytes = stream.ReadFully(),
                            Name = file.FileName,
                            Type = MediaType.Document,
                            Source = "document_icon.png",
                            PathMediaSource = file.FullPath,
                        });
                    }
                    else if (file.ContentType.Split('/')[0] == "audio")
                    {
                        if (TapLastAction.MediaItems
                        .Where(x => x.Type == MediaType.Document ||
                            x.Type == MediaType.Photo ||
                            x.Type == MediaType.Video).ToList().Count != 0)
                        {
                            await messageService.ShowAsync("Ограничение", "Чередовать аудио с другими элементами невозможно");
                            return;
                        }
                        TapLastAction.MediaItems.Add(new MediaItem()
                        {
                            //Bytes = stream.ReadFully(),
                            Name = file.FileName,
                            Type = MediaType.Audio,
                            Source = "music_icon.png",
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

        private KeyboardItem keyboardItem { get; set; }

        public KeyboardItem KeyboardItem
        {
            get => keyboardItem;
            set
            {
                keyboardItem = value;
                OnPropertyChanged();
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

        public ConstructorViewModel()
        {
            ActionBoxBuilder = new ActionBoxBuilder();

            this.messageService = DependencyService.Get<App.IMessageService>();

            //MatrixGridLines = new MatrixGrid(Colors.Black, 1);
        }
    }
}
