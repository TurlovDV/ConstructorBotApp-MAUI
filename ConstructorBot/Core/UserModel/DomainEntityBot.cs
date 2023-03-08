using Core.DomainHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UserModel
{
    public class DomainEntityBot
    {
        public Handler Handler { get; set; } = null!;
        public Logger Logger { get; set; } = null!;
        public Guid Id { get; set; }
    }
}
