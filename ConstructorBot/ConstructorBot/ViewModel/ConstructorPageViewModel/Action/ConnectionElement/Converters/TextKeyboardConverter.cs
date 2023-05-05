using ConstructorBot.Language;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBot.ViewModel.ConstructorPageViewModel.Action.ConnectionElement.Converters
{
    class TextKeyboardConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == "")
                return LocalizationResourceManager.Instance["Button"];
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
