using ConstructorBot.Model.ChatModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBot.Model
{
    public class LogicUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<ChatMessageModel> ChatMessage { get; set; }

        public ObservableCollection<SaveMessageItem> SaveAnswers { get; set; }

        public LogicUser()
        {
            ChatMessage = new();
            SaveAnswers = new();
        }
    }
}
