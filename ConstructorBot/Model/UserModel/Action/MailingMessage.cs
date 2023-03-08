using Model.MessageModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.UserModel.Action
{
    public class MailingMessage
    {
        public Message? Message { get; set; }

        public DateTime DateTime { get; set; }
    }
}
