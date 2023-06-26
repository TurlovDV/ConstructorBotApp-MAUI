using AutoMapper.Execution;
using CommunityToolkit.Maui.Views;
using ConstructorBot.Model.Action;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Keyboard = ConstructorBot.Model.Action.Keyboard;

namespace ConstructorBot.Controls
{
    public class MessageItem : Border
    {
        #region Properties
        public static readonly BindableProperty MessageProperty =
            BindableProperty.Create(nameof(Message), typeof(string), typeof(MessageItem), "null");
        
        public static readonly BindableProperty DateTimeProperty =
            BindableProperty.Create(nameof(DateTime), typeof(string), typeof(MessageItem), "null");

        public static readonly BindableProperty KeyboardProperty =
            BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(MessageItem), null);

        public static readonly BindableProperty StackButtonsProperty =
            BindableProperty.Create(nameof(StackButtons), typeof(StackLayout), typeof(MessageItem), null);
        #endregion
        public string DateTime
        {
            get => (string)GetValue(DateTimeProperty);
            set
            {
                SetValue(DateTimeProperty, value);
            }
        }

        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set
            {
                SetValue(MessageProperty, value);
            }
        }

        public StackLayout StackButtons
        {
            get => (StackLayout)GetValue(StackButtonsProperty);
            set
            {
                SetValue(StackButtonsProperty, value);
            }
        }

        public Keyboard Keyboard
        { 
            get
            {
                return GetValue(KeyboardProperty) as Keyboard;
                //return new Keyboard()
                //{
                //    KeyboardItems = new System.Collections.ObjectModel.ObservableCollection<KeyboardItem>()
                //    {
                //        new()
                //        {
                //            Text = "Hello",
                //            Row = 0,
                //            Column = 0
                //        },
                //        new()
                //        {
                //            Text = "Hello",
                //            Row = 0,
                //            Column = 1
                //        },
                //        new()
                //        {
                //            Text = "Hello",
                //            Row = 0,
                //            Column = 2
                //        },
                //        new()
                //        {
                //            Text = "Hello",
                //            Row = 1,
                //            Column = 0
                //        },
                //        new()
                //        {
                //            Text = "Hello",
                //            Row = 1,
                //            Column = 1
                //        },
                //        new()
                //        {
                //            Text = "Hello",
                //            Row = 1,
                //            Column = 2,
                //            IsEnabled = true
                //        },
                //        new()
                //        {
                //            Text = "Hello",
                //            Row = 2,
                //            Column = 0,
                //            IsEnabled = true
                //        },
                //        new()
                //        {
                //            Text = "Hello",
                //            Row = 2,
                //            Column = 1,
                //            IsEnabled = true
                //        },
                //        new()
                //        {
                //            Text = "Hello",
                //            Row = 2,
                //            Column = 2
                //        },
                //    }
                //};
            }//GetValue(MessageProperty) as Keyboard;
            set
            {
                SetValue(KeyboardProperty, value);
            }
        }

        public MessageItem()
        {
            var stackLayout = new StackLayout();

            var text = new Label()
            {
                TextColor = Colors.White
            };
            text.BindingContext = this;
            text.SetBinding(Label.TextProperty, "Message");
            stackLayout.Add(text);

            var dateTime = new Label()
            {
                TextColor = Color.Parse("#7A7A7A"),
                FontSize = 11,
                HorizontalOptions = LayoutOptions.End
            };
            dateTime.BindingContext = this;
            dateTime.SetBinding(Label.TextProperty, "DateTime");
            stackLayout.Add(dateTime);

            var border = new Border();
            border.BackgroundColor = Colors.Transparent;
            border.Stroke = Colors.Transparent;
            border.BindingContext = this;
            border.SetBinding(Border.ContentProperty, "StackButtons");

            stackLayout.Add(border);
            
            //if (Keyboard != null)
            //    stackLayout.Add(GetStackLayoutKeyboard(Keyboard));

            this.Content = stackLayout;
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
