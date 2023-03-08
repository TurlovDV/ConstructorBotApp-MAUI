using System;
using System.Collections.Generic;
using System.Text;

namespace Model.MessageModel.ElementMessage
{
    public class KeyboardButton
    {
        public List<List<Button>>? inline_keyboard { get; set; } 
    }

    public class Button
    {
        public string url { get; set; } = "";
        public string callback_data { get; set; } = "";
        public string text { get; set; } = "";
    }
}
