using ConstructorBot.Model.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBot.Services.ServiceStorage
{
    public interface IStorageService
    {
        public void SaveActions(List<ActionBox> actions);

        public List<ActionBox> GetActions();

        public void SetConnectionToken(string token);

        public string GetConnectionToken();

        bool GetOptions(string key, bool isNull = true);

        void SetOptions(string key, bool isOptions);
    }
}
