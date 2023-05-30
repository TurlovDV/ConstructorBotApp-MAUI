using ConstructorBot.Language;
using ConstructorBot.Model.Action;
using ConstructorBotMessengerApi.HandlerLogicModel.ActionModel;
using ConstructorBotMessengerApi.Model.MessageModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBot.ViewModel.MainPageViewModel
{
    public static class LogicMatrixConverter
    {        
        private static ConstructorBotMessengerApi.Model.MessageModel.Keyboard GetKeyboard(ActionBox actionBox)
        {
            int column = 0;
            ConstructorBotMessengerApi.Model.MessageModel.Keyboard keyboard = new();
            keyboard.ArrayButton = new();
            List<DomainButton> domainButtons = new();

            foreach (var x in actionBox.Keyboard.KeyboardItems)
            {
                if (x.IsEnabled)
                    domainButtons.Add(new DomainButton() { 
                        text = x.Text == "" ? LocalizationResourceManager.Instance["Button"].ToString() : x.Text, 
                        callback_data = x.Text == "" ? LocalizationResourceManager.Instance["Button"].ToString() : x.Text
                    });

                if ((column + 1) % 3 == 0)
                {
                    if (domainButtons.Count > 0)
                    {
                        keyboard.ArrayButton.Add(domainButtons);
                        domainButtons = new();
                    }
                    column = 0;
                }
                else
                    column++;
            }

            keyboard.IsInline = actionBox.Keyboard.IsInline;

            if (keyboard.ArrayButton.Count > 0)
                return keyboard;
            else
                return null;
        }

        private static void GetMedia(ActionBox actionBox, ref ChildAction domainChild)
        {
            if (actionBox.MediaItems.Count == 0)
                return;

            if (actionBox.MediaItems.Count == 1)
            {
                domainChild.MessageAnswer.Photo = new Photo()
                {
                    File = actionBox.MediaItems[0].Bytes                    
                };
                domainChild.MessageAnswer.MessageType = MessageType.Photo;
                return;
            }

            domainChild.MessageAnswer.MessageType = MessageType.GroupMedia;
            domainChild.MessageAnswer.MediaGroup = new();
            foreach (var item in actionBox.MediaItems)
            {
                domainChild.MessageAnswer.MediaGroup.Add(new ConstructorBotMessengerApi.Model.MessageModel.MediaItem()
                {                    
                    type = ConstructorBotMessengerApi.Model.MessageModel.MediaType.Photo,
                    File = File.ReadAllBytes(item.PathMediaSource)                    
                });
            }
        }
        
        private static ChildAction GetChildAction(ActionBox actionBox)
        {
            ChildAction domainChild = new ChildAction();
            domainChild.Question = actionBox.Question;
            domainChild.MessageAnswer = new Message()
            {
                Text = actionBox.MessageText
            };

            domainChild.MessageAnswer.Keyboard = GetKeyboard(actionBox);

            GetMedia(actionBox, ref domainChild);

            domainChild.SaveName = actionBox.NameSaveMessage;

            return domainChild;
        }
        
        public static List<ParentAction> GetParentActions(List<ActionBox> _actions)
        {
            List<ParentAction> result = new List<ParentAction>();
            int numParent = 1;
            var logic = new List<ParentAction>();
            List<ActionBox> acts = new List<ActionBox>(_actions);

            
            var c = GetChildAction(acts[0]);
            c.Id = acts[0].Id;
            result.Add(new ParentAction()
            {
                NumberMainAction = 0,
                ChildActions = new List<ChildAction>()
                {
                    c
                }
            });

            foreach (var action in acts)
            {
                ParentAction parentAction = new ParentAction();
                parentAction.ChildActions = new List<ChildAction>();
                parentAction.NumberMainAction = numParent;
                parentAction.Id = action.Id;
                numParent++;
                if (action.ConnectionActions.Count == 0)
                    continue;

                foreach (var child in action.ConnectionActions)
                {
                    //ChildAction childAction = child.Connect.ChildAction;
                    ChildAction childAction = GetChildAction(_actions.First(x => x.Id == child.Connect.Id)); //new
                    childAction.Id = child.Connect.Id;//new
                    parentAction.ChildActions.Add(childAction);
                }
                result.Add(parentAction);
            }

            foreach (var parent in result)
            {
                if (parent.ChildActions == null)
                    continue;
                foreach (var child in parent.ChildActions)
                {
                    var action = acts.First(x => x.Id == child.Id); //Actions.
                    if (action.ConnectionActions.Count == 0)
                    {
                        child.ForwardAction = parent.NumberMainAction;
                    }
                    foreach (var connection in action.ConnectionActions)
                    {
                        foreach (var parent2 in result)
                        {
                            if (parent2.ChildActions == null)
                                continue;
                            foreach (var child2 in parent2.ChildActions)
                                if (child2.Id == connection.Connect.Id)
                                    child.ForwardAction = parent2.NumberMainAction;
                        }
                    }
                }
            }
            return result;
        }
    }
}
