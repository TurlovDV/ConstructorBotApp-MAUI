using ConstructorBotCore.DomainHandler;
using ConstructorBotCore.UserModel.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Requests;

namespace ConstructorBotCore
{
    public class Domain
    {
        private Handler? _handlerMessanger;
        private List<DomainParentAction>? _parentActions;
 
        public Domain UpdateBot(List<DomainParentAction> domainParents)
        {
            _parentActions = domainParents;
            return this;
        }

        public string GetNamingBot()
        {
            return _handlerMessanger!.MessengerApi.NamingBot;
        }

        public async Task<bool> OnStart()
        {
            if (_parentActions != null)
            {
                _handlerMessanger = new Handler(_parentActions!);
                return await _handlerMessanger.StartMessenger();
            }
            return false;
        }

        public async Task<bool> OnStartRestart()
        {
            return await _handlerMessanger!.StartMessenger();
        }

        public void OnStop()
        {
            _handlerMessanger!.MessengerApi.IsBot = false;
        }

        public Logger GetLogger()
        {
            return _handlerMessanger!.Logger;
        }
    }
}
