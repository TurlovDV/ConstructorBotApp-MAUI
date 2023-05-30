using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBot.Model.Action
{
    public class Keyboard : INotifyPropertyChanged
    {
        private bool isInline;
        private ObservableCollection<KeyboardItem> keyboardItems;
        public ObservableCollection<KeyboardItem> KeyboardItems
        {
            get => keyboardItems;
            set
            {
                keyboardItems = value;
                OnPropertyChanged();
            }
        }

        public bool IsInline 
        {
            get => isInline; 
            set
            {
                isInline = value;
                OnPropertyChanged();
            }
        }

        public Keyboard()
        {
            KeyboardItems = new ObservableCollection<KeyboardItem>();
            //keyboardItems = new KeyboardItem[2,3];
            //for (int i = 0; i < keyboardItems.Length; i++)
            //    for (int j = 0; j < 3; j++)
            //        keyboardItems[i,j] = new KeyboardItem();

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    KeyboardItems.Add(new KeyboardItem()
                    {
                        Row = i,
                        Column = j                        
                    });


            //keyboardItems = new List<List<KeyboardItem>>()
            //{
            //    new List<KeyboardItem>()
            //    {
            //        new (),
            //        new (),
            //        new ()
            //    },
            //    new List<KeyboardItem>()
            //    {
            //        new (),
            //        new (),
            //        new ()
            //    },
            //    new List<KeyboardItem>()
            //    {
            //        new (),
            //        new (),
            //        new ()
            //    }
            //};
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
