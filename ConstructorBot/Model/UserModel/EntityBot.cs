using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.UserModel
{
    public class EntityBot
    {
        public Dictionary<string, string> ConfigurationSettings { get; set; } = null!;
        public BotLogic? BotLogic { get; set; } = null!;
        public EntityBotType BotType { get; set; }
        public Guid Id { get; set; }

        public EntityBot()
        {
            BotLogic = new BotLogic();
            ConfigurationSettings = new Dictionary<string, string>();
        }
    }

    public enum EntityBotType
    {
        Telegram
    }


}
