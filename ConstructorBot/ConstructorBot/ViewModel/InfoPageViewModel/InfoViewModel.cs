using ConstructorBot.Language;
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
                IntroTitle = LocalizationResourceManager.Instance["info1"].ToString(),
                IntroImage = "info1",
                GridRow = 1,
                LayoutOptions = LayoutOptions.Center
            });

            IntroScreens.Add(new IntroScreenModel
            {
                IntroTitle = LocalizationResourceManager.Instance["info2"].ToString(),
                IntroImage = "info2",
                GridRow = 1,
                LayoutOptions = LayoutOptions.Center
            });

            IntroScreens.Add(new IntroScreenModel
            {
                IntroTitle = LocalizationResourceManager.Instance["info3"].ToString(),
                IntroImage = "info3",
                GridRow = 3,
                LayoutOptions = LayoutOptions.Start
            });
            IntroScreens.Add(new IntroScreenModel
            {
                IntroTitle = LocalizationResourceManager.Instance["info4"].ToString(),
                IntroImage = "info4",
                GridRow = 3,
                LayoutOptions = LayoutOptions.Start
            });

            IntroScreens.Add(new IntroScreenModel
            {
                IntroTitle = LocalizationResourceManager.Instance["info5"].ToString(),
                IntroImage = "info5",
                GridRow = 1,
                LayoutOptions = LayoutOptions.Center
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
