using Core.MessageModel;
using Model.MessageModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.UserModel.Action
{
    public class DomainChildAction
    {
        public int ForwardAction { get; set; }
        public string? Question { get; set; }
        public DomainMessage? MessageAnswer { get; set; }

        public bool IsSaveAnswer = false;
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
