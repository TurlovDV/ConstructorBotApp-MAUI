using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBot.ViewModel.ConstructorPageViewModel.Action.ConnectionElement.Converters
{
    class IsAnyToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if ((parameter as Label).Text is null)
            //    return null;

            //bool isAny = System.Convert.ToBoolean((parameter as Label).Text);
            //if (isAny)
            //    return "Любая фраза";
            //else
            //    return value;

            if (value.ToString() == "")
                return "Любая фраза";
            else
                return value;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
        //public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        //{
        //    throw new NotImplementedException();
        //}

        //public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
