using ConstructorBotCore.DomainMessageModel;
using ConstructorBotCore.MessengerModel;
using ConstructorBotCore.MessengerModel.TelegramBot;
using ConstructorBotCore.UserModel.Action;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBotCore.DomainHandler
{
    public class Handler
    {
        private List<LogicUser> LogicUsers { get; set; } 
        public List<DomainParentAction> BotLogic { get; set; }
        public TelegramApi MessengerApi { get; set; } 
        private DomainChildAction BackChild { get; set; } = null!;

        public Logger Logger { get; set; }
        
        public Handler(List<DomainParentAction> domainParents)
        {
            Logger = new Logger();
            BotLogic = domainParents;
            BackChild = new DomainChildAction();
            LogicUsers = new List<LogicUser>();

            MessengerApi = 
                new TelegramApi("5639252625:AAEc8NZlurYXxejvHW9MXsK6yvEp8tZ1nSM",
                GetAnswerMessage,
                MessageRequest);
        }
        public void MessageRequest(DomainMessage message)
        {
            if(message.MessageType.Equals(MessageType.Photo))
            {
                foreach (var parent in BotLogic)
                {
                    foreach(var child in parent.ChildActions)
                    {
                        if(child.MessageAnswer!.Photo != null)
                        if (child.MessageAnswer!.Photo!.File == message.Photo!.File)
                            child.MessageAnswer.Photo.Id = message.Photo!.Id;
                    }
                }
            }
            if(message.MessageType.Equals(MessageType.Video))
            {
                foreach (var parent in BotLogic)
                {
                    foreach (var child in parent.ChildActions)
                    {
                        if (child.MessageAnswer!.Video != null)
                            if (child.MessageAnswer!.Video!.File == message.Video!.File)
                                child.MessageAnswer.Video.Id = message.Video!.Id;
                    }
                }
            }
            if(message.MessageType.Equals(MessageType.Audio))
            {
                foreach (var parent in BotLogic)
                {
                    foreach (var child in parent.ChildActions)
                    {
                        if (child.MessageAnswer!.Audio != null)
                            if (child.MessageAnswer!.Audio!.File == message.Audio!.File)
                                child.MessageAnswer.Audio.Id = message.Audio!.Id;
                    }
                }
            }
            if(message.MessageType.Equals(MessageType.Document))
            {
                foreach (var parent in BotLogic)
                {
                    foreach (var child in parent.ChildActions)
                    {
                        if (child.MessageAnswer!.Document != null)
                            if (child.MessageAnswer!.Document!.File == message.Document!.File)
                                child.MessageAnswer.Document.Id = message.Document!.Id;
                    }
                }
            }
            if(message.MessageType.Equals(MessageType.GroupMedia))
            {
                foreach(var parent in BotLogic)
                {
                    foreach(var child in parent.ChildActions)
                    {
                        if (child.MessageAnswer == BackChild.MessageAnswer)
                            child.MessageAnswer!.MediaGroups = message.MediaGroups;
                    }
                }
            }
        }

        public DomainMessage GetAnswerMessage(DomainMessage telegramMessage)
        {
            Logger.CountMessage++;
            LogicUser logicUser = GetLogicUser(telegramMessage.ChatId);
            if (logicUser == null)
            {
                Logger.CountProfile++;
                logicUser = new LogicUser()
                {
                    ChatId = telegramMessage.ChatId,
                    IndexParentAction = 0
                };
                LogicUsers.Add(logicUser);
            }

            foreach (var parentAction in BotLogic)
            {
                if (logicUser.IndexParentAction == parentAction.NumberMainAction)
                {
                    if(parentAction.ChildActions != null)
                    foreach (var childAction in parentAction.ChildActions)
                    {
                        childAction.MessageAnswer!.ChatId = logicUser.ChatId;
                        if (childAction.Question == telegramMessage.Text)
                        {
                            BackChild = childAction;
                            SetIndexParentAction(logicUser.ChatId, childAction.ForwardAction);

                            return childAction.MessageAnswer;
                        }
                    }
                    if (parentAction.ChildActions != null)
                        foreach (var childAction in parentAction.ChildActions)
                        {
                            childAction.MessageAnswer!.ChatId = logicUser.ChatId;
                            if (childAction.Question == "")
                            {
                                BackChild = childAction;
                                SetIndexParentAction(logicUser.ChatId, childAction.ForwardAction);

                                return childAction.MessageAnswer;
                            }
                        }
                }
            }

            return null!;
        }
        
        private void SetIndexParentAction(long _chatId, int _indexParentAction)
        {
            foreach (var user in LogicUsers)
                if (user.ChatId == _chatId)
                    user.IndexParentAction = _indexParentAction;
        }
        
        private LogicUser GetLogicUser(long _chatId)
        {
            foreach (var user in LogicUsers)
                if (user.ChatId == _chatId)
                    return user;
            return null!;
        }

    }
}

