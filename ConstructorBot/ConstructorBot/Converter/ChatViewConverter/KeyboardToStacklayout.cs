using ConstructorBot.Model.Action;
using Microsoft.Maui.Controls.Shapes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Keyboard = ConstructorBot.Model.Action.Keyboard;

namespace ConstructorBot.Converter.ChatViewConverter
{
    class KeyboardToStacklayout : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is not null)
                return GetStackLayoutKeyboard(value as Keyboard);
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public StackLayout GetStackLayoutKeyboard(Keyboard keyboard)
        {
            StackLayout result = new StackLayout();

            StackLayout stackButtons = new StackLayout();
            for (int row = 0; row < 3; row++)
            {
                stackButtons = new StackLayout();
                stackButtons.Orientation = StackOrientation.Horizontal;
                stackButtons.HorizontalOptions = LayoutOptions.Center;

                stackButtons.Spacing = 5;
                for (int column = 0; column < 3; column++)
                {
                    var keyboardItem = keyboard.KeyboardItems.ToList()
                               .Where(x => x.Column == column && x.Row == row).First();

                    if (keyboardItem.IsEnabled)
                    {
                        stackButtons.Add(new Border()
                        {
                            StrokeShape = new RoundRectangle()
                            {
                                CornerRadius = new CornerRadius(3, 3, 3, 3)
                            },
                            Padding = new Thickness(10, 5, 10, 5),
                            BackgroundColor = Color.Parse("#33334A"),
                            Content = new Label()
                            {
                                Text = keyboardItem.Text,
                                FontSize = 12
                                //FontAttributes = FontAttributes.Bold
                            }
                        });
                    }
                }

                if (stackButtons.Count != 0)
                    result.Add(stackButtons);
            }

            return result.Count == 0 ? null : result;
        }
    }
}
