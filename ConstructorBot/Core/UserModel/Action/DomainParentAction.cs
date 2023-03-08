using Core.MessageModel;
using Model.MessageModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.UserModel.Action
{
    public class DomainParentAction
    {
        public int NumberMainAction { get; set; }
        public List<DomainChildAction> ChildActions { get; set; } = null!;
        public DomainMessage? WrongAnswer { get; set; }

        
    }
}
