using ConstructorBotCore.DomainMessageModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConstructorBotCore.UserModel.Action
{
    public class DomainChildAction
    {
        public int ForwardAction { get; set; }
        public string? Question { get; set; }
        public DomainMessage? MessageAnswer { get; set; }
        public Guid Id { get; set; } 
    }
}
