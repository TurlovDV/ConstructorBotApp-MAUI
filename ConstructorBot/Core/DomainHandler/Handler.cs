using AutoMapper;
using Core.DomainMessageModel.ElementMessage;
using Core.DomainMessagModel.ElementMessage;
using Core.MessageModel;
using Core.UserModel;
using Core.UserModel.Action;
using Model;
using Model.MessageModel;
using Model.MessageModel.ElementMessage;
using Model.UserModel;
using Model.UserModel.Action;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram;

namespace Core.DomainHandler
{
    public class Handler
    {
        private List<LogicUser> LogicUsers { get; set; }
        public List<SaveAnswer> SaveAnswers { get; set; }        
        public List<DomainParentAction> BotLogic { get; set; }
        public IMessengerApi MessengerApi { get; set; } = null!;
        public static IMapper Mapper { get
            {
                var config = new MapperConfiguration(cfg => {
                    cfg.CreateMap<ParentAction, DomainParentAction>().ReverseMap();
                    cfg.CreateMap<ChildAction, DomainChildAction>().ReverseMap();
                    cfg.CreateMap<Model.MessageModel.Message, DomainMessage>().ReverseMap();
                    cfg.CreateMap<Model.MessageModel.ElementMessage.Button, DomainButton>().ReverseMap();
                    cfg.CreateMap<Model.MessageModel.ElementMessage.KeyboardButton, DomainKeyboardButton>().ReverseMap();
                    cfg.CreateMap<Model.MessageModel.ElementMessage.MediaGroup, DomainMediaGroup>().ReverseMap();
                    cfg.CreateMap<Model.MessageModel.ElementMessage.Photo, DomainPhoto>().ReverseMap();
                    cfg.CreateMap<Model.MessageModel.ElementMessage.Audio, DomainAudio>().ReverseMap();
                    cfg.CreateMap<Model.MessageModel.ElementMessage.Document, DomainDocument>().ReverseMap();
                    cfg.CreateMap<Model.MessageModel.ElementMessage.MediaGroup, MediaGroup>().ReverseMap();
                    cfg.CreateMap<Model.MessageModel.ElementMessage.Video, DomainVideo>().ReverseMap();
                    cfg.CreateMap<Model.MessageModel.ElementMessage.ReplyKeyboardMarkup, DomainReplyKeyboardMarkup>().ReverseMap();
                });
                return config.CreateMapper();
            }
        }
        private DomainChildAction BackChild { get; set; } = null!;


        public Handler(EntityBot entityBot)
        {
            BotLogic = entityBot!.BotLogic!.LogicBot.Select(x => Mapper.Map<ParentAction, DomainParentAction>(x)).ToList();

            //New!! Уаление элементов с id
            foreach (var p in BotLogic)
                foreach (var child in p.ChildActions)
                    switch (child.MessageAnswer!.MessageType)
                    {
                        case MessageModel.MessageType.Photo:
                            if(child.MessageAnswer.Photo != null)
                            child.MessageAnswer.Photo!.Id = null;
                            break;
                        case MessageModel.MessageType.Video:
                            if (child.MessageAnswer.Video != null)
                                child.MessageAnswer.Video!.Id = null;
                            break;
                        case MessageModel.MessageType.Document:
                            if (child.MessageAnswer.Document != null)
                                child.MessageAnswer.Document!.Id = null;
                            break;
                        case MessageModel.MessageType.Audio:
                            if (child.MessageAnswer.Audio != null)
                                child.MessageAnswer.Audio!.Id = null;
                            break;
                        case MessageModel.MessageType.GroupMedia:
                            if (child.MessageAnswer.MediaGroups != null)
                                child.MessageAnswer.MediaGroups!.ForEach(x => x.Id = null);
                            break;
                    }
            //BotLogic[0].ChildActions[0].MessageAnswer.MediaGroups.Add(new MediaGroup()
            //{
            //    File = File.ReadAllBytes("C:\\New\\ConstructorBotApp\\Core\\video_icon.png"),
            //    type = Model.MessageModel.ElementMessage.MediaType.Photo
            //});

            BackChild = new DomainChildAction();
            LogicUsers = new List<LogicUser>();
            SaveAnswers = new List<SaveAnswer>();

            if (entityBot.BotType.Equals(EntityBotType.Telegram))
                MessengerApi = new TelegramApi(entityBot.ConfigurationSettings["TelegramToken"], GetAnswerMessage, MessageRequest);
        }
        public void MessageRequest(Model.MessageModel.Message message)
        {
            if(message.MessageType.Equals(Model.MessageModel.MessageType.Photo))
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
            if(message.MessageType.Equals(Model.MessageModel.MessageType.Video))
            {
                foreach (var parent in BotLogic)
                {
                    foreach (var child in parent.ChildActions)
                    {
                        if (child.MessageAnswer!.Video != null)
                            if (child.MessageAnswer!.Video!.File == message.Video!.File)
                                child.MessageAnswer.Video.Id = message.Video!.Id;
                    }
                    if (parent.WrongAnswer == BackChild.MessageAnswer)
                        parent.WrongAnswer!.Video = Mapper.Map<Model.MessageModel.ElementMessage.Video, DomainVideo>(message.Video!);
                }
            }
            if(message.MessageType.Equals(Model.MessageModel.MessageType.Audio))
            {
                foreach (var parent in BotLogic)
                {
                    foreach (var child in parent.ChildActions)
                    {
                        if (child.MessageAnswer!.Audio != null)
                            if (child.MessageAnswer!.Audio!.File == message.Audio!.File)
                                child.MessageAnswer.Audio.Id = message.Audio!.Id;
                    }
                    if (parent.WrongAnswer == BackChild.MessageAnswer)
                        parent.WrongAnswer!.Audio = Mapper.Map<Model.MessageModel.ElementMessage.Audio, DomainAudio>(message.Audio!);
                }
            }
            if(message.MessageType.Equals(Model.MessageModel.MessageType.Document))
            {
                foreach (var parent in BotLogic)
                {
                    foreach (var child in parent.ChildActions)
                    {
                        if (child.MessageAnswer!.Document != null)
                            if (child.MessageAnswer!.Document!.File == message.Document!.File)
                                child.MessageAnswer.Document.Id = message.Document!.Id;
                    }
                    if (parent.WrongAnswer == BackChild.MessageAnswer)
                        parent.WrongAnswer!.Document = Mapper.Map<Model.MessageModel.ElementMessage.Document, DomainDocument>(message.Document!);
                }
            }
            if(message.MessageType.Equals(Model.MessageModel.MessageType.GroupMedia))
            {
                foreach(var parent in BotLogic)
                {
                    foreach(var child in parent.ChildActions)
                    {
                        if (child.MessageAnswer == BackChild.MessageAnswer)
                            child.MessageAnswer!.MediaGroups = message.MediaGroups;
                    }
                    if (parent.WrongAnswer == BackChild.MessageAnswer)
                        parent.WrongAnswer!.MediaGroups = message.MediaGroups;
                }
            }
        }

