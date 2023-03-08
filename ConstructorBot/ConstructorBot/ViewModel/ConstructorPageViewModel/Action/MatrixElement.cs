using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBot.ViewModel.ConstructorPageViewModel.Action
{
    public class MatrixElement : INotifyPropertyChanged
    {
        public double _moveX;
        public double _moveY;
        protected double translationX;
        protected double translationY;
        double rotation;

        public virtual double TranslationX
        {
            get { return translationX; }
            set
            {
                translationX = value;
                OnPropertyChanged();
            }
        }

        public virtual double TranslationY
        {
            get { return translationY; }
            set
            {
                translationY = value;
                OnPropertyChanged();
            }
        }

        public double Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                OnPropertyChanged();
            }
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
