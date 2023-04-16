using AutoMapper;
using ConstructorBot.ViewModel.ConstructorPageViewModel.Action;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ConstructorBot.SaveData
{
    public class Storage // : IStorage
    {
        private static MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg => 
        {
            cfg.CreateMap<ActionModel, ActionBox>().ReverseMap().ForAllMembers(x => x.Ignore());
            cfg.CreateMap<MediaItem, ViewModel.ConstructorPageViewModel.Action.MediaItem>().ReverseMap().ForAllMembers(x => x.Ignore());
            cfg.CreateMap<Line, ViewModel.ConstructorPageViewModel.Action.ConnectionElement.Line>().ReverseMap().ForAllMembers(x => x.Ignore());
            cfg.CreateMap<KeyboardItem, ViewModel.ConstructorPageViewModel.Action.KeyboardItem>().ReverseMap().ForAllMembers(x => x.Ignore());
            cfg.CreateMap<Keyboard, ViewModel.ConstructorPageViewModel.Action.Keyboard>().ReverseMap().ForAllMembers(x => x.Ignore());
            cfg.CreateMap<ConnectionAction, ViewModel.ConstructorPageViewModel.Action.ConnectionElement.ConnectionActionBox>().ReverseMap().ForAllMembers(x => x.Ignore());
            cfg.CreateMap<Arrow, ViewModel.ConstructorPageViewModel.Action.ConnectionElement.Arrow>().ReverseMap().ForAllMembers(x => x.Ignore());
        });

        public static List<ActionBox> GetActions()
        {
            var mapper = mapperConfiguration.CreateMapper();
            var getStringStorage = Preferences.Get("Actions", null);
            var myDeserializedClass = JsonConvert.DeserializeObject<List<ActionModel>>(getStringStorage);
            
            List<ActionBox> actions = myDeserializedClass.Select(x => 
                mapper.Map<ActionModel, ActionBox>(x)).ToList();

            actions.ForEach(x =>
            {
                x.ConnectionActions.ToList()
                    .ForEach(y =>
                    {
                        y.OutConnect = x;
                        y.Connect = actions.First(f => f.Id == y.ConnectId);
                    });
            });

            return actions;
        }

        public static void SaveActions(List<ActionBox> actionBoxes)
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

        public static void SetKey(string token)
        {
            Preferences.Set("telegram", token);
        }

        public static string GetKey()
        {
            return Preferences.Get("telegram", null);
        }
    }    
}
