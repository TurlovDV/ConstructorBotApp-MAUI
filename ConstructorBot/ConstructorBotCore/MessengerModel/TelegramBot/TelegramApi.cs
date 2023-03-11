using ConstructorBotCore.DomainMessageModel;
using ConstructorBotCore.DomainMessageModel.ElementMessage;
using ConstructorBotCore.DomainMessagModel.ElementMessage;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace ConstructorBotCore.MessengerModel.TelegramBot
{
    public class TelegramApi : IMessenger
    {
        public string Token { get; set; }

        public delegate DomainMessage MessageUpdate(DomainMessage message);

        public event MessageUpdate NewMessage = null!;

        public delegate void MessageAnswer(DomainMessage messageSend);

        public event MessageAnswer MessageRequest = null!;

        static ITelegramBotClient bot = null!;
        public CancellationToken CancellationToken { get; set; }
        public string NamingBot { get; set; } = "";

        public bool _isBot = true;
        public bool IsBot 
        {
            get => _isBot;
            set
            {
                if(value)
                {
                   Start();
                }
                _isBot = value;
            }
        }

        public TelegramApi(string token, MessageUpdate update, MessageAnswer mesRequest)
        {
            Token = token;
            NewMessage += update;
            MessageRequest += mesRequest;           

            bot = new TelegramBotClient(token);

            //Start();
        }

        public async Task<bool> Start()
        {
            try
            {
                var name = await bot.GetMeAsync();
                NamingBot = name.FirstName + " " + name.LastName;
                //Console.WriteLine("\tBot start: " + Name, ConsoleColor.Red);

                var cts = new CancellationTokenSource();
                CancellationToken = cts.Token;
                var receiverOptions = new ReceiverOptions
                {
                    Limit = 1,
                    ThrowPendingUpdates = true,
                };

                //await bot.CloseAsync();

                bot.StartReceiving(
                    HandleUpdateAsync,
                    HandleErrorAsync,
                    receiverOptions,
                    CancellationToken);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (!_isBot)
            {
                await bot.CloseAsync();
            }
            else
            {
                //Получение ответного сообщения от логики бота
                DomainMessage message = null!;
                if (update.CallbackQuery != null)
                    message = new DomainMessage()
                    {
                        ChatId = update.CallbackQuery.From.Id,
                        Text = update.CallbackQuery.Data,
                        FirstName = update.CallbackQuery.From.FirstName + update.CallbackQuery.From.LastName,
                        UserName = update.CallbackQuery.From.Username
                    };
                else if (update.Message!.Type.Equals(Telegram.Bot.Types.Enums.MessageType.Text))
                    message = new DomainMessage()
                    {
                        ChatId = update.Message.Chat.Id,
                        Text = update.Message.Text,
                        FirstName = update.Message!.From!.FirstName + update.Message.From.LastName,
                        UserName = update.Message.From.Username
                    };
                else
                    return;
                var messageAnswer = NewMessage(message);

                if (messageAnswer == null)
                    return;
                //Отправка сообщения в api telegram
                if (messageAnswer.MessageType.Equals(DomainMessageModel.MessageType.Photo))
                {
                    var mes = await botClient.SendPhotoAsync(
                        messageAnswer.ChatId,
                        messageAnswer.Photo!.Id! != null ? messageAnswer.Photo!.Id!
                        //: new InputOnlineFile(new FileStream(messageAnswer.Photo!.Path!, FileMode.Open, FileAccess.Read, FileShare.Read)) ,
                        : new InputOnlineFile(new MemoryStream(messageAnswer.Photo.File!)),
                        caption: messageAnswer.Text,
                        replyMarkup: GetReplyMarkup(messageAnswer));
                    if (messageAnswer.Photo!.Id! == null)
                        MessageRequest(
                        new DomainMessage()
                        {
                            MessageType = DomainMessageModel.MessageType.Photo,
                            Photo = new DomainPhoto()
                            {
                                File = messageAnswer.Photo.File,
                                Id = mes.Photo!.ToList().Last().FileId
                            }
                        });
                }
                else if (messageAnswer.MessageType.Equals(DomainMessageModel.MessageType.Video))
                {
                    var mes = await botClient.SendVideoAsync(
                        messageAnswer.ChatId,
                        messageAnswer.Video!.Id! != null ? messageAnswer.Video!.Id!
                        : new InputOnlineFile(new MemoryStream(messageAnswer.Video!.File!)),
                        caption: messageAnswer.Text,
                        replyMarkup: GetReplyMarkup(messageAnswer));
                    if (messageAnswer.Video!.Id! == null)
                        MessageRequest(
                            new DomainMessage()
                            {
                                MessageType = DomainMessageModel.MessageType.Video,
                                Video = new DomainMessageModel.ElementMessage.DomainVideo()
                                {
                                    File = messageAnswer.Video.File,
                                    Id = mes.Video!.FileId
                                }
                            });
                }
                else if (messageAnswer.MessageType.Equals(DomainMessageModel.MessageType.Audio))
                {
                    var mes = await botClient.SendAudioAsync(
                        messageAnswer.ChatId,
                        messageAnswer.Audio!.Id! != null ? messageAnswer.Audio!.Id!
                        : new InputOnlineFile(new MemoryStream(messageAnswer.Audio!.File!)),
                        caption: messageAnswer.Text,
                        replyMarkup: GetReplyMarkup(messageAnswer));
                    if (messageAnswer.Audio!.Id! == null)
                        MessageRequest(
                            new DomainMessage()
                            {
                                MessageType = DomainMessageModel.MessageType.Audio,
                                Audio = new DomainAudio()
                                {
                                    File = messageAnswer.Audio.File,
                                    Id = mes.Audio!.FileId
                                }
                            });
                }
                else if (messageAnswer.MessageType.Equals(DomainMessageModel.MessageType.Document))
                {
                    var mes = await botClient.SendDocumentAsync(
                        messageAnswer.ChatId,
                        messageAnswer.Document!.Id! != null ? messageAnswer.Document!.Id!
                        : new InputOnlineFile(new MemoryStream(messageAnswer.Document!.File!)),
                        caption: messageAnswer.Text,
                        replyMarkup: GetReplyMarkup(messageAnswer));
                    if (messageAnswer.Document!.Id! == null)
                        MessageRequest(
                            new DomainMessage()
                            {
                                MessageType = DomainMessageModel.MessageType.Document,
                                Document = new DomainDocument()
                                {
                                    File = messageAnswer.Document.File,
                                    Id = mes.Document!.FileId
                                }
                            });
                }
                else if (messageAnswer.MessageType.Equals(DomainMessageModel.MessageType.GroupMedia))
                {
                    int count = 0;
                    var albumInputMediasPhoto =
                        messageAnswer.MediaGroups!
                        .Select(x => x.type.Equals(MediaType.Photo)
                        ? ((x.Id == null)
                                    ? new InputMediaPhoto(new InputMedia(new MemoryStream(x.File!), "element" + count++))
                                    : new InputMediaPhoto(x.Id))
                        : default).ToList();

                    var albumInputMediasVideo =
        messageAnswer.MediaGroups!
        .Select(x => x.type.Equals(MediaType.Video)
        ? ((x.Id == null)
                    ? new InputMediaVideo(new InputMedia(new MemoryStream(x.File!), "element" + count++))
                    : new InputMediaVideo(x.Id))
        : default).ToList();
                    var albumInputMediasAudio =
        messageAnswer.MediaGroups!
        .Select(x => x.type.Equals(MediaType.Audio)
        ? ((x.Id == null)
                    ? new InputMediaAudio(new InputMedia(new MemoryStream(x.File!), "element" + count++))
                    : new InputMediaAudio(x.Id))
        : default).ToList();
                    var albumInputMediasDocument =
        messageAnswer.MediaGroups!
        .Select(x => x.type.Equals(MediaType.Document)
        ? ((x.Id == null)
                    ? new InputMediaDocument(new InputMedia(new MemoryStream(x.File!), "element" + count++))
                    : new InputMediaDocument(x.Id))
        : null).ToList();


                    List<IAlbumInputMedia> albumInputMedias = new List<IAlbumInputMedia>();
                    albumInputMediasAudio.RemoveAll(item => item == null);
                    albumInputMediasVideo.RemoveAll(item => item == null);
                    albumInputMediasDocument.RemoveAll(item => item == null);
                    albumInputMediasPhoto.RemoveAll(item => item == null);

                    if (albumInputMediasPhoto.Count != 0)
                        albumInputMediasPhoto[0]!.Caption = messageAnswer.Text;
                    if (albumInputMediasVideo.Count != 0)
                        albumInputMediasVideo[0]!.Caption = messageAnswer.Text;
                    if (albumInputMediasAudio.Count != 0)
                        albumInputMediasAudio[0]!.Caption = messageAnswer.Text;
                    if (albumInputMediasDocument.Count != 0)
                        albumInputMediasDocument[0]!.Caption = messageAnswer.Text;

                    if (albumInputMediasPhoto.Count > 0 || albumInputMediasVideo.Count > 0)
                    {
                        albumInputMediasPhoto.ForEach(x => albumInputMedias.Add(x!));
                        albumInputMediasVideo.ForEach(x => albumInputMedias.Add(x!));
                    }
                    else if (albumInputMediasDocument.Count > 0)
                    {
                        albumInputMediasDocument.ForEach(x => albumInputMedias.Add(x!));
                    }
                    else if (albumInputMediasAudio.Count > 0)
                    {
                        albumInputMediasAudio.ForEach(x => albumInputMedias.Add(x!));
                    }

                    //var firstElement = albumInputMedias[0];
                    //albumInputMedias.Remove(firstElement);
                    //firstElement.Caption =

                    var mes = await botClient.SendMediaGroupAsync(
                        messageAnswer.ChatId,
                        media: albumInputMedias!);

                    List<DomainMediaGroup> mediaGroups = new List<DomainMediaGroup>();
                    foreach (var item in mes)
                    {
                        DomainMediaGroup mediaGroup1 = new DomainMediaGroup();
                        if (item.Type == Telegram.Bot.Types.Enums.MessageType.Photo)
                        {
                            mediaGroup1.Id = item.Photo!.ToList().Last().FileId;
                            mediaGroup1.type = MediaType.Photo;
                        }
                        if (item.Type == Telegram.Bot.Types.Enums.MessageType.Video)
                        {
                            mediaGroup1.Id = item.Video!.FileId;
                            mediaGroup1.type = MediaType.Video;
                        }
                        if (item.Type == Telegram.Bot.Types.Enums.MessageType.Document)
                        {
                            mediaGroup1.Id = item.Document!.FileId;
                            mediaGroup1.type = MediaType.Document;
                        }
                        if (item.Type == Telegram.Bot.Types.Enums.MessageType.Audio)
                        {
                            mediaGroup1.Id = item.Audio!.FileId;
                            mediaGroup1.type = MediaType.Audio;
                        }
                        mediaGroups.Add(mediaGroup1);
                    }
                    MessageRequest(
                        new DomainMessage()
                        {
                            MessageType =  DomainMessageModel.MessageType.GroupMedia,
                            MediaGroups = mediaGroups
                        });

                }
                else if (messageAnswer.MessageType.Equals(DomainMessageModel.MessageType.Text))
                    await botClient.SendTextMessageAsync(messageAnswer.ChatId, messageAnswer.Text!, replyMarkup: GetReplyMarkup(messageAnswer));
            }
        }

        //Маппинг клавиатуры к телеграмму
        private IReplyMarkup GetReplyMarkup(DomainMessage message)
        {
            if (message.KeyboardButton != null)
            {
                var list = new List<List<InlineKeyboardButton>>();

                foreach (var item in message.KeyboardButton.inline_keyboard!)
                {
                    List<InlineKeyboardButton> inlineKeyboardButtons = new List<InlineKeyboardButton>();
                    foreach (var i in item)
                        inlineKeyboardButtons.Add(new InlineKeyboardButton(i.text) { CallbackData = i.callback_data, Url = i.url });
                    list.Add(inlineKeyboardButtons);
                }
                return new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup(list);
            }

            if (message.ReplyKeyboardMarkup != null)
            {
                var list = new List<List<Telegram.Bot.Types.ReplyMarkups.KeyboardButton>>();

                //foreach (var item in message.KeyboardButton!.inline_keyboard!)
                foreach (var item in message.ReplyKeyboardMarkup!.Keyboard!)
                {
                    List<Telegram.Bot.Types.ReplyMarkups.KeyboardButton> inlineKeyboardButtons = new List<Telegram.Bot.Types.ReplyMarkups.KeyboardButton>();
                    foreach (var i in item)
                        inlineKeyboardButtons.Add(new Telegram.Bot.Types.ReplyMarkups.KeyboardButton(i));
                    list.Add(inlineKeyboardButtons);
                }
                return new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup(list);
            }

            return null!;
        }

        public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            await botClient.CloseAsync();            
        }
    }
}