        /*
        public void MailingUpdate()
        {
            var mesMailling = GetMessageMailing();
            if (mesMailling != null)
            {
                foreach (var userLogic in LogicUsers)
                {
                    var mes = mesMailling;
                    mes.ChatId = userLogic.ChatId;
                    MessengerApi.SendMessage(mes);
                }
            }
        }

        private Message GetMessageMailing()
        {
            foreach (var malling in Mailling)
            {
                if (malling.DateTime < DateTime.Now)
                {
                    MailingMessage mallingMes = malling;
                    Mailling.Remove(mallingMes);
                    var mes = mallingMes.Message;
                    return mes!;
                }
            }
            return null!;
        }
        */
        public Model.MessageModel.Message GetAnswerMessage(Model.MessageModel.Message telegramMessage)
        {
            LogicUser logicUser = GetLogicUser(telegramMessage.ChatId);
            if (logicUser == null)
            {
                logicUser = new LogicUser()
                {
                    ChatId = telegramMessage.ChatId,
                    IndexParentAction = 0
                };
                LogicUsers.Add(logicUser);
            }

            foreach (var parentAction in BotLogic)
            {
                if(parentAction.WrongAnswer != null)
                    parentAction.WrongAnswer!.ChatId = logicUser.ChatId;
                if (logicUser.IndexParentAction == parentAction.NumberMainAction)
                {
                    if(parentAction.ChildActions != null)
                    foreach (var childAction in parentAction.ChildActions)
                    {
                        childAction.MessageAnswer!.ChatId = logicUser.ChatId;
                        if (childAction.Question == telegramMessage.Text)
                        {
                            /*
                            if (BackChild.IsSaveAnswer)
                            {
                                SaveAnswer deleteSaveAnswer = new SaveAnswer();
                                foreach (var saveAnswer in SaveAnswers)
                                    if (saveAnswer.Id == BackChild.Id)
                                        deleteSaveAnswer = saveAnswer;
                                SaveAnswers.Remove(deleteSaveAnswer);
                                SaveAnswers.Add(new SaveAnswer()
                                {
                                    Answer = telegramMessage.Text!,
                                    FirstName = telegramMessage.FirstName!,
                                    UserName = telegramMessage.UserName!,
                                    Question = BackChild.MessageAnswer!.Text!,
                                    Id = BackChild.Id
                                });

                                //console.foregroundcolor = consolecolor.yellow;
                                //console.writeline("\t\tsave answers: " + messengerapi.name);
                                //console.resetcolor();
                            }
                            */
                            BackChild = childAction;
                            SetIndexParentAction(logicUser.ChatId, childAction.ForwardAction);

                            return Mapper.Map<DomainMessage, Model.MessageModel.Message>(childAction.MessageAnswer);
                        }
                    }
                    if (parentAction.ChildActions != null)
                        foreach (var childAction in parentAction.ChildActions)
                        {
                            childAction.MessageAnswer!.ChatId = logicUser.ChatId;
                            if (childAction.Question == "")
                            {
                                /*
                                if (BackChild.IsSaveAnswer)
                                {
                                    SaveAnswer deleteSaveAnswer = new SaveAnswer();
                                    foreach (var saveAnswer in SaveAnswers)
                                        if (saveAnswer.Id == BackChild.Id)
                                            deleteSaveAnswer = saveAnswer;
                                    SaveAnswers.Remove(deleteSaveAnswer);
                                    SaveAnswers.Add(new SaveAnswer()
                                    {
                                        Answer = telegramMessage.Text!,
                                        FirstName = telegramMessage.FirstName!,
                                        UserName = telegramMessage.UserName!,
                                        Question = BackChild.MessageAnswer!.Text!,
                                        Id = BackChild.Id
                                    });

                                    //console.foregroundcolor = consolecolor.yellow;
                                    //console.writeline("\t\tsave answers: " + messengerapi.name);
                                    //console.resetcolor();
                                }
                                */
                                BackChild = childAction;
                                SetIndexParentAction(logicUser.ChatId, childAction.ForwardAction);

                                return Mapper.Map<DomainMessage, Model.MessageModel.Message>(childAction.MessageAnswer);
                            }
                        }
                    //BackChild = new DomainChildAction()
                    //{
                    //    MessageAnswer = parentAction.WrongAnswer
                    //};
                    //return Mapper.Map<DomainMessage, Model.MessageModel.Message>(parentAction.WrongAnswer!);
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

