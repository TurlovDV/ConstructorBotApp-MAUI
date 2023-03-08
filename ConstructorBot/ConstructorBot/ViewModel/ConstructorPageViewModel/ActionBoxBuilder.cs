using ConstructorBot.ViewModel.ConstructorPageViewModel.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBot.ViewModel.ConstructorPageViewModel
{
    public class ActionBoxBuilder
    {
        private ActionBox _action;

        public ActionBoxBuilder() =>
            _action = new();

        public ActionBox GetActionBox()
        {
            ActionBox actionBox = _action;
            _action = new();
            return actionBox;
        }

        public ActionBoxBuilder BuildColorNone(Color color)
        {
            _action.ColorNone = color;
            return this;
        }

        public ActionBoxBuilder BuildMessageText(string messageText)
        {
            _action.MessageText = messageText;
            return this;
        }

        public ActionBoxBuilder BuildQuestion(string question)
        {
            _action.Question = question;  
            return this;
        }

        public ActionBoxBuilder BuildMainAction()
        {
            _action.IsMainAction = true;
            return this;
        }

        public ActionBoxBuilder BuildNaming(string naming)
        {
            _action.Naming = naming;
            return this;
        }

        //public ActionBoxBuilder BuildTranslation(double translationX, double translationY)
        //{
        //    _action.TranslationX = translationX;
        //    _action.TranslationY = translationY;
        //    return this;
        //}

        public ActionBoxBuilder BuildRotation(double rotation)
        {
            _action.Rotation = rotation;
            return this;
        }
    }
}
