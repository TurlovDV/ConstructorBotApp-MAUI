using Model.MessageModel.ElementMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.MessageModel
{
    public class Message
    {
        public long ChatId { get; set; }
        public string? Text { get; set; }
        public string? FirstName { get; set; }
        public string? UserName { get; set; }
        public KeyboardButton? KeyboardButton { get; set; }
        public ReplyKeyboardMarkup? ReplyKeyboardMarkup { get; set; }
        public Photo? Photo { get; set; }
        public Video? Video { get; set; }
        public Document? Document { get; set; } 
        public Audio? Audio { get; set; }
        public List<MediaGroup>? MediaGroups { get; set; }

        public MessageType  MessageType { get; set; }

        public Message()
        {
            Photo = new Photo();
            Video = new Video();
            Audio = new Audio();
            Document = new Document();
            MediaGroups = new List<MediaGroup>();
        }
    }

    public enum MessageType
    {
        Text,
        Video,
        Photo,
        Audio,
        Document,
        GroupMedia
    }
}
