using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBotCore.DomainHandler
{
    public class LogicUser
    {
        public long ChatId { get; set; }
        public int IndexParentAction { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Dictionary<string, string> SaveMessage { get; set; }

        public LogicUser()
        {
            SaveMessage = new ();
        }
    }
}
