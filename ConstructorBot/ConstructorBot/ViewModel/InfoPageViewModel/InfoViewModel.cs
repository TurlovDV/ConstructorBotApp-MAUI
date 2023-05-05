using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ConstructorBot.ViewModel.InfoPageViewModel
{
    internal class InfoViewModel : INotifyPropertyChanged
    {
        #region Properties

        private string _buttonText = "Далее";
        public string ButtonText
        {
            get => _buttonText;
            set
            {
                _buttonText = value;
                OnPropertyChanged();
            }
        }

        private int _position;
        public int Position
        {
            get => _position;
            set 
            {
                _position = value;

                OnPropertyChanged();

                if (value == IntroScreens.Count)
                { 
                    AppShell.Current.GoToAsync($"..");
                    ButtonText = "Старт";
                }
                else
                    ButtonText = "Далее";
            }
        }

        public ObservableCollection<IntroScreenModel> IntroScreens { get; set; } = new ObservableCollection<IntroScreenModel>();
        #endregion

        public InfoViewModel()
        {
            IntroScreens.Add(new IntroScreenModel
            {
                IntroTitle = "Order Your Food",
                IntroDescription = "Now you can order food anytime right from mobile",
                IntroImage = "info1"
            });

            IntroScreens.Add(new IntroScreenModel
            {
                IntroTitle = "Cooking Safe Food",
                IntroDescription = "We are maintain safty and we keep clean while making food",
                IntroImage = "info2"
            });

            IntroScreens.Add(new IntroScreenModel
            {
                IntroTitle = "Quick Delivery",
                IntroDescription = "Orders your favorite meals will be immediately deliver.",
                IntroImage = "info3"
            });
            IntroScreens.Add(new IntroScreenModel
            {
                IntroTitle = "Order Your Food",
                IntroDescription = "Now you can order food anytime right from mobile",
                IntroImage = "info4"
            });

            IntroScreens.Add(new IntroScreenModel
            {
                IntroTitle = "Cooking Safe Food",
                IntroDescription = "We are maintain safty and we keep clean while making food",
                IntroImage = "info5"
            });
        }



        public ICommand NextCommand => new Command(async () =>
        {
            if (Position >= IntroScreens.Count - 1)
            {
                await AppShell.Current.GoToAsync($"//{nameof(MainPage)}");
            }
            else
            {
                Position += 1;
            }
        });
        
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
