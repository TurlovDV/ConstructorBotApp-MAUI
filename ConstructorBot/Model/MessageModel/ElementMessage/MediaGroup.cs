using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.MessageModel.ElementMessage
{
    public class MediaGroup
    {
        public MediaType type { get; set; }
        public string? Id { get; set; }

        public byte[]? File { get; set; }
    }

    public enum MediaType 
    {
        Photo,
        Video,
        Document,
        Audio
    }
}
