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

        public Task<string> ShowOrOkAsync(string title, string message)
        {
            throw new NotImplementedException();
        }
    }
}
