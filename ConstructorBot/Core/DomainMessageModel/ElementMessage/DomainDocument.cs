using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainMessageModel.ElementMessage
{
    public class DomainDocument
    {
        public byte[]? File { get; set; }
        public string? Id { get; set; }
    }
}
