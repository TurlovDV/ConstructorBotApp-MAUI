using Core.UserModel.Action;
using Model.UserModel.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UserModel
{
    public class BotLogic
    {
        public List<DomainParentAction> LogicBot { get; set; } = null!;
        public List<MailingMessage> Mailling { get; set; }

        public BotLogic() =>
            Mailling = new List<MailingMessage>();

        public BotLogic(List<DomainParentAction> parentActions)
        {
            LogicBot = parentActions;
            Mailling = new List<MailingMessage>();
        }

    }
}
