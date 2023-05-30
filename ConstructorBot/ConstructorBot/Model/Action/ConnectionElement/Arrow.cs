using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBot.Model.Action.ConnectionElement
{
    public class Arrow : MatrixElement, ICloneable
    {
        public object Clone()
        {
            return new
            {
                translationX = this.TranslationX,
                translationY = this.TranslationY,
                Rotation = this.Rotation
            };
        }
    }
}
