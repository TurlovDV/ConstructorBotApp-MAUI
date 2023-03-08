using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainHandler
{
    public class SaveAnswer
    {
        public string Answer { get; set; } = null!;
        public string Question { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public Guid Id { get; set; }
    }
}
