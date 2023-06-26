using Microsoft.Maui.Controls.Shapes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBot.Converter.ChatViewConverter
{
    public class BoolToCornerRadiusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var IsBot = (bool)value;
            return new RoundRectangle()
            {
                CornerRadius = new CornerRadius(15, 15, IsBot ? 15 : 0, IsBot ? 0 : 15)
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
