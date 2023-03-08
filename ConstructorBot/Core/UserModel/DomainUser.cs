using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UserModel
{
    public class DomainUser 
    {
        public Guid Id { get; set; }
        public List<DomainEntityBot> Bots { get; set; } = null!;
    }
}
