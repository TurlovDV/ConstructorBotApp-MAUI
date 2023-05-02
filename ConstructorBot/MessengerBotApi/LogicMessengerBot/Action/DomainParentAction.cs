using System;
using System.Collections.Generic;
using System.Text;

namespace ConstructorBotCore.UserModel.Action
{
    public class DomainParentAction
    {
        public Guid Id { get; set; }
        public int NumberMainAction { get; set; }
        public List<DomainChildAction> ChildActions { get; set; } = null!;

        public DomainParentAction()
        {
            Id = Guid.NewGuid();
        }
    }
}
