using AutoMapper;
using ConstructorBot.Language;
using ConstructorBot.Model.Action;
using ConstructorBot.ViewModel.ConstructorPageViewModel;
using Microsoft.Maui.Layouts;
using Newtonsoft.Json;

namespace ConstructorBot.Services.ServiceStorage
{
    public class FileStorageService : IStorageService
    {
        private static MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ActionBoxDTO, ActionBox>().ReverseMap().ForAllMembers(x => x.Ignore());
            cfg.CreateMap<MediaItemDTO, MediaItem>().ReverseMap().ForAllMembers(x => x.Ignore());
            cfg.CreateMap<LineDTO, Model.Action.ConnectionElement.Line>().ReverseMap().ForAllMembers(x => x.Ignore());
            cfg.CreateMap<KeyboardItemDTO, Model.Action.KeyboardItem>().ReverseMap().ForAllMembers(x => x.Ignore());
            cfg.CreateMap<KeyboardDTO, Model.Action.Keyboard>().ReverseMap().ForAllMembers(x => x.Ignore());
            cfg.CreateMap<ConnectionActionDTO, Model.Action.ConnectionElement.ConnectionActionBox>().ReverseMap().ForAllMembers(x => x.Ignore());
            cfg.CreateMap<ArrowDTO, Model.Action.ConnectionElement.Arrow>().ReverseMap().ForAllMembers(x => x.Ignore());
        });        
                
        private List<ActionBox> cachedActionBoxes;
        private string cachedConnectionToken;

        public FileStorageService()
        {
            GetActions();
            GetConnectionToken();
        }

        public List<ActionBox> GetActions()
        {
            if (cachedActionBoxes != null)
                return cachedActionBoxes;

            var mapper = mapperConfiguration.CreateMapper();
            var getStringStorage = Preferences.Get("Actions", null);
            List<ActionBox> actions = new();

            if (getStringStorage is not null)
            {
                var myDeserializedClass = JsonConvert.DeserializeObject<List<ActionBoxDTO>>(getStringStorage);

                actions = myDeserializedClass.Select(x =>
                    mapper.Map<ActionBoxDTO, ActionBox>(x)).ToList();

                actions.ForEach(x =>
                {
                    x.ConnectionActions.ToList()
                        .ForEach(y =>
                        {
                            y.OutConnect = x;
                            y.Connect = actions.First(f => f.Id == y.ConnectId);
                        });
                });
            }

            if (actions.Count == 0)
            {
                actions.Add(new ActionBoxBuilder()
                    .BuildMessageText(LocalizationResourceManager.Instance["Message"].ToString())
                    .BuildQuestion("")
                    .BuildNaming("Start")
                    .BuildMainAction()
                    .GetActionBox());
            }

            cachedActionBoxes = actions;
            return actions;
        }

        public void SaveActions(List<ActionBox> actionBoxes)
        {
            cachedActionBoxes = actionBoxes;

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

            Preferences.Set("Actions", JsonConvert.SerializeObject(actionBoxes));

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


        public void SetOptions(string key, bool isOptions)
        {
            Preferences.Default.Set<bool>(key, isOptions);
        }

        public bool GetOptions(string key, bool isNull = true)
        {

            if (!Preferences.ContainsKey(key))
                SetOptions(key, isNull);
            else 
                return Preferences.Default.Get<bool>(key, isNull);

            return isNull;
        }

        public void SetConnectionToken(string token)
        {
            cachedConnectionToken = token;
            Preferences.Set("telegram", token);
        }

        public string GetConnectionToken()
        {           
            if(cachedConnectionToken != null)
                return cachedConnectionToken;

            return Preferences.Get("telegram", null) ?? "";
        }
    }
}
