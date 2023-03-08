using ConstructorBotCore.DomainMessageModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBotCore.MessengerModel
{
    public interface IMessenger
    {

         public delegate DomainMessage MessageUpdate(DomainMessage message);

         public bool IsBot { get; set; }
    }
}
