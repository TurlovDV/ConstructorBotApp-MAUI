using ConstructorBot.ViewModel.ConstructorPageViewModel.Action.ConnectionElement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBot.ViewModel.ConstructorPageViewModel
{
    public class MatrixGrid
    {
        public Color Color { get; set; }

        public double Size { get; set; }

        public MatrixGrid(Color color, double size) 
        {
            Color = color;
            Size = size;
        }
    }
}
