using ConstructorBotCore.DomainHandler;
using ConstructorBotCore.MessengerModel.TelegramBot;
using ConstructorBotCore.UserModel.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessengerBotApi
{
    public class Domain
    {
        TelegramApi telegramApi;
        Handler handler;

        public Domain(List<DomainParentAction> parentActions, string token)
        {
            handler = new Handler(parentActions);
            telegramApi = new TelegramApi(
                token: token,
                update: handler.GetAnswerMessage,
                mesRequest: handler.MessageRequest);
        }

        public async Task<BotInfo?> Start()
        {
            return await telegramApi.Start();
        }

        public void Stop()
        {
            telegramApi.Stop();
        }

        public Logger GetMessengerInfo()
        {
            return handler.GerLogger();
        }
    }
}
