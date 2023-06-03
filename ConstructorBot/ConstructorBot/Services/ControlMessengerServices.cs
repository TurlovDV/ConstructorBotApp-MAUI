using ConstructorBot.Services.PopupService;
using ConstructorBotMessengerApi;
using ConstructorBotMessengerApi.HandlerLogicModel.ActionModel;
using ConstructorBotMessengerApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBot.Services
{
    public class ControlMessengerServices : IControlMessengerService
    {
        ConstructorBotMessengerApi.IMessengerService messengerService;
        IBackgroundWorkService backgroundWork;
        readonly List<ParentAction> parentActions;
        readonly string connectionToken;

        public ControlMessengerServices(List<ParentAction> parentActions, string connectionToken)
        {
            messengerService = ServiceProvider.GetService<ConstructorBotMessengerApi.IMessengerService>();
            backgroundWork = ServiceProvider.GetService<IBackgroundWorkService>();
            this.parentActions = parentActions;
            this.connectionToken = connectionToken;
        }

        public async Task<MessengerBotInfo> Start(bool isBackgroundWork = true)
        {
            var result = await messengerService.Start(parentActions, connectionToken);
            
            if (result != null && isBackgroundWork)
                backgroundWork.Start();

            return result;
        }

        public void Stop()
        {
            messengerService.Stop();
            backgroundWork.Stop();
        }

        public async Task<MessengerBotInfo> Restart()
        {
            return await messengerService.Restart();
        }

        public Logger GetLogger() =>
            messengerService.GetLogger();
    }
}
