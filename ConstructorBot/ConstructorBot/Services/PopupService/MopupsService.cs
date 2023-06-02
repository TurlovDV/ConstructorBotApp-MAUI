using ConstructorBot.Mopups;
using Mopups.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBot.Services.PopupService
{
    class MopupsService : IMessageService
    {
        public async Task ShowAsync(string title, string message)
        {
            await MopupService.Instance.PushAsync(new PopupPage(title, message));
        }

        public async Task ShowNotification(string title, string message, int timeClosePopup)
        {
            var popup = new PopupNotificationPage(title, message);
            await MopupService.Instance.PushAsync(popup);
            //await Task.Run(async () =>
            //{
            await Task.Delay(timeClosePopup);
            await MopupService.Instance.RemovePageAsync(popup);
            //});            
        }

        public Task<string> ShowOrOkAsync(string title, string message)
        {
            throw new NotImplementedException();
        }
    }
}
