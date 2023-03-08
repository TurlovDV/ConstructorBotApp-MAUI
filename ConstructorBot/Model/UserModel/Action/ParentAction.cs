using Model.MessageModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.UserModel.Action
{
    public class ParentAction
    {
        public Guid Id { get; set; }
        public int NumberMainAction { get; set; }
        public List<ChildAction> ChildActions { get; set; } = null!;
        public Message? WrongAnswer { get; set; }
    }
}
