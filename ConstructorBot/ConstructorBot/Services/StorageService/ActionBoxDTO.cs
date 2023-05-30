using ConstructorBot.Model.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBot.Services.ServiceStorage
{
    public class ArrowDTO
    {
        public double _moveX { get; set; }
        public double _moveY { get; set; }
        public double TranslationX { get; set; }
        public double TranslationY { get; set; }
        public double Rotation { get; set; }
    }

    public class ConnectionActionDTO
    {
        public string Id { get; set; }
        public ArrowDTO Arrow { get; set; }
        public LineDTO Line { get; set; }
        public object Connect { get; set; }
        public string ConnectId { get; set; }
        public object OutConnect { get; set; }
    }

    public class KeyboardDTO
    {
        public List<KeyboardItemDTO> KeyboardItems { get; set; }
        public bool IsInline { get; set; }
    }

    public class KeyboardItemDTO
    {
        public string Text { get; set; }
        public double Column { get; set; }
        public double Row { get; set; }
        public string Url { get; set; }
        public bool IsEnabled { get; set; }
    }

    public class LineDTO
    {
        public double _moveX { get; set; }
        public double _moveY { get; set; }
        public double HeightRequest { get; set; }
        public double TranslationX { get; set; }
        public double TranslationY { get; set; }
        public double Rotation { get; set; }
    }

    public class MediaItemDTO
    {
        public int[] Bytes { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public MediaType Type { get; set; }
        public string PathMediaSource { get; set; }
        public string Source { get; set; }
    }

    public class ActionBoxDTO
    {
        public double _moveX { get; set; }
        public double _moveY { get; set; }
        public bool IsMainAction { get; set; }
        public string NameSaveMessage { get; set; }
        public Color ColorNone { get; set; }//string
        public string Id { get; set; }
        public double TranslationX { get; set; }
        public double TranslationY { get; set; }
        public KeyboardDTO Keyboard { get; set; }
        public string Question { get; set; }
        public string Naming { get; set; }
        public string MessageText { get; set; }
        public Color Color { get; set; }//string
        public List<ConnectionActionDTO> ConnectionActions { get; set; }
        public List<MediaItem> MediaItems { get; set; }
        public double Rotation { get; set; }
    }
}

