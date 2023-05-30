using ConstructorBot.Services.PopupService;
using ConstructorBotMessengerApi;
using ConstructorBotMessengerApi.HandlerLogicModel.ActionModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBot.Services
{
    public class BuildMessengerService : IBuildMessengerService
    {        
        public IControlMessengerService Initialization(List<ParentAction> parentActions, string connectionToken) =>
            new ControlMessengerServices(parentActions, connectionToken);
    }
}
