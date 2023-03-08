using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.UserModel
{
    public class User 
    {
        public Guid Id { get; set; }
        public List<EntityBot> Bots { get; set; } = null!;

        public User()
        {
            Bots = new List<EntityBot>();
            Id = Guid.NewGuid();
        }
    }
}
