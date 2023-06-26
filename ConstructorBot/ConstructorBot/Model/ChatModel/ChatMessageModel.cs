using ConstructorBot.Model.Action;
using ConstructorBot.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Keyboard = ConstructorBot.Model.Action.Keyboard;

namespace ConstructorBot.Model.ChatModel
{
    public class ChatMessageModel
    {
        public string Message { get; set; }

        public DateTime DateTime { get; set; }

        public Keyboard Keyboard { get; set; }

        public bool IsBot { get; set; }

        public ObservableCollection<MediaItem> MediaItems { get; set; }
    }
}
