using ConstructorBotCore.DomainMessageModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConstructorBotCore.UserModel.Action
{
    public class DomainMailingMessage
    {
        public DomainMessage? Message { get; set; }

        public DateTime DateTime { get; set; }
    }
}
