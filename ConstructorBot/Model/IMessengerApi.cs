using Model.MessageModel;
using Model.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface IMessengerApi
    {
        public delegate Message MessageUpdate(Message message);
        public bool IsBot { get; set; }
        public string Name { get; set; }
    }
}
