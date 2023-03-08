using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBot.ViewModel.ConstructorPageViewModel.Action.ConnectionElement
{
    public class Line : MatrixElement, ICloneable
    {
        private double heightRequest;

        //public double X2;
        //public double Y2;
        public double HeightRequest
        {
            get { return heightRequest; }
            set 
            {
                heightRequest = value;
                OnPropertyChanged();
            }
        }

        public object Clone()
        {
            return new
            {
                HeightRequest = this.HeightRequest,
                translationX = this.TranslationX,
                translationY = this.TranslationY,
                Rotation = this.Rotation
            };
        }
    }
}
