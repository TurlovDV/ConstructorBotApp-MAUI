using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBot.Services.PopupService
{
    public interface IMessageService
    {
        Task ShowAsync(string title, string message);
        Task<string> ShowOrOkAsync(string title, string message);

        Task ShowNotification(string title, string message, int timeClosePopup);
    }
}
