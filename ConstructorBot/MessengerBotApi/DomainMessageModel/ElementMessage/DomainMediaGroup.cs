using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBotCore.DomainMessagModel.ElementMessage
{
    public class DomainMediaGroup
    {
        public MediaType type { get; set; }
        public string? Id { get; set; }
        public byte[]? File { get; set; }

    }

    public enum MediaType 
    {
        Photo,
        Video,
        Audio,
        Document
    }
}
