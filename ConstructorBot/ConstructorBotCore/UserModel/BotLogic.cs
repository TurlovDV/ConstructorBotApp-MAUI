using ConstructorBotCore.UserModel.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBotCore.UserModel
{
    public class BotLogic
    {
        public List<DomainParentAction> LogicBot { get; set; } = null!;        
        public BotLogic(List<DomainParentAction> parentActions)
        {
            LogicBot = parentActions;
        }
    }
}
