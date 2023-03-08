using Core.MessageModel;
using Model.MessageModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.UserModel.Action
{
    public class DomainMailingMessage
    {
        public DomainMessage? Message { get; set; }

        public DateTime DateTime { get; set; }
    }
}
