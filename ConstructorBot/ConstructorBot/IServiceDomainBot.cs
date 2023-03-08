using ConstructorBotCore.UserModel.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBot
{
    public interface IServiceDomainBot
    {
        public void Start();
        public void Stop();
    }
}
