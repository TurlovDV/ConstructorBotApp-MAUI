using ConstructorBotCore.DomainMessageModel.ElementMessage;
using ConstructorBotCore.DomainMessagModel.ElementMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBotCore.DomainMessageModel
{
    public class DomainMessage
    {
        public long ChatId { get; set; }
        public string? Text { get; set; }
        public string? FirstName { get; set; }
        public string? UserName { get; set; }
        public DomainKeyboardButton? KeyboardButton { get; set; }
        public DomainReplyKeyboardMarkup? ReplyKeyboardMarkup { get; set; }
        public DomainPhoto? Photo { get; set; }
        public DomainAudio? Audio { get; set; }
        public DomainDocument? Document { get; set; }
        public DomainVideo? Video { get; set; }
        public List<DomainMediaGroup>? MediaGroups { get; set; }
        public MessageType  MessageType { get; set; }

        public DomainMessage() =>
            MediaGroups = new List<DomainMediaGroup>();
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
