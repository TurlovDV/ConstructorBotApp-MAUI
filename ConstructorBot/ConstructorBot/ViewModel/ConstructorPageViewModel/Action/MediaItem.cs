using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBot.ViewModel.ConstructorPageViewModel.Action
{
    public class MediaItem
    {
        public byte[] Bytes { get; set; }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public MediaType Type { get; set; }

        public string PathMediaSource { get; set; }

        public string Source { get; set; }

        public MediaItem()
        {
            Id = Guid.NewGuid();
        }
    }

    public enum MediaType
    {
        Photo,
        Video,
        Audio,
        Document
    }

    public static class StreamExtensions
    {
        public static byte[] ReadFully(this Stream input)
        {
            byte[] k;
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                k = ms.ToArray();
            }
            return k;
        }
    }
}
