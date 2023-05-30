using ConstructorBotMessengerApi.HandlerLogicModel.ActionModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBot.Services
{
    public interface IBuildMessengerService
    {
        IControlMessengerService Initialization(List<ParentAction> parentActions, string Token);
    }
}
