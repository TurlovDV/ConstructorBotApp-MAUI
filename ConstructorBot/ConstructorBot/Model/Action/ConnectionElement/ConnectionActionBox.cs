using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBot.Model.Action.ConnectionElement
{
    public class ConnectionActionBox : ICloneable
    {
        public Guid Id { get; set; }

        public Arrow Arrow { get; set; }

        public Line Line { get; set; }

        public ActionBox Connect { get; set; }

        public Guid ConnectId { get; set; }

        public ActionBox OutConnect { get; set; }

        public ConnectionActionBox()
        {
            Id = Guid.NewGuid();
            Line = new Line();
            Arrow = new Arrow();
        }

        public void UpdateConnectionLine()
        {
            double X1 = Connect.TranslationX+10;
            double X2 = OutConnect.TranslationX+10;
            double Y1 = Connect.TranslationY+10;
            double Y2 = OutConnect.TranslationY+10;

            Line.HeightRequest = GetSize(new Point(X1, Y1), new Point(X2, Y2));
            Line.Rotation = GetCorner(new Point(X2, Y2),new Point(X1, Y1)) - 90;

            Arrow.TranslationX = (X1 + X2) / 2 - 10;
            Arrow.TranslationY = (Y1 + Y2) / 2 - 10;

            Line.TranslationX = Arrow.TranslationX;
            Line.TranslationY = Arrow.TranslationY;

            Arrow.Rotation = Line.Rotation - 90;
        }

        private double GetSize(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
        }

        public double GetCorner(Point a, Point b)
        {
            Point va = new Point(1, 0);
            Point vb = new Point(b.X - a.X, b.Y - a.Y);

            double числитель = va.X * vb.X + va.Y * vb.Y;
            double знаменатель = Math.Sqrt(va.X * va.X + va.Y * va.Y) * Math.Sqrt(vb.X * vb.X + vb.Y * vb.Y);
            double rotation = Math.Acos(числитель / знаменатель) * (180 / Math.PI);

            if (vb.Y > 0)
                return rotation;
            else
                return -rotation;
        }

        public object Clone()
        {
            return new
            {
                line = this.Line.Clone(),
                Arrow = this.Arrow.Clone(),
                Id = this.Id
            };
        }
    }
}
