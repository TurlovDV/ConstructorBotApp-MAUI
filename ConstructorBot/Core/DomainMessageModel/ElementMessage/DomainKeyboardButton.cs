using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DomainMessagModel.ElementMessage
{
    public class DomainKeyboardButton
    {
        public List<List<DomainButton>>? inline_keyboard { get; set; } 
    }

    public class DomainButton
    {
        public string url { get; set; } = "";
        public string callback_data { get; set; } = "";
        public string text { get; set; } = "";
    }
}
