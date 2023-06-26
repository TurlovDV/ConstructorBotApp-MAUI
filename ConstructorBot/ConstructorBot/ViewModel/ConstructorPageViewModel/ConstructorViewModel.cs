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
using Mopups.Services;
using ConstructorBot.View.PopupConstructorPageView;
using CommunityToolkit.Mvvm.Input;

namespace ConstructorBot.ViewModel.ConstructorPageViewModel
{
    public partial class ConstructorViewModel : ObservableObject
    {
        public ActionBoxBuilder ActionBoxBuilder;

        private IMessageService messageService;
        private IStorageService storageService;

        [ObservableProperty]
        ActionBox tapLastAction;

        public ObservableCollection<ActionBox> ActionBoxes { get; set; }

        public ConstructorViewModel(IMessageService messageService, IStorageService storageService)
        {
            ActionBoxBuilder = new ActionBoxBuilder();
            this.messageService = messageService;
            this.storageService = storageService;
        }

        [RelayCommand]
        public void GoToMainBox()
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
        }


        [RelayCommand]
        public void AddAction()
        {
            MopupService.Instance.PushAsync(new PopupCreateActionView(ActionBoxes));
        }

        [RelayCommand]
        public void OpenPopupPutActionBox(object sender)
        {
            MopupService.Instance.PushAsync(new PopupPutBoxView(sender as ActionBox));
            TapLastAction = null;
        }

        [RelayCommand]
        public void TapAction(object sender)
        {
            var actionBox = sender as ActionBox;

            //Проверка, что пользователь не пытается привязать один и тот же блок на себя
            if (TapLastAction == sender as ActionBox)
            {
                TapLastAction.SetStatusActionType(ActionStatusType.None);
                TapLastAction = null;
                return;
            }
            else if (TapLastAction == null)
            {
                actionBox.SetStatusActionType(ActionStatusType.Tap);
                TapLastAction = actionBox;
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
        }


        [RelayCommand]
        public void RemoveAction(object sender)
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
        }
    }
}
