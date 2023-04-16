using ConstructorBot.ViewModel.ConstructorPageViewModel.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBot.SaveData
{
    public interface IStorage
    {
        public void Save(List<ActionBox> actions);
        public List<ActionBox> Get();
    }
}
