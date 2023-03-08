using ConstructorBot.ViewModel.ConstructorPageViewModel.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
using System.Text.Json.Nodes;
using ConstructorBot.ViewModel.ConstructorPageViewModel;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace ConstructorBot
{
    public static class SaveSettingOrActionBoxes
    {
        public static void Clear()
        {
            Preferences.Remove("Actions");
        }
        public static void Save(List<ActionBox> actionBoxes)
        {
            actionBoxes.ForEach(x =>
            {
                x.ConnectionActions.ToList()
                .ForEach(y =>
                {
                    y.OutConnect = null;
                    y.ConnectId = y.Connect.Id;
                    y.Connect = null;
                });
            });

            if (Preferences.ContainsKey("Actions"))
            {
                Preferences.Remove("Actions");
                Preferences.Set("Actions", Newtonsoft.Json.JsonConvert.SerializeObject(actionBoxes));                
            }
            else
                Preferences.Set("Actions", Newtonsoft.Json.JsonConvert.SerializeObject(actionBoxes));
            
            actionBoxes.ForEach(x =>
            {
                x.ConnectionActions.ToList()
                .ForEach(y =>
                {
                    y.OutConnect = x;
                    y.Connect = actionBoxes.First(f => f.Id == y.ConnectId);
                });
            });
        }

        public static List<ActionBox> Get()
        {
            if (!Preferences.ContainsKey("Actions"))
            {
                List<ActionBox> actionBoxes = new();
                ActionBoxBuilder actionBoxBuilder = new();
                actionBoxes.Add(actionBoxBuilder
                .BuildMessageText("Сообщение")
                .BuildQuestion("")
                .BuildMainAction()
                .BuildNaming("Start")
                .BuildColorNone(Colors.AliceBlue)
                .GetActionBox());

                Save(actionBoxes);

                return actionBoxes;
            }
            
            var actions = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ActionBox>>(Preferences.Get("Actions", null));

            ActionBoxBuilder actionBoxBuild = new();

            if (actions.Count == 0)
            {
                actions.Add(actionBoxBuild
                .BuildMessageText("Сообщение")
                .BuildQuestion("")
                .BuildMainAction()
                .BuildNaming("Start")
                .BuildColorNone(Colors.AliceBlue)
                .GetActionBox());
            }
            if (actions[0].IsMainAction != true)
            {
                actions[0] = actionBoxBuild
                .BuildMessageText("Сообщение")
                .BuildQuestion("")
                .BuildMainAction()
                .BuildNaming("Start")
                .BuildColorNone(Colors.AliceBlue)
                .GetActionBox();
            }

            actions.ForEach(x =>
            {
                int c = x.Keyboard.KeyboardItems.Count / 2;
                for (int i = 0; i < c; i++)
                {
                    x.Keyboard.KeyboardItems.RemoveAt(0);
                }
            
                x.ConnectionActions.ToList()
                    .ForEach(y =>
                    {                        
                        y.OutConnect = x;
                        y.Connect = actions.First(f => f.Id == y.ConnectId);
                    });
            });

            return actions;
        }
    }
}
