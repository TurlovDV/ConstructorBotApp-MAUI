using ConstructorBotMessengerApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBot.Services
{
    public interface IControlMessengerService
    {
        Task<MessengerBotInfo> Start();

        void Stop();

        public Logger GetLogger();

        Task<MessengerBotInfo> Restart();
    }
}
