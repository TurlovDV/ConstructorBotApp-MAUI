using ConstructorBot.ViewModel.ConstructorPageViewModel.Action.ConnectionElement;
using Microsoft.Maui.Graphics.Text;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBot.ViewModel.ConstructorPageViewModel.Action
{
    public class ActionBox : MatrixElement, ICloneable
    {
        private string question;
        private string naming;
        private string messageText;
        private Color color;
        private ObservableCollection<ConnectionActionBox> connectionActions;
        private bool isMainAction = false;
        private ObservableCollection<MediaItem> mediaItems;
        private Keyboard keyboard;

        public bool IsMainAction
        {
            get => isMainAction;
            set
                { isMainAction = value; OnPropertyChanged(); }
        }

        public Color ColorNone { get; set; } = Colors.White;

        public Guid Id { get; set; }        

        public override double TranslationX
        {
            get => base.translationX;
            set
            {
                base.translationX = value;
                
                ConnectionActions.ToList()
                    .ForEach(x => x.UpdateConnectionLine());
                
                base.OnPropertyChanged();
            }
        }

        public override double TranslationY
        {
            get => base.translationY;
            set
            {
                base.translationY = value;

                ConnectionActions.ToList()
                    .ForEach(x => x.UpdateConnectionLine());
                
                base.OnPropertyChanged();
            }
        }

        public void BaseTranslation(double translationX, double translationY)
        {
            base.TranslationY = translationY;
            base.TranslationX = translationX;
        }      

        public ActionBox()
        {
            Id = Guid.NewGuid();
            ConnectionActions = new ObservableCollection<ConnectionActionBox>();
            Color = Colors.White;
            MediaItems = new ObservableCollection<MediaItem>();
            Keyboard = new Keyboard();
        }

        public Keyboard Keyboard
        {
            get => keyboard;
            set
            {
                keyboard = value;
                OnPropertyChanged();
            }
        }

        public void SetStatusActionType(ActionStatusType statusType)
        {
            switch (statusType)
            {
                case ActionStatusType.None: 
                    Color = ColorNone;
                    break;
                case ActionStatusType.Tap:
                    Color = Color.FromArgb("CBD6FF");
                    break;
                case ActionStatusType.Move:
                    Color = Colors.Yellow;
                    break;
            }
        }

        public object Clone()
        {

            //List<object> connectionActionBoxes = new List<object>();
            //foreach (var connectionAction in ConnectionActions)
            //{
            //    connectionActionBoxes.Add(connectionAction.Clone());
            //}
            return new
            {
                question = this.Question,
                naming = this.Naming,
                messageText = this.MessageText,
                color = this.Color,
                //connectionActions = connectionActionBoxes,
                isMainAction = this.IsMainAction,
                //mediaItems = this.MediaItems,
                //keyboard = this.Keyboard,
                Id = this.Id,
                translationX = this.TranslationX,
                translationY = this.TranslationY,
                Rotation = this.Rotation,
                ColorNone = this.ColorNone
            };
        }

        public string Question 
        {
            get => question;
            set
            {
                question = value;
                OnPropertyChanged();
            }
        }

        public string Naming
        {
            get => naming;
            set
            {
                naming = value;
                OnPropertyChanged();
            }
        }

        public string MessageText
        {
            get => messageText;
            set
            {
                messageText = value;
                OnPropertyChanged();
            }
        }

        public Color Color
        {
            get => color;
            set
            {
                color = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ConnectionActionBox> ConnectionActions
        {
            get => connectionActions;
            set
            {
                connectionActions = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<MediaItem> MediaItems
        {
            get => mediaItems;
            set
            {
                mediaItems = value;
                OnPropertyChanged();
            }
        }
    }

    public enum ActionStatusType
    {
        None,
        Tap,
        Move
    }
}
