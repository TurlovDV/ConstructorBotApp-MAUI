using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBot.Converter
{
    class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dateTime = System.Convert.ToDateTime(value);
            string date = "";
            if (dateTime.Hour > 9)
                date += dateTime.Hour + ":";
            else
                date += "0" + dateTime.Hour + ":";
            if (dateTime.Minute > 9)
                date += dateTime.Minute + ":";
            else
                date += "0" + dateTime.Minute + ":";
            if (dateTime.Second > 9)
                date += dateTime.Second;
            else
                date += "0" + dateTime.Second;

            return date;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
