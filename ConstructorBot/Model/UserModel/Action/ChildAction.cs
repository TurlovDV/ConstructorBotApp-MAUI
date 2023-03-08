using Model.MessageModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.UserModel.Action
{
    public class ChildAction
    {
        public Guid Id { get; set; }
        public int ForwardAction { get; set; }
        public string? Question { get; set; }
        public Message? MessageAnswer { get; set; }
        public bool? IsSaveAnswer { get; set; }

        public ChildAction()
        {
            MessageAnswer = new Message();
        }
    }
}
